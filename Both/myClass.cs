using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Both
{
    namespace MyClass
    {
        static public class Obj<o>
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
        static public class Folders
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
                    urls = Obj<string>.add(urls, i);
                    foreach (DirectoryInfo dir in dirs)
                    {
                        string[] i2 = new string[1];
                        i2[0] = dir.FullName;
                        urls = Obj<string>.add(urls, i2);
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
                    string[] ret = Folders.ReadFolder(url);

                    for (int i = 0; i < ret.Length; i++)
                    {
                        if (ret[i] != type)
                        {
                            ret = Obj<string>.sub(ret, i);
                            i--;
                        }
                    }
                    return ret;
                }
            }

            public static string[] ReadFolder(string url)
            {
                ReadAllFolder r = new ReadAllFolder();
                string[] u = { "" };
                return Obj<string>.sub(r.ReadFolder(url, u), 0);
            }
            public static void CopyFolder(string folferA, string folderB)
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
                    CopyFolder(dir.FullName, folderB + @"\" + folder.Name);
                }
            }
            public static void DeletFolder(string folder)
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
                        DeletFolder(dir.FullName);
                    }
                }
                Folder.Delete();
            }
        }
    }
}
