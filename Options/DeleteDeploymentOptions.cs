using CommandLine;

namespace KubeDeploy
{
    [Verb("delete", HelpText = "Remove deployment from the cluster")]
    public class DeleteDeploymentOptions : IBaseOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }

    }
}