using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Both
{
    static public class Net
    {
        static public int Port = 8888;
        static public int MaxByte = 1000000;

        //IP
        static public string NameComputer
        {
            get
            {
                return Dns.GetHostName();
            }
        }
        static public string NameToIP(string Name)
        {
            IPHostEntry hostname = Dns.GetHostByName(Name);
            IPAddress[] ip = hostname.AddressList;
            return ip[0].ToString();
        }
    }
}
