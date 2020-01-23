using CommandLine;

namespace KubeDeploy
{
    [Verb("remove-deployment", HelpText = "Remove Project")]
    public class RemoveDeploymnetOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public bool AddConfig { get; set; }
    }
}