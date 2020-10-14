using System.Collections.Generic;
using CommandLine;

namespace KubeDeploy
{
    [Verb("remove", HelpText = "Delete deployment files from project")]
    public class RemoveFileOptions : IBaseOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }
    }

}
