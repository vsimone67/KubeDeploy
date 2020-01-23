using System;
using CommandLine;

namespace KubeDeploy
{

    [Verb("remove-file", HelpText = "Remove Deployment Files From Project")]
    public class RemoveFileOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public bool AddConfig { get; set; }
    }

}
