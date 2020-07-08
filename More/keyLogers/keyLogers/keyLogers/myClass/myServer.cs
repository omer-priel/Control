using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace myClass
{
    class myServer
    {
        public static TcpListener serverSocket;
        public static TcpClient clientSocket;
        public static int port = 8888, maxByte = 10000000;
        public static bool exit = false;
        private static Hashtable clientsList = new Hashtable();
        public static void start()
        {
            serverSocket = new TcpListener(port);
            clientSocket = default(TcpClient);
            int counter = 0;
            list<handleClinet> client = new list<handleClinet>(new handleClinet());
            serverSocket.Start();
            counter = 0;
            while (!exit)
            {
                try
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    if (!exit)
                    {
                        byte[] bytesFrom = new byte[maxByte];
                        string dataFromClient = null;
                        NetworkStream networkStream = clientSocket.GetStream();
                        networkStream.Read(bytesFrom, 0, maxByte);
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                        clientsList.Add(dataFromClient, clientSocket);
                        broadcast(dataFromClient, "conect");
                        client.add(new handleClinet());
                        client.getValue(client.Length() - 1).startClient(clientSocket, dataFromClient, clientsList);
                    }
                }
                catch
                {
                    keyLogers.Program.write("ero conect");
                }
            }
            int count = client.Length();
            for (int i = 0; i < count; i++)
            {
                if (client.getValue(i).ctThread != null)
                    client.getValue(i).ctThread.Abort();
            }
            clientSocket.Close();
            serverSocket.Stop();
        }
        public static mesg get = null;
        public static mesgByte getByte = null;
        public static void broadcast(string uName, string msg, byte[] msgByte = null)
        {
            if (get != null)
                get(uName, msg);
            if (getByte != null && msgByte != null)
                getByte(uName, msgByte);
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;
                string end = "";
                if (msgByte != null)
                    end = Encoding.ASCII.GetString(msgByte);
                broadcastBytes = Encoding.ASCII.GetBytes("[" + uName + "] " + msg + "$" + end + "^");
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }
    }
    delegate void mesg(string name, string text);
    delegate void mesgByte(string name, byte[] text);
    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;
        public Thread ctThread;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, 10024);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    if (dataFromClient == "exit")
                    {
                        myServer.broadcast(clNo, "remove", new byte[0]);
                        clientsList.Remove(clNo);
                        return;
                    }
                    Byte[] g = new Byte[dataFromClient.Length];
                    Array.Copy(bytesFrom, g, dataFromClient.Length);
                    myServer.broadcast(clNo, dataFromClient, g);
                    rCount = Convert.ToString(requestCount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}