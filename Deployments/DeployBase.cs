using System;
using System.Diagnostics;

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
            string newName = AddDeploymnetType(name);
            return GetDeploymentName(newName);
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

        protected void BuildDockerandPublishDockerImage(string appName, string projectDir, string deployDir)
        {
            var psCommand = $"./deploy.ps1 -appName {appName} -projectDir {projectDir}";
            var psDir = $"{projectDir}\\{deployDir}";
            Utils.RunProcess("powershell.exe", psCommand, psDir, true, Process_OutputDataReceived, Process_ErrorDataReceived, Process_DockerBuildComplete);
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
            return @"param(
		[Parameter(Position=0, Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[System.String]
		$appName,

		[Parameter(Position=1, Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[System.String]
		$projectDir
	)

Set-Location $projectDir
docker build -t $appName -f ""$($projectDir)\dockerfile"".
docker tag ""$($appName):latest"" vsimone67/""$($appName):latest""
docker push vsimone67/""$($appName):latest""";
        }

        protected string GetSettingsForScript()
        {
            return @"kubectl create secret generic appsettings-secret-NAMEGOESHERE --namespace NAMESPACEGOESHERE --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-NAMEGOESHERE --namespace NAMESPACEGOESHERE --from-file=../appsettings.json";
        }
    }
}