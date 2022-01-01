using CommandLine;

namespace KubeDeploy;

[Verb("clean", HelpText = "Delete deployment files from project")]
public class CleanFileOptions : IBaseOptions
{
    public string FileName { get; set; }
    public IEnumerable<string> Projects { get; set; }
    public string KubeDirName { get; set; }
}
