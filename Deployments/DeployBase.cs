using System.Diagnostics;
using System.Reflection;

namespace KubernetesExtension
{
    public class DeployBase
    {

        protected string NormalizeAppName(string name)
        {
            return name.Replace(" ", "_").ToLower();
        }

        protected string AddDeploymnetType(string name, string prefix = "")
        {
            string newName = name;

            if (name.ToUpper().Contains("SERVICE"))
            {
                newName += $"{prefix}service";
            }

            if (name.ToUpper().Contains("WEB") || name.ToUpper().Contains("PRESENTATION"))
            {
                newName += $"{prefix}web";
            }

            if (name.ToUpper().Contains("API"))
            {
                newName += $"{prefix}gateway";
            }

            return newName;
        }

        protected string GetDeploymentName(string name)
        {
            name = NormalizeAppName(name);
            string newName = name;

            // remove any project portions of the name e.g. project.domain.name
            if (name.Contains("."))
            {
                // just get the right most portion of the project
                int pos = name.LastIndexOf(".");
                newName = name.Substring(pos + 1);
            }

            return newName;
        }

        protected string MakeDeploymentName(string name)
        {
            //string newName = AddDeploymnetType(name);
            return GetDeploymentName(name);
        }

        protected async void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

            if (e.Data != null)
            {
                await Console.Out.WriteLineAsync(e.Data);
            }
        }

        protected async void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                await Console.Out.WriteLineAsync(e.Data);
            }
        }

        protected void BuildPublishAndDeploy(string appName, string projectDir, string deployDir)
        {
            var psCommand = $"./deploy.ps1 -appName {appName} -projectDir ..";
            var psDir = $"{projectDir}\\{deployDir}";

            Utils.RunProcess("pwsh.exe", psCommand, psDir, true, Process_OutputDataReceived, Process_ErrorDataReceived, Process_DockerBuildComplete);
        }

        protected void BuildAndPublishDockerImage(string appName, string projectDir, string deployDir)
        {
            var psCommand = $"./deploy.ps1 -appName {appName} -projectDir ..";
            var psDir = $"{projectDir}\\{deployDir}";

            Utils.RunProcess("pwsh.exe", psCommand, psDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        protected string GetNameSpaceFromYaml(string projectDir, string kubeDir)
        {
            var retval = string.Empty;


            var filename = $"{projectDir}\\{kubeDir}\\deployment.yaml";

            GetValueFromFile file = new GetValueFromFile();
            retval = file.GetValue(filename, "namespace");

            return retval;
        }

        protected virtual void Process_DockerBuildComplete(object sender, EventArgs e)
        {
        }

        protected string GetPowerShellDeployScript()
        {
            var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return File.ReadAllText($"{rootDir}/Templates/deploy-script.ps1");
        }

        protected string GetSettingsForScript()
        {
            var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            return File.ReadAllText($"{rootDir}/Templates/configmap-template.ps1");
        }

        protected string GetSettingsForKubeDeployScript()
        {
            var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            return File.ReadAllText($"{rootDir}/Templates/kubedeploy-template.ps1");
        }
    }
}