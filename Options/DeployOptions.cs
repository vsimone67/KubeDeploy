using CommandLine;

namespace KubeDeploy
{
    [Verb("deploy", HelpText = "Deploy to cluster")]

    public class DeployOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public bool AddConfig { get; set; }
        public int Replicas { get; set; }
    }
}