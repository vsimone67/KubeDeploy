
namespace KubeDeploy
{
    public class Program
    {
        static void Main(string[] args)
        {
            ProcessFromCommandLine commandLine = new ProcessFromCommandLine();
            commandLine.RunFromCommandLine(args);
        }
    }
}