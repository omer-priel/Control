using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myClass
{
    class numberBig
    {
        public static string add(string a, string b)
        {
            a = changeSing(a);
            b = changeSing(b);
            int i1 = 0,i2 = 0, plos = 0;
            string ret = "";
            while (i1 < a.Length && i2 < b.Length)
            {
                int sum = (int)a[i1] + (int)b[i2] - (2 * '0');
                sum += plos;
                if (sum > 9)
                {
                    plos = 1;
                    sum -= 10;
                }
                else
                {
                    plos = 0;
                }
                ret += sum;
                i1++;
                i2++;
            }
            while (i1 < a.Length)
            {
                if (plos == 1)
                {
                    ret += ((int)a[i1] - '0' + 1) % 10;
                    if (a[i1] != 9)
                        plos = 0;
                }
                else
                {
                    ret += a[i1];
                }
                i1++;
            }
            while (i2 < b.Length)
            {
                if (plos == 1)
                {
                    ret += ((int)b[i2] - '0' + 1) % 10;
                    if (b[i2] != 9)
                        plos = 0;
                }
                else
                {
                    ret += b[i2];
                }
                i2++;
            }
            if (plos == 1)
                ret += "1";
            return changeSing(ret);
        }
        public static string sub(string a, string b)
        {
            a = changeSing(a);
            b = changeSing(b);
            int i1 = 0,i2 = 0, min = 0;
            string ret = "";
            while (i2 < b.Length && i1 < a.Length)
            {
                int sum = ((int)a[i1] - (int)b[i2]);
                sum -= min;
                if (sum < 0)
                {
                    min = 1;
                    sum += 10;
                    i1++;
                }
                else
                {
                    min = 0;
                }
                ret += sum;
                i1++;
                i2++;
            }
            if (min == 1 && i1 - 1 < a.Length)
            {
                min = 0;
                ret += (char)(a[i1 - 1] - 1);
            }
            while (i1 < a.Length)
            {
                ret += a[i1];
                i1++;
            }
            return changeSing(ret);
        }
        public static string mul(string a, string b)
        {
            a = changeSing(a);
            b = changeSing(b);
            int iArr = 0;
            string ret = "";
            string[] arr = new string[a.Length];
            foreach (char item in a)
            {
                string m = "0";
                for (int i = 0; i < (int)(item - '0'); i++)
                {
                    m = add(m, changeSing(b));
                }
                arr[iArr] = m + ret;
                ret += "0";
                iArr++;
            }
            ret = "0";
            foreach (string item in arr)
            {
                ret = add(ret, item);
            }
            return ret;
        }
        public static string[] div(string a, string b)
        {
            string[] ret = {"", ""};
            string equal = "", tab = "";
            foreach (char item in a)
            {
                equal += item;
                if (big(equal, b, true))
                {
                    int count = 0;
                    do
                    {
                        count++;
                        Console.WriteLine("{0} : {1} [{2}]", equal, b, count);
                        equal = sub(equal, b);
                    }
                    while (big(equal, b));
                    if (big(b, equal))
                        ret[1] += sub(b, sub(b, equal));
                    Console.WriteLine("[{3}]: {0} : {1} [{2}]", equal, b, count, item);
                    ret[0] += count + tab;
                    tab = "";
                }
                else
                    tab += "0";
            }
            if (ret[1] == "")
                ret[1] = "0";
            else
                ret[1] = delZreo(ret[1]);
            ret[0] = delZreo(ret[0]);
            return ret;
        }

        public static string delZreo(string num)
        {
            if (num[0] != '0')
                return num;
            string ret = "";
            int i = 0;
            while (i + 1 < num.Length && num[i] == '0')
            {
                i++;
            }
            while (i < num.Length)
            {
                ret += num[i];
                i++;
            }
            return ret;
        }
        public static bool big(string a, string b, bool e = true)
        {
            a = delZreo(a);
            b = delZreo(b);
            if (a == b && e)
                return true;
            return (a.Length > b.Length || (a.Length == b.Length && a.CompareTo(b) > 0)) ;
        }
        public static string changeSing(string number)
        {
            string ret = "";
            foreach (char item in number)
            {
                ret = item + ret;
            }
            return ret;
        }
    }
}
