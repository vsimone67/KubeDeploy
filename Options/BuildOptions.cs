using CommandLine;

namespace KubeDeploy
{
    [Verb("build", HelpText = "Build Project")]
    public class BuildOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public string DeployType { get; set; }
        public int Replicas { get; set; }
    }
}