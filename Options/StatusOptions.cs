using CommandLine;

namespace KubeDeploy
{
    [Verb("status", HelpText = "Get deployment status")]
    public class StatusOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public bool AddConfig { get; set; }
        public int Replicas { get; set; }
    }
}