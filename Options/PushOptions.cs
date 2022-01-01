using CommandLine;

namespace KubeDeploy;

[Verb("push", HelpText = "Deploy to cluster (pulls current container, no build)")]
public class PushOptions : IBaseOptions
{
    public string FileName { get; set; }
    public IEnumerable<string> Projects { get; set; }
    public string KubeDirName { get; set; }
}
