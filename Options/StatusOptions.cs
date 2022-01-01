using CommandLine;

namespace KubeDeploy;

[Verb("status", HelpText = "Get deployment status")]
public class StatusOptions : IBaseOptions
{
    public string FileName { get; set; }
    public IEnumerable<string> Projects { get; set; }
    public string KubeDirName { get; set; }

}
