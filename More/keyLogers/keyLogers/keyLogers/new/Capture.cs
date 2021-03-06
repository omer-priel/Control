// ------------------------------------------------------------------
// DirectX.Capture
//
// History:
//	2003-Jan-24		BL		- created
//
// Future Improvements
// 	- Notify unusual events (disk full, quick logoff) using IMediaEventEx (see AMCap line 1624)
// 	- Capture stats: IAMDroppedFrames, IQualProp.get_AvgFrameRate
// 	- Show preview while cued: ICaptureGraphBuilder2.ControlStream lets you stop capture but continue preview
// 	- Pre-alloc space with ICaptureGraphBuilder2.AllocCapFile(), copy data using ICaptureGraphBuilder2.CopyCaptureFile()
// 	- Audio preview (why doesn't VirtualDub do audio preview?)
// 	- Hardware Deinterlacing: "Setting Deinterlace Preferences", or IVMRDeinterlaceControl9
// 	- Time limit (and TimeRemaining) -> AmCap manually handles the time limit
//  - Don't destory filter graph when changing compressors, just remove compressor and add new one
//  - Change VideoFormat (NTSC/PAL), I think you need to use IAMVideoDecoder (WDM only)
//
// Copyright (c) 2003 Brian Low
// ------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices; 
using System.Threading;
using System.Windows.Forms;
using DShowNET;



																			   

namespace DirectX.Capture
{
	public class Capture
	{
		protected enum GraphState
		{
			Null,			// No filter graph at all
			Created,		// Filter graph created with device filters added
			Rendered,		// Filter complete built, ready to run (possibly previewing)
			Capturing		// Filter is capturing
		}



		// ------------------ Public Properties --------------------

		public bool Capturing { get { return( graphState==GraphState.Capturing ); } }

		public bool Cued { get { return( isCaptureRendered && graphState==GraphState.Rendered ); } }

		public bool Stopped { get { return( graphState!=GraphState.Capturing ); } }
		
		public string Filename 
		{ 
			get { return( filename ); } 
			set 
			{ 
				assertStopped();
				if ( Cued )
					throw new InvalidOperationException( "The Filename cannot be changed once cued. Use Stop() before changing the filename." );
				filename = value; 
				if ( fileWriterFilter != null )
				{
					string s;
					AMMediaType mt = new AMMediaType(); 
					int hr = fileWriterFilter.GetCurFile( out s, mt );
					if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
					if ( mt.formatSize > 0 )
						Marshal.FreeCoTaskMem( mt.formatPtr ); 
					hr = fileWriterFilter.SetFileName( filename, mt );
					if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
				}
			} 
		}

		public Control PreviewWindow
		{
			get { return( previewWindow ); }
			set
			{
				assertStopped();
				derenderGraph();
				previewWindow = value;
				wantPreviewRendered = ( ( previewWindow != null ) && ( videoDevice != null ) );
				renderGraph();
				startPreviewIfNeeded();
			}
		}

		public VideoCapabilities VideoCaps 
		{ 
			get 
			{ 
				if ( videoCaps == null )
				{
					if ( videoStreamConfig != null )
					{
						try 
						{ 
							videoCaps = new VideoCapabilities( videoStreamConfig ); 
						}
						catch ( Exception ex ) { Debug.WriteLine( "VideoCaps: unable to create videoCaps." + ex.ToString() ); }
					}
				}
				return( videoCaps ); 
			} 
		}
		public AudioCapabilities AudioCaps 
		{ 
			get 
			{ 
				if ( audioCaps == null )
				{
					if ( audioStreamConfig != null )
					{
						try 
						{ 
							audioCaps = new AudioCapabilities( audioStreamConfig ); 
						}
						catch ( Exception ex ) { Debug.WriteLine( "AudioCaps: unable to create audioCaps." + ex.ToString() ); }
					}
				}
				return( audioCaps ); 
			} 
		}

		public Filter VideoDevice { get { return( videoDevice ); } }
		public Filter AudioDevice { get { return( audioDevice ); } }
		public Filter VideoCompressor { 
			get { return( videoCompressor ); } 
			set 
			{ 
				assertStopped();
				destroyGraph();
				videoCompressor = value;
				renderGraph();
				startPreviewIfNeeded();
			}
		}
		public Filter AudioCompressor 
		{ 
			get { return( audioCompressor ); } 
			set 
			{ 
				assertStopped();
				destroyGraph();
				audioCompressor = value;
				renderGraph();
				startPreviewIfNeeded();
			}
		}

		public Source VideoSource 
		{ 
			get { return( VideoSources.CurrentSource ); } 
			set { VideoSources.CurrentSource = value; } 
		}

		public Source AudioSource 
		{
			get { return( AudioSources.CurrentSource ); } 
			set { AudioSources.CurrentSource = value; }
		}

		public SourceCollection VideoSources
		{
			get 
			{ 
				if ( videoSources == null )
				{
					try
					{
						if ( videoDevice != null )
							videoSources = new SourceCollection( captureGraphBuilder, videoDeviceFilter, true );
						else
							videoSources = new SourceCollection();
					}
					catch ( Exception ex ) { Debug.WriteLine( "VideoSources: unable to create VideoSources." + ex.ToString() ); }
				}
				return ( videoSources );
			}
		}

		public SourceCollection AudioSources
		{
			get 
			{ 
				if ( audioSources == null )
				{
					try
					{
						if ( audioDevice != null )
							audioSources = new SourceCollection( captureGraphBuilder, audioDeviceFilter, false );
						else
							audioSources = new SourceCollection();
					}
					catch ( Exception ex ) { Debug.WriteLine( "AudioSources: unable to create AudioSources." + ex.ToString() ); }
				}
				return ( audioSources );
			}
		}

		public PropertyPageCollection PropertyPages 
		{
			get
			{
				if ( propertyPages == null )
				{
					try 
					{ 
						propertyPages = new PropertyPageCollection( 
							captureGraphBuilder, 
							videoDeviceFilter, audioDeviceFilter, 
							videoCompressorFilter, audioCompressorFilter, 
							VideoSources, AudioSources );
					}
					catch ( Exception ex ) { Debug.WriteLine( "PropertyPages: unable to get property pages." + ex.ToString() ); }

				}
				return( propertyPages );
			}
		}

		public Tuner Tuner { get { return( tuner ); } }
		public double FrameRate
		{
			get
			{
				long avgTimePerFrame = (long) getStreamConfigSetting( videoStreamConfig, "AvgTimePerFrame" );
				return( (double) 10000000 / avgTimePerFrame );
			}
			set
			{
				long avgTimePerFrame = (long) ( 10000000 / value );
				setStreamConfigSetting( videoStreamConfig, "AvgTimePerFrame", avgTimePerFrame );
			}
		}

		public Size FrameSize
		{
			get
			{
				BitmapInfoHeader bmiHeader;
				bmiHeader = (BitmapInfoHeader) getStreamConfigSetting( videoStreamConfig, "BmiHeader" );
				Size size = new Size( bmiHeader.Width, bmiHeader.Height );
				return( size );
			}
			set
			{
				BitmapInfoHeader bmiHeader;
				bmiHeader = (BitmapInfoHeader) getStreamConfigSetting( videoStreamConfig, "BmiHeader" );
				bmiHeader.Width = value.Width;
				bmiHeader.Height = value.Height;
				setStreamConfigSetting( videoStreamConfig, "BmiHeader", bmiHeader );
			}		
		}

		public short AudioChannels
		{
			get
			{
				short audioChannels = (short) getStreamConfigSetting( audioStreamConfig, "nChannels" );
				return( audioChannels );
			}
			set
			{
				setStreamConfigSetting( audioStreamConfig, "nChannels", value );
			}
		}

		public int AudioSamplingRate
		{
			get
			{
				int samplingRate = (int) getStreamConfigSetting( audioStreamConfig, "nSamplesPerSec" );
				return( samplingRate );
			}
			set
			{
				setStreamConfigSetting( audioStreamConfig, "nSamplesPerSec", value );
			}
		}
		public short AudioSampleSize
		{
			get
			{
				short sampleSize = (short) getStreamConfigSetting( audioStreamConfig, "wBitsPerSample" );
				return( sampleSize );
			}
			set
			{
				setStreamConfigSetting( audioStreamConfig, "wBitsPerSample", value );
			}
		}

		public event EventHandler					CaptureComplete;

		protected GraphState		graphState = GraphState.Null;		// State of the internal filter graph
		protected bool				isPreviewRendered = false;			// When graphState==Rendered, have we rendered the preview stream?
		protected bool				isCaptureRendered = false;			// When graphState==Rendered, have we rendered the capture stream?
		protected bool				wantPreviewRendered = false;		// Do we need the preview stream rendered (VideoDevice and PreviewWindow != null)
		protected bool				wantCaptureRendered = false;		// Do we need the capture stream rendered

		protected int				rotCookie = 0;						// Cookie into the Running Object Table
		protected Filter			videoDevice = null;					// Property Backer: Video capture device filter
		protected Filter			audioDevice = null;					// Property Backer: Audio capture device filter
		protected Filter			videoCompressor = null;				// Property Backer: Video compression filter
		protected Filter			audioCompressor = null;				// Property Backer: Audio compression filter
		protected string			filename = "";						// Property Backer: Name of file to capture to
		protected Control			previewWindow = null;				// Property Backer: Owner control for preview
		protected VideoCapabilities	videoCaps = null;					// Property Backer: capabilities of video device
		protected AudioCapabilities	audioCaps = null;					// Property Backer: capabilities of audio device
		protected SourceCollection	videoSources = null;				// Property Backer: list of physical video sources
		protected SourceCollection	audioSources = null;				// Property Backer: list of physical audio sources
		protected PropertyPageCollection propertyPages = null;			// Property Backer: list of property pages exposed by filters
		protected Tuner				tuner = null;						// Property Backer: TV Tuner

		protected IGraphBuilder		graphBuilder;						// DShow Filter: Graph builder 
		protected IMediaControl		mediaControl;						// DShow Filter: Start/Stop the filter graph -> copy of graphBuilder
		protected IVideoWindow		videoWindow;						// DShow Filter: Control preview window -> copy of graphBuilder
		protected ICaptureGraphBuilder2		captureGraphBuilder = null;	// DShow Filter: building graphs for capturing video
		protected IAMStreamConfig	videoStreamConfig = null;			// DShow Filter: configure frame rate, size
		protected IAMStreamConfig	audioStreamConfig = null;			// DShow Filter: configure sample rate, sample size
		protected IBaseFilter		videoDeviceFilter = null;			// DShow Filter: selected video device
		protected IBaseFilter		videoCompressorFilter = null;		// DShow Filter: selected video compressor
		protected IBaseFilter		audioDeviceFilter = null;			// DShow Filter: selected audio device
		protected IBaseFilter		audioCompressorFilter = null;		// DShow Filter: selected audio compressor
		protected IBaseFilter		muxFilter = null;					// DShow Filter: multiplexor (combine video and audio streams)
		protected IFileSinkFilter	fileWriterFilter = null;			// DShow Filter: file writer
		
		public Capture(Filter videoDevice, Filter audioDevice)
		{
			if ( videoDevice == null && audioDevice == null )
				throw new ArgumentException( "The videoDevice and/or the audioDevice parameter must be set to a valid Filter.\n" );
			this.videoDevice = videoDevice;
			this.audioDevice = audioDevice;
			this.Filename = getTempFilename();
			createGraph();
		}

		~Capture()
		{
			Dispose();
		}


		public void Cue()
		{
			assertStopped();

			wantCaptureRendered = true;

			renderGraph();

			int hr = mediaControl.Pause();
			if ( hr != 0 ) Marshal.ThrowExceptionForHR( hr ); 
		}

		public void Start()
		{
			assertStopped();

			// We want the capture stream rendered
			wantCaptureRendered = true;

			// Re-render the graph (if necessary)
			renderGraph();

			// Start the filter graph: begin capturing
			int hr = mediaControl.Run();
			if ( hr != 0 ) Marshal.ThrowExceptionForHR( hr ); 

			// Update the state
			graphState = GraphState.Capturing;
		}

		public void Stop()
		{
			wantCaptureRendered = false;
			if ( mediaControl != null )
			{
				mediaControl.Stop();
			}

			if ( graphState == GraphState.Capturing )
			{
				graphState = GraphState.Rendered;
				if ( CaptureComplete != null )
					CaptureComplete( this, null );
			}
			try { renderGraph(); } catch {}
			try { startPreviewIfNeeded(); } catch {}
		}

		public void Dispose()
		{
			wantPreviewRendered = false;
			wantCaptureRendered = false;
			CaptureComplete = null;

			try { destroyGraph(); } catch {}

			if ( videoSources != null )
				videoSources.Dispose(); videoSources = null;
			if ( audioSources != null )
				audioSources.Dispose(); audioSources = null;

		}

		protected void createGraph()
		{
			Guid					cat;
			Guid					med;
			int						hr;

			if ( videoDevice == null && audioDevice == null )
				throw new ArgumentException( "The video and/or audio device have not been set. Please set one or both to valid capture devices.\n" );

			if ( (int)graphState < (int)GraphState.Created )
			{
				GC.Collect();

				graphBuilder = (IGraphBuilder) Activator.CreateInstance( Type.GetTypeFromCLSID( Clsid.FilterGraph, true ) ); 

				Guid clsid = Clsid.CaptureGraphBuilder2;
				Guid riid = typeof(ICaptureGraphBuilder2).GUID;
				captureGraphBuilder = (ICaptureGraphBuilder2) DsBugWO.CreateDsInstance( ref clsid, ref riid ); 

				hr = captureGraphBuilder.SetFiltergraph( graphBuilder );
				if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );

				#if DEBUG
				DsROT.AddGraphToRot( graphBuilder, out rotCookie );
				#endif

				if ( VideoDevice != null )
				{
					videoDeviceFilter = (IBaseFilter) Marshal.BindToMoniker( VideoDevice.MonikerString );
					hr = graphBuilder.AddFilter( videoDeviceFilter, "Video Capture Device" );
					if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
				}

				if ( AudioDevice != null )
				{
					audioDeviceFilter = (IBaseFilter) Marshal.BindToMoniker( AudioDevice.MonikerString );
					hr = graphBuilder.AddFilter( audioDeviceFilter, "Audio Capture Device" );
					if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
				}

				if ( VideoCompressor != null )
				{
					videoCompressorFilter = (IBaseFilter) Marshal.BindToMoniker( VideoCompressor.MonikerString ); 
					hr = graphBuilder.AddFilter( videoCompressorFilter, "Video Compressor" );
					if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
				}

				if ( AudioCompressor != null )
				{
					audioCompressorFilter = (IBaseFilter) Marshal.BindToMoniker( AudioCompressor.MonikerString ); 
					hr = graphBuilder.AddFilter( audioCompressorFilter, "Audio Compressor" );
					if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
				}

				object o;
				cat = PinCategory.Capture;
				med = MediaType.Interleaved;
				Guid iid = typeof(IAMStreamConfig).GUID;
				hr = captureGraphBuilder.FindInterface(
					ref cat, ref med, videoDeviceFilter, ref iid, out o );

				if ( hr != 0 )
				{
					med = MediaType.Video;
					hr = captureGraphBuilder.FindInterface(
						ref cat, ref med, videoDeviceFilter, ref iid, out o );
				
					if ( hr != 0 )
						o = null;
				}
				videoStreamConfig = o as IAMStreamConfig;

				o = null;
				cat = PinCategory.Capture;
				med = MediaType.Audio ;
				iid = typeof(IAMStreamConfig).GUID;
				hr = captureGraphBuilder.FindInterface(
					ref cat, ref med, audioDeviceFilter, ref iid, out o );
				if ( hr != 0 )
					o = null;
				audioStreamConfig = o as IAMStreamConfig;

				mediaControl = (IMediaControl) graphBuilder;

				if ( videoSources != null ) videoSources.Dispose(); videoSources = null;

				if ( audioSources != null ) audioSources.Dispose(); audioSources = null;
				
				if ( propertyPages != null ) propertyPages.Dispose(); propertyPages = null;

				videoCaps = null;

				audioCaps = null;

				o = null;
				cat = PinCategory.Capture;
				med = MediaType.Interleaved; 
				iid = typeof(IAMTVTuner).GUID;
				hr = captureGraphBuilder.FindInterface( 
					ref cat, ref med, videoDeviceFilter, ref iid, out o );
				if ( hr != 0 )
				{
					med = MediaType.Video ;
					hr = captureGraphBuilder.FindInterface( 
						ref cat, ref med, videoDeviceFilter, ref iid, out o );
					if ( hr != 0 )
						o = null;
				}
				IAMTVTuner t = o as IAMTVTuner;
				if ( t != null )
					tuner = new Tuner( t );

				graphState = GraphState.Created;
			}
		}

		protected void renderGraph()
		{
			Guid					cat;
			Guid					med;
			int						hr;
			bool					didSomething = false;
			const int WS_CHILD			= 0x40000000;	
			const int WS_CLIPCHILDREN	= 0x02000000;
			const int WS_CLIPSIBLINGS	= 0x04000000;

			assertStopped();

			if ( filename == null )
				throw new ArgumentException( "The Filename property has not been set to a file.\n" );

			if ( mediaControl != null )
				mediaControl.Stop();

			createGraph();
			if ( !wantPreviewRendered && isPreviewRendered )
				derenderGraph();
			if ( !wantCaptureRendered && isCaptureRendered )
				if ( wantPreviewRendered )
					derenderGraph();


			// Render capture stream (only if necessary)
			if ( wantCaptureRendered && !isCaptureRendered )
			{
				// Render the file writer portion of graph (mux -> file)
				Guid mediaSubType = MediaSubType.Avi;
				hr = captureGraphBuilder.SetOutputFileName( ref mediaSubType, Filename, out muxFilter, out fileWriterFilter );
				if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );

				// Render video (video -> mux)
				if ( VideoDevice != null )
				{
					// Try interleaved first, because if the device supports it,
					// it's the only way to get audio as well as video
					cat = PinCategory.Capture;
					med = MediaType.Interleaved;
					hr = captureGraphBuilder.RenderStream( ref cat, ref med, videoDeviceFilter, videoCompressorFilter, muxFilter ); 
					if( hr < 0 ) 
					{
						med = MediaType.Video;
						hr = captureGraphBuilder.RenderStream( ref cat, ref med, videoDeviceFilter, videoCompressorFilter, muxFilter ); 
						if ( hr == -2147220969 ) throw new DeviceInUseException( "Video device", hr );
						if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
					}
				}

				// Render audio (audio -> mux)
				if ( AudioDevice != null )
				{
					cat = PinCategory.Capture;
					med = MediaType.Audio;
					hr = captureGraphBuilder.RenderStream( ref cat, ref med, audioDeviceFilter, audioCompressorFilter, muxFilter );
					if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );
				}

				isCaptureRendered = true;
				didSomething = true;
			}

			// Render preview stream (only if necessary)
			if ( wantPreviewRendered && !isPreviewRendered )
			{
				// Render preview (video -> renderer)
				cat = PinCategory.Preview;
				med = MediaType.Video;
				hr = captureGraphBuilder.RenderStream( ref cat, ref med, videoDeviceFilter, null, null ); 
				if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );

				// Get the IVideoWindow interface
				videoWindow = (IVideoWindow) graphBuilder;

				// Set the video window to be a child of the main window
				hr = videoWindow.put_Owner( previewWindow.Handle );
				if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );

				// Set video window style
				hr = videoWindow.put_WindowStyle( WS_CHILD | WS_CLIPCHILDREN | WS_CLIPSIBLINGS);
				if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );

				// Position video window in client rect of owner window
				previewWindow.Resize += new EventHandler( onPreviewWindowResize );
				onPreviewWindowResize( this, null );

				// Make the video window visible, now that it is properly positioned
				hr = videoWindow.put_Visible( DsHlp.OATRUE );
				if( hr < 0 ) Marshal.ThrowExceptionForHR( hr );

				isPreviewRendered = true;
				didSomething = true;
			}

			if ( didSomething )
				graphState = GraphState.Rendered;
		}

		/// <summary>
		///  Setup and start the preview window if the user has
		///  requested it (by setting PreviewWindow).
		/// </summary>
		protected void startPreviewIfNeeded()
		{
			// Render preview 
			if ( wantPreviewRendered && isPreviewRendered && !isCaptureRendered )
			{
				// Run the graph (ignore errors)
				// We can run the entire graph becuase the capture
				// stream should not be rendered (and that is enforced
				// in the if statement above)
				mediaControl.Run();
			}
		}

		/// <summary>
		///  Disconnect and remove all filters except the device
		///  and compressor filters. This is the opposite of
		///  renderGraph(). Soem properties such as FrameRate
		///  can only be set when the device output pins are not
		///  connected. 
		/// </summary>
		protected void derenderGraph()
		{
			// Stop the graph if it is running (ignore errors)
			if ( mediaControl != null )
				mediaControl.Stop();

			// Free the preview window (ignore errors)
			if ( videoWindow != null )
			{
				videoWindow.put_Visible( DsHlp.OAFALSE );
				videoWindow.put_Owner( IntPtr.Zero );
				videoWindow = null;
			}

			// Remove the Resize event handler
			if ( PreviewWindow != null )
				previewWindow.Resize -= new EventHandler( onPreviewWindowResize );

			if ( (int)graphState >= (int)GraphState.Rendered )
			{
				// Update the state
				graphState = GraphState.Created;
				isCaptureRendered = false;
				isPreviewRendered = false;

				// Disconnect all filters downstream of the 
				// video and audio devices. If we have a compressor
				// then disconnect it, but don't remove it
				if ( videoDeviceFilter != null )
					removeDownstream( videoDeviceFilter, (videoCompressor==null) );
				if ( audioDeviceFilter != null )
					removeDownstream( audioDeviceFilter, (audioCompressor==null) );

				// These filters should have been removed by the
				// calls above. (Is there anyway to check?)
				muxFilter = null;
				fileWriterFilter = null;
			}
		}

		/// <summary>
		///  Removes all filters downstream from a filter from the graph.
		///  This is called only by derenderGraph() to remove everything
		///  from the graph except the devices and compressors. The parameter
		///  "removeFirstFilter" is used to keep a compressor (that should
		///  be immediately downstream of the device) if one is begin used.
		/// </summary>
		protected void removeDownstream( IBaseFilter filter, bool removeFirstFilter )
		{
			// Get a pin enumerator off the filter
			IEnumPins pinEnum;
			int hr = filter.EnumPins( out pinEnum );
			pinEnum.Reset();
			if( (hr == 0) && (pinEnum != null) )
			{
				// Loop through each pin
				IPin[] pins = new IPin[1];
				int f;
				do
				{
					// Get the next pin
					hr = pinEnum.Next( 1, pins, out f );
					if( (hr == 0) && (pins[0] != null) )
					{
						// Get the pin it is connected to
						IPin pinTo = null;
						pins[0].ConnectedTo( out pinTo );
						if ( pinTo != null )
						{
							// Is this an input pin?
							PinInfo info = new PinInfo();
							hr = pinTo.QueryPinInfo( out info );
							if( (hr == 0) && (info.dir == (PinDirection.Input)) )
							{
								// Recurse down this branch
								removeDownstream( info.filter, true );

								// Disconnect 
								graphBuilder.Disconnect( pinTo );
								graphBuilder.Disconnect( pins[0] );

								// Remove this filter
								// but don't remove the video or audio compressors
								if ( ( info.filter != videoCompressorFilter ) &&
									 ( info.filter != audioCompressorFilter ) )
									graphBuilder.RemoveFilter( info.filter );
							}
							Marshal.ReleaseComObject( info.filter );
							Marshal.ReleaseComObject( pinTo );
						}
						Marshal.ReleaseComObject( pins[0] );
					}
				}
				while( hr == 0 );

				Marshal.ReleaseComObject( pinEnum ); pinEnum = null;
			}
		}

		/// <summary>
		///  Completely tear down a filter graph and 
		///  release all associated resources.
		/// </summary>
		protected void destroyGraph()
		{
			// Derender the graph (This will stop the graph
			// and release preview window. It also destroys
			// half of the graph which is unnecessary but
			// harmless here.) (ignore errors)
			try{ derenderGraph(); } catch {}

			// Update the state after derender because it
			// depends on correct status. But we also want to
			// update the state as early as possible in case
			// of error.
			graphState = GraphState.Null;
			isCaptureRendered = false;
			isPreviewRendered = false;

			// Remove graph from the ROT
			if ( rotCookie != 0 )
			{
				DsROT.RemoveGraphFromRot( ref rotCookie );
				rotCookie = 0;
			}

			// Remove filters from the graph
			// This should be unnecessary but the Nvidia WDM
			// video driver cannot be used by this application 
			// again unless we remove it. Ideally, we should
			// simply enumerate all the filters in the graph
			// and remove them. (ignore errors)
			if ( muxFilter != null )
				graphBuilder.RemoveFilter( muxFilter );
			if ( videoCompressorFilter != null )
				graphBuilder.RemoveFilter( videoCompressorFilter  );
			if ( audioCompressorFilter != null )
				graphBuilder.RemoveFilter( audioCompressorFilter  );
			if ( videoDeviceFilter != null )
				graphBuilder.RemoveFilter( videoDeviceFilter );
			if ( audioDeviceFilter != null )
				graphBuilder.RemoveFilter( audioDeviceFilter );

			// Clean up properties
			if ( videoSources != null )
				videoSources.Dispose(); videoSources = null;
			if ( audioSources != null )
				audioSources.Dispose(); audioSources = null;
			if ( propertyPages != null )
				propertyPages.Dispose(); propertyPages = null;
			if ( tuner != null )
				tuner.Dispose(); tuner = null;

			// Cleanup
			if ( graphBuilder != null )
				Marshal.ReleaseComObject( graphBuilder );  graphBuilder = null;
			if ( captureGraphBuilder != null )
				Marshal.ReleaseComObject( captureGraphBuilder ); captureGraphBuilder = null;
			if ( muxFilter != null )
				Marshal.ReleaseComObject( muxFilter ); muxFilter = null;
			if ( fileWriterFilter != null )
				Marshal.ReleaseComObject( fileWriterFilter ); fileWriterFilter = null;
			if ( videoDeviceFilter != null )
				Marshal.ReleaseComObject( videoDeviceFilter ); videoDeviceFilter = null;
			if ( audioDeviceFilter != null )
				Marshal.ReleaseComObject( audioDeviceFilter ); audioDeviceFilter = null;
			if ( videoCompressorFilter != null )
				Marshal.ReleaseComObject( videoCompressorFilter ); videoCompressorFilter = null;
			if ( audioCompressorFilter != null )
				Marshal.ReleaseComObject( audioCompressorFilter ); audioCompressorFilter = null;

			// These are copies of graphBuilder
			mediaControl = null;
			videoWindow = null;

			// For unmanaged objects we haven't released explicitly
			GC.Collect();
		}

		/// <summary> Resize the preview when the PreviewWindow is resized </summary>
		protected void onPreviewWindowResize(object sender, EventArgs e)
		{
			if ( videoWindow != null )
			{
				// Position video window in client rect of owner window
				Rectangle rc = previewWindow.ClientRectangle;
				videoWindow.SetWindowPosition( 0, 0, rc.Right, rc.Bottom );
			}
		}
	
		/// <summary> 
		///  Get a valid temporary filename (with path). We aren't using 
		///  Path.GetTempFileName() because it creates a 0-byte file 
		/// </summary>
		protected string getTempFilename()
		{
			string s;
			try
			{
				int count = 0;
				int i;
				Random r = new Random();
				string tempPath = Path.GetTempPath();
				do
				{
					i = r.Next();
					s = Path.Combine( tempPath, i.ToString("X") + ".avi" );
					count++;
					if ( count > 100 ) throw new InvalidOperationException( "Unable to find temporary file." );
				} while ( File.Exists( s ) );
			}
			catch { s = "c:\temp.avi"; }
			return( s );
		}

		/// <summary>
		///  Retrieves the value of one member of the IAMStreamConfig format block.
		///  Helper function for several properties that expose
		///  video/audio settings from IAMStreamConfig.GetFormat().
		///  IAMStreamConfig.GetFormat() returns a AMMediaType struct.
		///  AMMediaType.formatPtr points to a format block structure.
		///  This format block structure may be one of several 
		///  types, the type being determined by AMMediaType.formatType.
		/// </summary>
		protected object getStreamConfigSetting( IAMStreamConfig streamConfig, string fieldName)
		{
			if ( streamConfig == null )
				throw new NotSupportedException();
			assertStopped();
			derenderGraph();

			object returnValue = null;
			IntPtr pmt = IntPtr.Zero;
			AMMediaType mediaType = new AMMediaType();

			try 
			{
				// Get the current format info
				int hr = streamConfig.GetFormat( out pmt );
				if ( hr != 0 )
					Marshal.ThrowExceptionForHR( hr );
				Marshal.PtrToStructure( pmt, mediaType );

				// The formatPtr member points to different structures
				// dependingon the formatType
				object formatStruct;
				if ( mediaType.formatType == FormatType.WaveEx )
					formatStruct = new WaveFormatEx();
				else if ( mediaType.formatType == FormatType.VideoInfo )
					formatStruct = new VideoInfoHeader();
				else if ( mediaType.formatType == FormatType.VideoInfo2 )
					formatStruct = new VideoInfoHeader2();
				else
					throw new NotSupportedException( "This device does not support a recognized format block." );

				// Retrieve the nested structure
				Marshal.PtrToStructure( mediaType.formatPtr, formatStruct );

				// Find the required field
				Type structType = formatStruct.GetType();
				FieldInfo fieldInfo = structType.GetField( fieldName );
				if ( fieldInfo == null )
					throw new NotSupportedException( "Unable to find the member '" + fieldName + "' in the format block." );

				// Extract the field's current value
				returnValue = fieldInfo.GetValue( formatStruct ); 
						
			}
			finally
			{
				DsUtils.FreeAMMediaType( mediaType );
				Marshal.FreeCoTaskMem( pmt );
			}
			renderGraph();
			startPreviewIfNeeded();

			return( returnValue );
		}

		/// <summary>
		///  Set the value of one member of the IAMStreamConfig format block.
		///  Helper function for several properties that expose
		///  video/audio settings from IAMStreamConfig.GetFormat().
		///  IAMStreamConfig.GetFormat() returns a AMMediaType struct.
		///  AMMediaType.formatPtr points to a format block structure.
		///  This format block structure may be one of several 
		///  types, the type being determined by AMMediaType.formatType.
		/// </summary>
		protected object setStreamConfigSetting( IAMStreamConfig streamConfig, string fieldName, object newValue)
		{
			if ( streamConfig == null )
				throw new NotSupportedException();
			assertStopped();
			derenderGraph();

			object returnValue = null;
			IntPtr pmt = IntPtr.Zero;
			AMMediaType mediaType = new AMMediaType();

			try 
			{
				// Get the current format info
				int hr = streamConfig.GetFormat( out pmt );
				if ( hr != 0 )
					Marshal.ThrowExceptionForHR( hr );
				Marshal.PtrToStructure( pmt, mediaType );

				// The formatPtr member points to different structures
				// dependingon the formatType
				object formatStruct;
				if ( mediaType.formatType == FormatType.WaveEx )
					formatStruct = new WaveFormatEx();
				else if ( mediaType.formatType == FormatType.VideoInfo )
					formatStruct = new VideoInfoHeader();
				else if ( mediaType.formatType == FormatType.VideoInfo2 )
					formatStruct = new VideoInfoHeader2();
				else
					throw new NotSupportedException( "This device does not support a recognized format block." );

				// Retrieve the nested structure
				Marshal.PtrToStructure( mediaType.formatPtr, formatStruct );

				// Find the required field
				Type structType = formatStruct.GetType();
				FieldInfo fieldInfo = structType.GetField( fieldName );
				if ( fieldInfo == null )
					throw new NotSupportedException( "Unable to find the member '" + fieldName + "' in the format block." );

				// Update the value of the field
				fieldInfo.SetValue( formatStruct, newValue );

				// PtrToStructure copies the data so we need to copy it back
				Marshal.StructureToPtr( formatStruct, mediaType.formatPtr, false ); 

				// Save the changes
				hr = streamConfig.SetFormat( mediaType );
				if ( hr != 0 )
					Marshal.ThrowExceptionForHR( hr );
			}
			finally
			{
				DsUtils.FreeAMMediaType( mediaType );
				Marshal.FreeCoTaskMem( pmt );
			}
			renderGraph();
			startPreviewIfNeeded();

			return( returnValue );
		}

		/// <summary>
		///  Assert that the class is in a Stopped state.
		/// </summary>
		protected void assertStopped()
		{
			if ( !Stopped )
				throw new InvalidOperationException( "This operation not allowed while Capturing. Please Stop the current capture." ); 
		}

	}
}

