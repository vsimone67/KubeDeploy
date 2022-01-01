using CommandLine;

namespace KubeDeploy.Options;

[Verb("init", HelpText = "Run setup files (configmap and namespace) for project")]
public class InitOptions : IBaseOptions
{
    public string FileName { get; set; }
    public IEnumerable<string> Projects { get; set; }
    public string KubeDirName { get; set; }

}
