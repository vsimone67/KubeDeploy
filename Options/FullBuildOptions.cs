using CommandLine;

namespace KubeDeploy
{
    [Verb("full", HelpText = "Build and Deploy to Cluster")]
    public class FullBuildOptions : IOptions
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDirName { get; set; }
        public bool AddConfig { get; set; }
    }
}