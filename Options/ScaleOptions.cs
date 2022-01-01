using CommandLine;

namespace KubeDeploy
{
    [Verb("scale", HelpText = "Scale a deployment with n replicas")]
    public class ScaleOptions : IScaleOptions
    {
        public string FileName { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public string KubeDirName { get; set; }
        public int Replicas { get; set; }
    }
}