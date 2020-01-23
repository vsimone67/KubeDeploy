using CommandLine;

namespace KubeDeploy
{
    [Verb("scale", HelpText = "Scale a deployment with n replicas")]
    public class ScaleOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public bool AddConfig { get; set; }
        public int Replicas { get; set; }
    }
}