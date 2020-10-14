using System.Collections.Generic;
using CommandLine;

namespace KubeDeploy
{
    [Verb("boobat", HelpText = "Create new deployment files.")]
    public class BooBatOptions : ICreateOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }
        public string DeployType { get; set; }
    }
}
