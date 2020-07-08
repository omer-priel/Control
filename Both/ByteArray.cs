using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Both
{
    public class ByteArray
    {
        static public int Length = 0;

        public int IndexRead = 0, indexWrite = 0;
        public int IndexWrite
        {
            get
            {
                return indexWrite;
            }
            set
            {
                indexWrite = value;
                writeOnTheMax = !(indexWrite >= Length);
            }
        }
        public byte[] Values;
        public bool writeOnTheMax = false;

        public ByteArray()
        {
            Values = new byte[Length];
        }
        public ByteArray(byte[] Values)
        {
            this.Values = new byte[Values.Length];
            Array.Copy(Values, this.Values, Values.Length);
        }

        public void Write(byte value)
        {
            Values[IndexWrite] = value;
            IndexWrite++;
        }
        public void Write(byte[] array, bool HaveLength)
        {
            if (HaveLength)
            {
                Write(array.Length);
            }
            Array.Copy(array, 0, Values, IndexWrite,array.Length);
            IndexWrite += array.Length;
        }
        public void Write(int value)
        {
            byte[] array = BitConverter.GetBytes(value);
            Write(array, false);
        }
        public void Write(char value)
        {
            byte[] charBytes = Encoding.Unicode.GetBytes(new[] { value }, 0, 1);
            Values[IndexWrite] = charBytes[0];
            IndexWrite++;
            Values[IndexWrite] = charBytes[1];
            IndexWrite++;
        }
        public void Write(string array)
        {
            Write(array.Length);
            foreach (char item in array)
            {
                Write(item);
            }
        }
        public void Write(DateTime value)
        {
            byte[] array = BitConverter.GetBytes(value.Ticks);
            Write(array, false);
        }

        public byte ReadByte()
        {
            byte ret = Values[IndexRead];
            IndexRead++;
            return ret;
        }
        public byte[] ReadByteArray()
        {
            int Length = ReadInt();
            byte[] ret = ReadByteArray(Length);
            return ret;
        }
        public byte[] ReadByteArray(int Length)
        {
            byte[] ret = new byte[Length];
            for (int i = 0; i < Length; i++)
            {
                ret[i] = Values[IndexRead];
                IndexRead++;
            }
            return ret;
        }
        public int ReadInt()
        {
            byte[] array = ReadByteArray(4);
            int ret = BitConverter.ToInt32(array, 0);
            return ret;
        }
        public char ReadChar()
        {
            byte[] arr = { ReadByte(), ReadByte() };
            char ret = Encoding.Unicode.GetChars(arr)[0];
            return ret;
        }
        public string ReadString()
        {
            int Length = ReadInt();
            string ret = "";
            for (int i = 0; i < Length; i++)
            {
                ret += ReadChar();
            }
            return ret;
        }
        public DateTime ReadTime()
        {
            byte[] array = ReadByteArray(8);
            long ticks = BitConverter.ToInt64(array, 0);
            DateTime ret = new DateTime(ticks);
            return ret;
        }
    }
}