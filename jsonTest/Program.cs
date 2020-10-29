using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace jsonTest
{
    public struct PatchItemInfo
    {
        public string patchFile { get; set; }
        public string url { get; set; }
    }
    public struct PatchItem
    {
        public string version { get; set; }
        public string patchName { get; set; }
        public string fileName { get; set; }
    }
    public struct FixPacks
    {
        public List<PatchItem> patchs;
    };

    public struct UpdatePacks
    {
        public string version { get; set; }
        public string fileName { get; set; }
    };


    class PackInfoFile
    {
        public string LatestVersion { get; set; }
        public FixPacks fixPacks { get; set; }
        public UpdatePacks updatePacks { get; set; }
        public string LatestIsoUrl { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var sr = new StreamReader("packsInfoWpf.json"))
            {
                // Read the stream as a string, and write the string to the console.
                Console.WriteLine(sr.ReadToEnd());
                var serializer = new JavaScriptSerializer();
                var ret = serializer.Deserialize<PackInfoFile>(packCtn);
                var lv = ret.LatestVersion;
                var iso = ret.LatestIsoUrl;
                var fixs = ret.fixPacks;
                var updatepk = ret.updatePacks;
            }
        }
    }
}
