using CommandLine;

namespace KubeDeploy
{
    [Verb("create", HelpText = "Create new deployment files.")]
    public class CreateOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public int Replicas { get; set; }
        public string DeployType { get; set; }
    }
}