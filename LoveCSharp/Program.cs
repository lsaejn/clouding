using System;
using System.IO;

namespace LoveCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream stm = new FileStream("1.txt", FileMode.Append, FileAccess.Write);
            string str = "fuck off, 你好吗?";
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
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
