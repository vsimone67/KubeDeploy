using System.Collections.Generic;
using CommandLine;

namespace KubeDeploy
{
    [Verb("build", HelpText = "Build project and deploy to Docker hub")]
    public class BuildOptions : IBaseOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }
    }
}