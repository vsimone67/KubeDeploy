using System.Collections.Generic;
using CommandLine;

namespace KubeDeploy
{
    [Verb("full", HelpText = "Build and deploy to cluster")]
    public class FullBuildOptions : IBaseOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }

    }
}