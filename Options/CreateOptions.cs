using System.Collections.Generic;
using CommandLine;

namespace KubeDeploy
{
    [Verb("create", HelpText = "Create new deployment files.")]
    public class CreateOptions : ICreateOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }
        public string DeployType { get; set; }
        public string Dns { get; set; }
    }
}