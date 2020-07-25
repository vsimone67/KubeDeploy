using CommandLine;

namespace KubeDeploy
{
    [Verb("full", HelpText = "Build and deploy to cluster")]
    public class FullBuildOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public string DeployType { get; set; }
        public int Replicas { get; set; }
    }
}