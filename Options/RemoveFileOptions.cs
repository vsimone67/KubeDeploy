using CommandLine;

namespace KubeDeploy
{

    [Verb("delete", HelpText = "Delete Deployment Files From Project")]
    public class RemoveFileOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public string DeployType { get; set; }
        public int Replicas { get; set; }
    }

}
