using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clouding
{
    public struct PatchItemInfo
    {
        public string patchFile { get; set; }
        public string fileName { get; set; }
        public string relativePath { get; set; }
    }
    public struct PatchItem
    {
        public string version { get; set; }
        public PatchItemInfo info { get; set; }
    }
    public struct UpdatePacks
    {
        public string version { get; set; }
        public string fileName { get; set; }
        public string relativePath { get; set; }
    };
    public class PackInfoFile
    {
        public string LatestVersion { get; set; }
        public List<PatchItem> FixPacks { get; set; }
        public UpdatePacks UpdatePacks { get; set; }
        public string LatestIsoUrl { get; set; }
    }
}
