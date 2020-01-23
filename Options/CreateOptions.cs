using CommandLine;

namespace KubeDeploy
{
    [Verb("create", HelpText = "Create New Deployment Files.")]
    public class CreateOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public bool AddConfig { get; set; }
    }
}