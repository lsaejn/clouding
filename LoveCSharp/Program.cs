using System;
using System.IO;
using System.Text;

namespace LoveCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                FileStream fs = new FileStream("1.txt", FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);

                Decoder d = Encoding.UTF8.GetDecoder();
                Encoder e = Encoding.UTF8.GetEncoder();
                //System.Text.DecoderNLS
                //d.GetChars()
                // TextReader tr = new TextReader();
            }
            {
                FileStream fs = new FileStream("D:\\wpfAutoUpdate\\LoveCSharp\\Program.cs", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.Unicode, true);
                string str_=sr.ReadToEnd();
            }
            {
                DirectoryInfo info = new DirectoryInfo("D:\\wpfAutoUpdate\\LoveCSharp\\Program.cs");
                var rootInfo=info.Root;
            }
            {
                DateTime t = new DateTime(2022, 2, 3,3,22,43,223);
                string localDateStr=t.ToLongDateString();
                string lts = t.ToLongTimeString();
                string st = t.ToShortDateString();
                string sts=t.ToShortTimeString();
                string ts = t.ToString();
                Console.WriteLine(st);
            }
            {
                DateTime t=new DateTime(2022, 2, 3);
                var ret=DateTime.IsLeapYear(2021);
                ret = DateTime.IsLeapYear(2022);
                ret = DateTime.IsLeapYear(2023);
                ret = DateTime.IsLeapYear(2024);
                ret = DateTime.IsLeapYear(2025);
                //ret = DateTime.IsLeapYear(-1);
            }
            {
                DateTime t = new DateTime(2009, 12, 25);
                t = t.AddDays(1000);
                var y=t.Year;
            }
            {
                DateTime t = DateTime.Now;
                FileInfo f = new FileInfo("2.txt");
                //f.Delete();
                var cT=f.CreationTime;
                var tCount=cT.Ticks;
                var cyT = f.CreationTimeUtc;
            }
            {
                //FileStream stream = File.Create("1.txt");
                //stream.Close();
                var att = File.GetAttributes("2.txt");
                File.SetAttributes("1.txt", FileAttributes.Normal);
                //File.Replace
            }
            { 
                var i = 3;
                FileStream fs = new FileStream(
                String.Format(@"C:\TEMP\TEST{0}.DAT", i),  // name of file
                FileMode.Create,    // create or overwrite existing file
                FileAccess.Write,   // write-only access
                FileShare.None,     // no sharing
                2 << 8,             // block transfer of i=18 -> size = 256 KB
                FileOptions.None);
            }
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
