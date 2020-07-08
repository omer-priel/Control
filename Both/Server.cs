using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Both
{
    public class Server
    {
        private int port, maxByte;
        public List<Chat> ListChat;
        private TcpListener serverSocket;
        private TcpClient clientSocket;
        public Server(int Port, int MaxByte)
        {
            port = Port;
            maxByte = MaxByte;
        }

        public void Open()
        {
            serverSocket = new TcpListener(port);
            clientSocket = default(TcpClient);
            ListChat = new List<Chat>();

            serverSocket.Start();
            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[byte.MaxValue];

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, byte.MaxValue);
                Chat chat = new Chat();
                chat.Read = Read;
                chat.Start(clientSocket, networkStream, maxByte, this);
                if (LoadChat != null)
                {
                    LoadChat(chat);
                }
                ListChat.Add(chat);
            }
        }
        public void Stop()
        {
            foreach (var item in ListChat)
            {
                item.Stop(true);
            }
            ListChat.Clear();
            clientSocket.Close();
            serverSocket.Stop();
        }

        public EventServer Read = null;
        public EventSender LoadChat = null;
        public void Write(byte[] text)
        {
            foreach (var item in ListChat)
            {
                item.Write(text);
            }
        }
    }
    public class Chat
    {
        private int maxByte;
        private TcpClient clientSocket;
        private Thread thread;
        private NetworkStream serverStream;
        private Server TheServer;
        private bool exit = false;

        public void Start(TcpClient inClientSocket, NetworkStream serverStream, int MaxByte, Server theServer)
        {
            this.clientSocket = inClientSocket;
            this.serverStream = serverStream;
            maxByte = MaxByte;
            TheServer = theServer;
            exit = false;
            thread = new Thread(TheChat);
            thread.Start();
        }
        public void Stop(bool CloseServer = false)
        {
            exit = true;
            thread.Abort();
            thread = null;
            if (!CloseServer)
                TheServer.ListChat.Remove(this);
            serverStream.Dispose();
        }

        public EventServer Read;
        public void Write(byte[] array)
        {
            try
            {
                serverStream.Write(array, 0, array.Length);
                serverStream.Flush();
            }
            catch { }
        }

        private void TheChat()
        {
            byte[] bytesFrom = new byte[maxByte];

            while (!exit)
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, maxByte);
                    if (Read != null)
                        Read(this, bytesFrom);
                }
                catch (Exception ex)
                {
                   
                }
            }
        }
    }
}