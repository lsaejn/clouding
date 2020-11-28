using System;
using System.IO;

namespace LoveCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream stm = new FileStream("1.txt", FileMode.Append, FileAccess.Write);

            string str = "电你好吗";
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);

            string ret1= System.Text.Encoding.UTF8.GetString(byteArray);

            byte[] byteArray2 = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] byteArray3 = System.Text.Encoding.Unicode.GetBytes(str);

            var name = System.Text.Encoding.Default.EncodingName;

            stm.Write(byteArray);
            str = "bye, bye";
            byteArray = System.Text.Encoding.Default.GetBytes(str);
            stm.Write(byteArray);
            byte[] ba = new byte[1024];
            stm.Write(ba);
            stm.Flush();
            Console.WriteLine("Hello World!");
        }
    }
}
