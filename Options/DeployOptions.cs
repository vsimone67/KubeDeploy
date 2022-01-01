using CommandLine;

namespace KubeDeploy
{
    [Verb("deploy", HelpText = "Build and deploy to cluster")]
    public class DeployOptions : IBaseOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }

    }
}