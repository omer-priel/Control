using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace myClass
{
    static class obj<o>
    {
        public static o[] add(o[] a, o[] b)
        {
            o[] sum = new o[a.Length + b.Length];
            if (a.Length == 0)
                sum = b;
            else
            {
                for (int i = 0; i < sum.Length; i++)
                {
                    if (i < a.Length)
                    {
                        sum[i] = a[i];
                    }
                    else
                    {
                        sum[i] = b[i - a.Length];
                    }
                }
            }
            return sum;
        }
        public static o[] add(o[] a, o b)
        {
            o[] sum = new o[a.Length + 1];
            if (a.Length == 0)
                sum[0] = b;
            else
            {
                for (int i = 0; i < a.Length; i++)
                    sum[i] = a[i];
                sum[sum.Length - 1] = b;
            }
            return sum;
        }
        public static o[] sub(o[] str, int index)
        {
            o[] ret = new o[str.Length - 1];
            for (int i = 0; i < ret.Length; i++)
            {
                if (i < index)
                {
                    ret[i] = str[i];
                }
                else
                {
                    ret[i] = str[i + 1];
                }
            }
            return ret;
        }
    }
    static class sql
    {
        public static string Alar(string tabel, string w = "", string type = "*")
        {
            string ret = "";
            ret += "SELECT " + type + " FROM [" + tabel + "] ";
            if (w != "")
                ret += "WHERE (" + w + ")";
            return ret;
        }
        public static string UpDet(string tabel, string[] inTo, string[] OutTo)
        {
            string ret = "";
            if (inTo.Length == OutTo.Length)
            {
                ret += "INSERT INTO [" + tabel + "] (" + inTo[0] + " ";
                for (int i = 1; i < inTo.Length; i++)
                {
                    ret += "," + inTo[i] + " ";
                }
                ret += ") VALUES (N'" + OutTo[0] + "' ";
                for (int i = 1; i < OutTo.Length; i++)
                {
                    ret += ",N'" + OutTo[i] + "' ";
                }
                ret += ")";
            }
            else
            {
                ret = null;
            }
            return ret;
        }
        public static string InUpDet(string tabel, string w, string[] inTo, string[] OutTo)
        {
            string ret = "";
            if (inTo.Length == OutTo.Length)
            {
                ret += "UPDATE [" + tabel + "] SET " + inTo[0] + "  = N'" + OutTo[0] + "' ";
                for (int i = 1; i < inTo.Length; i++)
                {
                    ret += "," + inTo[i] + "  = N'" + OutTo[i] + "' ";
                }
                if (w != "")
                    ret += " WHERE (" + w + ")";
            }
            else
            {
                ret = null;
            }
            return ret;
        }
        public static string Del(string tabel, string inTo, string OutTo)
        {
            string ret = "";
            ret += "DELETE FROM [" + tabel + "] WHERE (" + inTo + " = N'" + OutTo + "')";
            return ret;
        }

    }
    static class files
    {
        private class ReadAllFolder
        {
            public string[] ReadFolder(string url, string[] urls)
            {
                DirectoryInfo folder = new DirectoryInfo(url);
                FileInfo[] files = folder.GetFiles();
                DirectoryInfo[] dirs = folder.GetDirectories();

                string[] i = new string[files.Length];
                int cont = 0;
                foreach (FileInfo file in files)
                {
                    i[cont] = file.FullName;
                    cont++;
                }
                urls = myClass.obj<string>.add(urls, i);
                foreach (DirectoryInfo dir in dirs)
                {
                    string[] i2 = new string[1];
                    i2[0] = dir.FullName;
                    urls = myClass.obj<string>.add(urls, i2);
                    urls = ReadFolder(dir.FullName, urls);
                }
                return urls;
            }

            public static int ReadFolderLength(string url)
            {
                int sum = 0;
                DirectoryInfo folder = new DirectoryInfo(url);
                sum += folder.GetFiles().Length;
                DirectoryInfo[] dirs = folder.GetDirectories();
                sum += dirs.Length;

                foreach (DirectoryInfo dir in dirs)
                {
                    sum += ReadFolderLength(dir.FullName);
                }
                return sum;
            }
            public static int ReedFolderLength(string url, bool folder)
            {
                int sum = 0;
                DirectoryInfo Folder = new DirectoryInfo(url);
                sum += Folder.GetFiles().Length;
                DirectoryInfo[] dirs = Folder.GetDirectories();
                if (folder)
                    sum += dirs.Length;
                foreach (DirectoryInfo dir in dirs)
                {
                    sum += ReedFolderLength(dir.FullName, folder);
                }
                return sum;
            }
            public static string[] readFolder(string url, string type)
            {
                string[] ret = myClass.files.readFolder(url);

                for (int i = 0; i < ret.Length; i++)
                {
                    if (ret[i] != type)
                    {
                        ret = myClass.obj<string>.sub(ret, i);
                        i--;
                    }
                }
                return ret;
            }
        }

        public static string[] readFolder(string url)
        {
            ReadAllFolder r = new ReadAllFolder();
            string[] u = { "" };
            return obj<string>.sub(r.ReadFolder(url, u), 0);
        }
        public static void copyFolder(string folferA, string folderB)
        {
            DirectoryInfo folder = new DirectoryInfo(folferA);
            FileInfo[] files = folder.GetFiles();
            DirectoryInfo[] dirs = folder.GetDirectories();

            Directory.CreateDirectory(folderB + @"\" + folder.Name);
            foreach (FileInfo file in files)
            {
                File.Copy(file.FullName, folderB + @"\" + folder.Name + @"\" + file.Name);
            }
            foreach (DirectoryInfo dir in dirs)
            {
                copyFolder(dir.FullName, folderB + @"\" + folder.Name);
            }
        }
        public static void deletFolder(string folder)
        {
            DirectoryInfo Folder = new DirectoryInfo(folder);
            FileInfo[] files = Folder.GetFiles();
            DirectoryInfo[] dirs = Folder.GetDirectories();
            foreach (FileInfo file in files)
            {
                File.Delete(file.FullName);
            }
            foreach (DirectoryInfo dir in dirs)
            {
                if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
                {
                    dir.Delete();
                }
                else
                {
                    deletFolder(dir.FullName);
                }
            }
            Folder.Delete();
        }
    }
    static class Number
    {
        public static bool ExistsNum(string num)
        {
            if (num[0] == '-')
            {
                string str = num;
                num = "";
                for (int i = 1; i < str.Length; i++)
                {
                    num += str[i];
                }
            }
            foreach (char tv in num)
            {
                if (tv > '9' || '0' > tv)
                    return false;
            }
            return true;
        }  
        public static bool ExistsNumFloat(string num)
        {
            if (num[0] == '-')
            {
                string str = num;
                num = "";
                for (int i = 1; i < str.Length; i++)
                {
                    num += str[i];
                }
            }
            bool chPoint = false;
            foreach (char tv in num)
            {
                if (tv == '.')
                {
                    if (!chPoint)
                        chPoint = true;
                    else
                        return false;
                }
                else
                {
                    if (tv > '9' || '0' > tv)
                        return false;
                }
            }
            return true;
        }
        public const string NULL = "NULL";
        public static string math(string f)
        {
            return myMath.math(f);
        }

        private static class myMath
        {
            private static string[] arryeEro
            {
                get
                {
                    string[] str = { "NULL" };
                    return str;
                }
            }
            public static string math(string f)
            {
                if (f.Length == 0)
                {
                    return NULL;
                }
                string[] tv = createStringTv(f);
                if (!myClass.Number.ExistsNumFloat(tv[0]) || tv == arryeEro)
                    return NULL;
                for (int i = 0; i < tv.Length; i++)
                {
                    string str = tv[i];
                    if (str[0] == '(' && str[str.Length - 1] == ')')
                    {
                        str = "";
                        for (int i2 = 0; i2 < (tv[i]).Length - 2; i2++)
                        {
                            str += (tv[i])[i2 + 1];
                        }
                        tv[i] = myMath.math(str);
                        if (tv[i] == NULL)
                            return NULL;
                    }
                }
                float ret = float.Parse(tv[0]);
                for (int i = 0; i < tv.Length - 1; i += 2)
                {
                    string tot = a(ret + "", tv[i + 1], tv[i + 2]);
                    if (tot == NULL)
                        return NULL;
                    ret = float.Parse(tot);
                }
                return ret + "";
            }
            private static string[] createStringTv(string f)
            {
                if (f.Length == 0)
                    return arryeEro;
                if (f[0] == ' ')
                    return arryeEro;
                string[] tv;
                int cont = 1;
                int s = 0;
                for (int i = 0; i < f.Length; i++)
                {
                    switch (f[i])
                    {
                        case ' ':
                            {
                                if (s == 0)
                                    cont++;
                            }
                            break;
                        case '(':
                            {
                                s++;
                            }
                            break;
                        case ')':
                            {
                                s--;
                            }
                            break;
                    }
                    if (s < 0)
                        return arryeEro;
                }
                if (s != 0)
                    return arryeEro;
                tv = new string[cont];
                cont = 0;
                for (int i = 0; i < f.Length; i++)
                {
                    switch (f[i])
                    {
                        case ' ':
                            {
                                if (s == 0)
                                {
                                    cont++;
                                    tv[cont] = "";
                                }
                            }
                            break;
                        case '(':
                            {
                                s++;
                            }
                            break;
                        case ')':
                            {
                                s--;
                            }
                            break;
                    }
                    if (f[i] != ' ' || s != 0)
                        tv[cont] += f[i];
                }
                return tv;
            }
            private static string a(string var1, string oper, string var2)
            {
                if (myClass.Number.ExistsNumFloat(var1) && myClass.Number.ExistsNumFloat(var2))
                {
                    float number1 = float.Parse(var1), number2 = float.Parse(var2);
                    switch (oper)
                    {
                        case "+":
                            {
                                return (number1 + number2) + "";
                            }
                            break;
                        case "-":
                            {
                                return (number1 - number2) + "";
                            }
                            break;
                        case "*":
                            {
                                return (number1 * number2) + "";
                            }
                            break;
                        case "/":
                            {
                                if (number2 == 0)
                                    return NULL;
                                return (number1 / number2) + "";
                            }
                            break;
                        case "^":
                            {
                                float a = 0;
                                if (number2 == 0)
                                    return "1";
                                a = number1;
                                int ren = (int)number2;
                                if (ren < 0)
                                    ren *= -1;
                                for (int i = 1; i < ren; i++)
                                {
                                    a *= number1;
                                }
                                if (number2 < 0)
                                    a = 1 / a;
                                return a + "";
                            }
                            break;
                    }
                }
                return NULL;
            }
        }
    }
    static partial class defrent
    {
        public static string[] AllFont
        {
            get
            {
                string[] txt = new string[169];
                txt[0] = "Agency FB";
                txt[1] = "Aharoni";
                txt[2] = "Algerian";
                txt[3] = "Arial";
                txt[4] = "Arial Rounded MT";
                txt[5] = "Arial Unicode MS";
                txt[6] = "Baskerville Old Face";
                txt[7] = "Bauhaus 93";
                txt[8] = "Bell MT";
                txt[9] = "Berlin Sans FB";
                txt[10] = "Bernard MT";
                txt[11] = "Blackadder ITC";
                txt[12] = "Bodoni MT";
                txt[13] = "Bodoni MT Poster";
                txt[14] = "Yu Gothic";
                txt[15] = "Bookman Old Style";
                txt[16] = "Bookshelf Symbol 7";
                txt[17] = "Bradley Hand ITC";
                txt[18] = "Britannic";
                txt[19] = "Broadway";
                txt[20] = "Brush Script MT";
                txt[21] = "Buxton Sketch";
                txt[22] = "Calibri";
                txt[23] = "Californian FB";
                txt[24] = "Calisto MT";
                txt[25] = "Cambria";
                txt[26] = "Cambria Math";
                txt[27] = "Candara";
                txt[28] = "Castellar";
                txt[29] = "Centaur";
                txt[30] = "Century";
                txt[31] = "Century Gothic";
                txt[32] = "Century Schoolbook";
                txt[33] = "Chiller";
                txt[34] = "Colonna MT";
                txt[35] = "Comic Sans MS";
                txt[36] = "Consolas";
                txt[37] = "Constantia";
                txt[38] = "Cooper";
                txt[39] = "Copperplate Gothic";
                txt[40] = "Corbel";
                txt[41] = "Courier New";
                txt[42] = "Curlz MT";
                txt[43] = "David";
                txt[44] = "DengXian";
                txt[45] = "Edwardian Script ITC";
                txt[46] = "Elephant";
                txt[47] = "Engravers MT";
                txt[48] = "Eras ITC";
                txt[49] = "Felix Titling";
                txt[50] = "Footlight MT";
                txt[51] = "Forte";
                txt[52] = "Franklin Gothic";
                txt[53] = "Franklin Gothic Book";
                txt[54] = "FrankRuehl";
                txt[55] = "Freestyle Script";
                txt[56] = "French Script MT";
                txt[57] = "Gabriola";
                txt[58] = "Garamond";
                txt[59] = "Georgia";
                txt[60] = "Gigi";
                txt[61] = "Gill Sans";
                txt[62] = "Gill Sans MT";
                txt[63] = "Gisha";
                txt[64] = "Gloucester MT";
                txt[65] = "Goudy Old Style";
                txt[66] = "Goudy Stout";
                txt[67] = "Guttman Aharoni";
                txt[68] = "Guttman Drogolin";
                txt[69] = "Guttman Frank";
                txt[70] = "Guttman Frnew";
                txt[71] = "Guttman Haim";
                txt[72] = "Guttman Hatzvi";
                txt[73] = "Guttman Kav";
                txt[74] = "Guttman Logo1";
                txt[75] = "Guttman Mantova";
                txt[76] = "Guttman Mantova-Decor";
                txt[77] = "Guttman Miryam";
                txt[78] = "Guttman Myamfix";
                txt[79] = "Guttman Rashi";
                txt[80] = "Guttman Stam";
                txt[81] = "Guttman Stam1";
                txt[82] = "Guttman Vilna";
                txt[83] = "Guttman Yad";
                txt[84] = "Guttman Yad-Brush";
                txt[85] = "Guttman-Aharoni";
                txt[86] = "Guttman-Aram";
                txt[87] = "Guttman-CourMir";
                txt[88] = "Haettenschweiler";
                txt[89] = "Harlow Solid";
                txt[90] = "Harrington";
                txt[91] = "High Tower Text";
                txt[92] = "Impact";
                txt[93] = "Imprint MT Shadow";
                txt[94] = "Informal Roman";
                txt[95] = "Jokerman";
                txt[96] = "Juice ITC";
                txt[97] = "Kristen ITC";
                txt[98] = "Kunstler Script";
                txt[99] = "Latin";
                txt[100] = "Levenim MT";
                txt[101] = "Lucida Bright";
                txt[102] = "Lucida Calligraphy";
                txt[103] = "Lucida Console";
                txt[104] = "Lucida Fax";
                txt[105] = "Lucida Handwriting";
                txt[106] = "Lucida Sans";
                txt[107] = "Lucida Sans Typewriter";
                txt[108] = "Lucida Sans Unicode";
                txt[109] = "Magneto";
                txt[110] = "Maiandra GD";
                txt[111] = "Matura MT Script Capitals";
                txt[112] = "Microsoft MHei";
                txt[113] = "Microsoft NeoGothic";
                txt[114] = "Microsoft Sans Serif";
                txt[115] = "Miriam";
                txt[116] = "Miriam Fixed";
                txt[117] = "Mistral";
                txt[118] = "Modern No. 20";
                txt[119] = "Monotype Corsiva";
                txt[120] = "Monotype Hadassah";
                txt[121] = "MS Outlook";
                txt[122] = "MS Reference Sans Serif";
                txt[123] = "MS Reference Specialty";
                txt[124] = "MT Extra";
                txt[125] = "Narkisim";
                txt[126] = "Niagara Engraved";
                txt[127] = "Niagara Solid";
                txt[128] = "OCR A";
                txt[129] = "Old English Text MT";
                txt[130] = "Onyx";
                txt[131] = "Palace Script MT";
                txt[132] = "Palatino Linotype";
                txt[133] = "Papyrus";
                txt[134] = "Parchment";
                txt[135] = "Perpetua";
                txt[136] = "Perpetua Titling MT";
                txt[137] = "Playbill";
                txt[138] = "Poor Richard";
                txt[139] = "Pristina";
                txt[140] = "Rage";
                txt[141] = "Ravie";
                txt[142] = "Rockwell";
                txt[143] = "Rod";
                txt[144] = "Script MT";
                txt[145] = "Segoe Marker";
                txt[146] = "Segoe Print";
                txt[147] = "Segoe Script";
                txt[148] = "Segoe UI";
                txt[149] = "Segoe UI Symbol";
                txt[150] = "Segoe WP";
                txt[151] = "Showcard Gothic";
                txt[152] = "SketchFlow Print";
                txt[153] = "Snap ITC";
                txt[154] = "Stencil";
                txt[155] = "Symbol";
                txt[156] = "Tahoma";
                txt[157] = "Tempus Sans ITC";
                txt[158] = "Times New Roman";
                txt[159] = "Trebuchet MS";
                txt[160] = "Tw Cen MT";
                txt[161] = "Verdana";
                txt[162] = "Viner Hand ITC";
                txt[163] = "Vivaldi";
                txt[164] = "Vladimir Script";
                txt[165] = "Webdings";
                txt[166] = "Wingdings";
                txt[167] = "Wingdings 2";
                txt[168] = "Wingdings 3";
                return txt;
            }
        }
        public static void sendMail(string user, string password, string to, string subject, string body, bool html = false)
        {
            MailMessage mailMessage = new MailMessage(user, to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = html;

            SmtpClient smtpClient = new SmtpClient();
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = user.Split('@')[0],
                Password = password
            };
            //smtpClient.UseDefaultCredentials = false;
            smtpClient.Send(mailMessage);
        }
        public static string nameComputer()
        {
            return Dns.GetHostName();
        }
        public static string ipComputer()
        {
            IPHostEntry hostname = Dns.GetHostByName(nameComputer());
            IPAddress[] ip = hostname.AddressList;
            return ip[0].ToString();
        }
        public static string ipComputerOther(string name)
        {
            IPHostEntry hostname = Dns.GetHostByName(name);
            IPAddress[] ip = hostname.AddressList;
            return ip[0].ToString();
        }
    }
    
    class list<obj>
    {
        public obj value;
        public list<obj> next;

        public list(obj value, list<obj> next = null)
        {
            this.value = value;
            this.next = next;
        }
        public list(obj[] values)
        {
            next = null;
            value = values[0];
            for (int i = 1; i < values.Length; i++)
                add(values[i]);
        }

        //data
        public int Length()
        {
            if (next == null)
                return 1;
            return next.Length() + 1;
        }
        public int indexOf(obj value)
        {
            int ret = indexOf_lode(value);
            return (ret < Length()) ? ret : -1;
        }
        private int indexOf_lode(obj value)
        {
            if (value.Equals(value))
                return 0;
            if (next == null)
                return 1;
            return next.indexOf_lode(value) + 1;
        }
        public bool hesNext()
        {
            return (next != null);
        }

        //options
        public void add(obj value, list<obj> next = null)
        {
            if (this.next == null)
                this.next = new list<obj>(value, next);
            else
                this.next.add(value, next);
        }
        public void removeEnd(int index = 1, bool untilEnd = false)
        {
            remove(Length() - index, untilEnd);
        }
        public void remove(int index, bool untilEnd = false)
        {
            if (index == 1)
            {
                if (untilEnd)
                    next = null;
                else
                    next = next.next;
            }
            else
                next.remove(index - 1, untilEnd);
        }

        //vars
        public obj getValue(int index)
        {
            if (index == 0)
                return value;
            return next.getValue(index - 1);
        }
        public void setValue(int index, obj value)
        {
            if (index == 0)
                this.value = value;
            else
                next.setValue(index - 1, value);
        }
        public list<obj> getNext(int index)
        {
            if (index == 0)
                return next;
            return next.getNext(index - 1);
        }
        public void setNext(int index, list<obj> value)
        {
            if (index == 0)
                next = value;
            else
                next.setNext(index - 1, value);
        }

        public virtual string ToString(string tab = ">")
        {
            if (next == null)
                return value.ToString();
            else
                return value.ToString() + tab + next.ToString(tab);
        }
    }
    class listBack<obj>
    {
        public obj value;
        public  listBack<obj> back, next;

        public  listBack(obj value,  listBack<obj> next = null,  listBack<obj> back = null)
        {
            this.value = value;
            this.next = next;
            this.back = back;
        }
        public  listBack(obj[] values)
        {
            next = null;
            value = values[0];
            for (int i = 1; i < values.Length; i++)
                addNext(values[i]);
        }

        //data
        public int LengthNext()
        {
            if (next == null)
                return 1;
            return next.LengthNext() + 1;
        }
        public int LengthBack()
        {
            if (back == null)
                return 1;
            return next.LengthBack() + 1;
        }
        public int Length()
        {
            return next.LengthNext() + LengthBack() - 1;
        }
        public bool hesNext()
        {
            return (next != null);
        }
        public bool hesBack()
        {
            return (back != null);
        }

        //options
        public void addNext(obj value,  listBack<obj> next = null)
        {
            if (this.next == null)
                this.next = new  listBack<obj>(value, next, this);
            else
                this.next.addNext(value, next);
        }
        public void addBack(obj value,  listBack<obj> back = null)
        {
            if (this.back == null)
                this.back = new  listBack<obj>(value, this, back);
            else
                this.back.addNext(value, back);
        }
        public void removeEnd(int index = 1, bool untilEnd = false)
        {
            remove(Length() - index, untilEnd);
        }
        public void remove(int index, bool untilEnd = false)
        {
            if (index > 0)
            {
                if (index == 1)
                {
                    if (untilEnd)
                        next = null;
                    else
                        next = next.next;
                }
                else
                    next.remove(index - 1, untilEnd);
            }
            else
            {
                if (index == -1)
                {
                    if (untilEnd)
                        back = null;
                    else
                        back = back.back;
                }
                else
                    back.remove(index + 1, untilEnd);
            }
        }

        //vars
        public obj getValue(int index)
        {
            if (index == 0)
                return value;
            if (index > 0)
                return next.getValue(index - 1);
            else
                return back.getValue(index + 1);
        }
        public void setValue(int index, obj value)
        {
            if (index == 0)
                this.value = value;
            else
            {
                if (index > 0)
                    next.setValue(index - 1, value);
                else
                    back.setValue(index + 1, value);
            }
        }
        public  listBack<obj> getSell(int index)
        {
            if (index == 0)
                return next;
            if (index > 0)
                return next.getSell(index - 1);
            else
                return back.getSell(index + 1);
        }
        public void setSell(int index,  listBack<obj> value)
        {
            if (index == 0)
                next = value;
            else
            {
                if (index > 0)
                    next.setSell(index - 1, value);
                else
                    back.setSell(index + 1, value);
            }
        }

        //ToString
        public string ToStringNext(string tab = ">")
        {
            if (next == null)
                return value.ToString();
            else
                return value.ToString() + tab + next.ToStringNext(tab);
        }
        public string ToStringBack(string tab = "<")
        {
            if (back == null)
                return value.ToString();
            else
                return back.ToStringBack(tab) + tab + value.ToString();
        }
        public virtual string ToString(string tabBack = "<", string tabNext = ">")
        {
            string ret = ToStringBack(tabBack), str = ToStringNext(tabNext);
            for (int i = value.ToString().Length - 1; i < str.Length; i++)
            {
                ret += str[i];
            }
            return ret;
        }
    }
    class listTree
    {
        private int value;
        public int Value
        {
            get
            {
                return value;
            }
        }
        public listTree small, big;

        public listTree(int value, listTree small = null, listTree big = null)
        {
            this.value = value;
            this.small = big;
            this.big = big;
        }
        
        public bool hasSmall()
        {
            return (small != null);
        }
        public bool hasBig()
        {
            return (big != null);
        }
        public bool exist(int value)
        {
            if (value == this.value)
                return true;

            if (value > this.value)
            {
                if (hasBig())
                {
                    return big.exist(value);
                }
            }
            else
            {
                if (hasSmall())
                {
                    return small.exist(value);
                }
            }
            return false;
        }
        public int level(int value)
        {
            haveNumber ret = level_load(value);
            if (ret.have)
                return ret.num;
            return -1;
        }

        private haveNumber level_load(int value)
        {
            if (value == this.value)
                return new haveNumber(0);

            if (value > this.value)
            {
                if (hasBig())
                {
                    haveNumber ret = big.level_load(value);
                    ret.num++;
                    return ret;
                }
            }
            else
            {
                if (hasSmall())
                {
                    haveNumber ret = small.level_load(value);
                    ret.num++;
                    return ret;
                }
            }
            return new haveNumber(0,false);
        }
        private struct haveNumber
        {
            public bool have;
            public int num;
            public haveNumber(int num, bool have = true)
            {
                this.have = have;
                this.num = num;
            }
        }
        
        public void add(int value, listTree small = null, listTree big = null)
        {
            if (value > this.value)
            {
                if (hasBig())
                    this.big.add(value, small, big);
                else
                    this.big = new listTree(value, small, big);
            }
            else
            {
                if (hasSmall())
                    this.small.add(value, small, big);
                else
                    this.small = new listTree(value, small, big);
            }
        }
        
        public virtual string ToString()
        {
            string ret = "{" + value;
            if (hasSmall())
                ret += "," + small.ToString();
            if (hasBig())
                ret += "," + big.ToString();
            return ret + "}";
        }
        
        //console
        public void print(int tabTop = 2)
        {
            print(1, 2, Console.CursorTop, tabTop);
        }
        private void print(int loc, int len, int top, int tabTop)
        {
            Console.CursorLeft = Console.WindowWidth / len * loc;
            Console.CursorTop = top * tabTop;
            Console.Write(value);
            len *= 2;
            loc *= 2;
            top++;
            if (hasSmall())
                small.print(loc - 1, len, top, tabTop);
            if (hasBig())
                big.print(loc + 1, len, top, tabTop);
        }
    }
}
