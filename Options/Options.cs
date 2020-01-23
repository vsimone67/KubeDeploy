using CommandLine;

namespace KubeDeploy
{

    public interface IOptions
    {
        [Option("name", Required = true, HelpText = "Name of Project")]
        public string Name { get; set; }

        [Option("namespace", Required = false, HelpText = "Namespace of Cluster", Default = "playground")]
        public string NameSpace { get; set; }

        [Option("projdir", Required = false, HelpText = "Directory of Project ((Get-Location).path for current dir in powershell)")]
        public string ProjectDir { get; set; }

        [Option("kubedir", Required = false, HelpText = "Name to Use For Proj Dir When Created", Default = "k8")]
        public string KubeDirName { get; set; }

        [Option("add-config", Required = false, HelpText = "Add Config Info To Deployment File", Default = false)]
        public bool AddConfig { get; set; }
    }
}