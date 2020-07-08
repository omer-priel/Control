using System;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Both
{
    public class Client
    {
        private int port, maxByte;
        private TcpClient clientSocket;
        private NetworkStream serverStream;
        private Thread ChatThread;
        private bool connect = false;
        private bool isConnect
        {
            get
            {
                return connect;
            }
        }

        public Client(int Port, int MaxByte)
        {
            port = Port;
            maxByte = MaxByte;
        }

        public void Connect(string IP)
        {
            if (connect)
                return;
            clientSocket = new TcpClient();
            serverStream = default(NetworkStream);
            clientSocket.Connect(IP, port);
            serverStream = clientSocket.GetStream();

            byte[] outStream = Encoding.ASCII.GetBytes("$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            connect = true;
            ChatThread = new Thread(TheChat);
            ChatThread.Start();
        }
        public void Disconnect()
        {
            if (!connect)
                return;
            ChatThread.Abort();
            clientSocket.Close();
            serverStream.Close();
            connect = false;
        }

        public EventServer Read;
        public void Write(byte[] array)
        {
            byte[] outStream = array;
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void TheChat()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                byte[] inStream = new byte[maxByte];
                serverStream.Read(inStream, 0, maxByte);
                if (Read != null)
                    Read(this,inStream);
            }
        }
    }
}