using System;
using System.IO;
using System.Linq;
using System.Reflection;
using KubeClient.Models;
using Kubernetes;

namespace KubernetesExtension
{
    public class KubernetesDeploy : DeployBase, IDeployment
    {
        const string dnsSuffix = ".titan";
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDir { get; set; }
        public int Replicas { get; set; }
        public string DeployType { get; set; }
        public string DockerHubAccount { get; set; }
        public int Port { get; set; }
        public string Dns { get; set; }

        public void DeployToCluster()
        {
            //var appName = MakeDeploymentName(Name);
            BuildPublishAndDeploy(Name, ProjectDir, KubeDir);
        }

        public void CreateDeploymentFiles()
        {
            Directory.CreateDirectory($"{ProjectDir}\\{KubeDir}");

            string deployYaml = GetKubeYamlText(DeployType);
            string configYaml = GetSettingsForScript();
            string deployScript = GetSettingsForKubeDeployScript();
            string dockerFile = GetDockerFile();
            string middlewareYaml = GetMiddleWare();

            deployYaml = deployYaml.Replace("NAMEGOESHERE", MakeDeploymentName(Name));
            deployYaml = deployYaml.Replace("NAMESPACEGOESHERE", NameSpace);
            deployYaml = deployYaml.Replace("DNSHERE", Dns);
            deployYaml = deployYaml.Replace("ACTUATOR", Name);
            deployYaml = deployYaml.Replace("SERVICENAME", Name.Replace("service", ""));
            deployYaml = deployYaml.Replace("SERVICENAME", Name.Replace("processor", ""));

            configYaml = configYaml.Replace("NAMEGOESHERE", MakeDeploymentName(Name));
            configYaml = configYaml.Replace("NAMESPACEGOESHERE", NameSpace);

            deployScript = deployScript.Replace("NAMEGOESHERE", MakeDeploymentName(Name));
            deployScript = deployScript.Replace("NAMESPACEGOESHERE", NameSpace);

            dockerFile = dockerFile.Replace("SERVICENAME", ProjectDir);

            middlewareYaml = middlewareYaml.Replace("NAMESPACEGOESHERE", NameSpace);

            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\deployment.yaml", deployYaml);
            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\createconfigs.ps1", configYaml);
            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\deploy.ps1", GetPowerShellDeployScript());
            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\createnamespace.ps1", GetNamespaceScript());
            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\middleware.yaml", middlewareYaml);
            File.WriteAllText($"{ProjectDir}\\Dockerfile", dockerFile);

        }

        public void PushToCluster()
        {
            if (!HasDeploymnet())
            {
                DeployAllToCluster();
            }
            else
            {
                UpdateDeployment();
            }
        }

        public bool HasDeploymnet()
        {
            bool retval = false;
            KuberntesConnection kubeConnection = new KuberntesConnection();
            var appName = MakeDeploymentName(Name);
            var deployments = kubeConnection.GetAllDeployments();

            retval = deployments.Items.Any(exp => exp.Metadata.Name.ToUpper() == appName.ToUpper());
            return retval;
        }
        public bool KubeDirExists()
        {
            return Directory.Exists($"{ProjectDir}\\{KubeDir}");
        }

        public void DeleteDeployment()
        {
            var yamlDir = $"{ProjectDir}\\{KubeDir}";
            var kubeCommand = "delete -f deployment.yaml";

            Utils.RunProcess("kubectl.exe", kubeCommand, yamlDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        public void CheckDeploymentStatus()
        {
            var appName = MakeDeploymentName(Name);
            //var projectDir = Path.GetDirectoryName(ProjectDir);
            var yamlDir = $"{ProjectDir}\\{KubeDir}";
            var knamespace = GetNameSpaceFromYaml(ProjectDir, KubeDir);
            var kubeCommand = $"rollout status deploy/{appName} --namespace {knamespace}";

            Utils.RunProcess("kubectl.exe", kubeCommand, yamlDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        public void ScaleDeployment(int numberOfReplicas)
        {
            string kNameSpace = GetNameSpaceFromYaml(ProjectDir, KubeDir);
            string deploymentName = MakeDeploymentName(Name);
            KuberntesConnection kubernetesConnection = new KuberntesConnection();
            kubernetesConnection.ScaleDeployment(deploymentName, numberOfReplicas, kNameSpace);
        }

        public void InitDeployment()
        {

            if (!KubeDirExists())
                throw new Exception("Kubernetes files do not exist, please run create");

            var psDir = $"{ProjectDir}\\{KubeDir}";

            // create namespace
            var psCommand = $"./createnamespace.ps1";
            Utils.RunProcess("powershell.exe", psCommand, psDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);

            // create config
            psCommand = $"./createconfigs.ps1";
            Utils.RunProcess("powershell.exe", psCommand, psDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);

            // create config
            psCommand = $"apply -f middleware.yaml";
            Utils.RunProcess("kubectl.exe", psCommand, psDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);

        }

        public DeploymentV1 GetDeploymentInfo()
        {
            KuberntesConnection kubeConnection = new KuberntesConnection();
            var deployments = kubeConnection.GetAllDeployments();
            var appName = MakeDeploymentName(Name);
            return deployments.Items.Where(exp => exp.Metadata.Name.ToUpper() == appName.ToUpper()).FirstOrDefault();
        }

        protected void DeployAllToCluster()
        {
            if (!KubeDirExists())
                throw new Exception("Kubernetes files do not exist, please run create");

            var yamlDir = $"{ProjectDir}\\{KubeDir}";
            var kubeCommand = "apply -f deployment.yaml";

            Utils.RunProcess("kubectl.exe", kubeCommand, yamlDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        protected void UpdateDeployment()
        {
            if (!KubeDirExists())
                throw new Exception("Kubernetes files do not exist, please run create");

            var appName = MakeDeploymentName(Name);
            var yamlDir = $"{ProjectDir}\\{KubeDir}";
            var knamespace = GetNameSpaceFromYaml(ProjectDir, KubeDir);
            var kubeCommand = $"set image deployment/{appName} {appName}-pod={DockerHubAccount}/{appName} --namespace {knamespace} --record";

            Console.WriteLine($"The kube command is {kubeCommand}");
            Utils.RunProcess("kubectl.exe", kubeCommand, yamlDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        public void Build()
        {
            if (!KubeDirExists())
                throw new Exception("Kubernetes files do not exist, please run create");

            var appName = MakeDeploymentName(Name);
            BuildAndPublishDockerImage(appName, ProjectDir, KubeDir);

        }
        public void RemoveDeploymentFiles()
        {
            Directory.Delete($"{ProjectDir}\\{KubeDir}", true);
        }

        protected override void Process_DockerBuildComplete(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Delay(5000).Wait();
            PushToCluster();
        }

        #region Yaml/PS file contents

        private string GetNamespaceScript()
        {
            return $"kubectl create namespace { NameSpace}";
        }

        private string GetMiddleWare()
        {
            return GetFile("middleware.yaml");
        }
        private string GetKubeYamlText(string deployType)
        {
            string fileName = $"{deployType}-deployment.yaml";
            return GetFile(fileName);
        }
        private string GetDockerFile()
        {
            string fileName = $"DockerFile";
            return GetFile(fileName);
        }

        private string GetFile(string fileName)
        {
            var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return File.ReadAllText($"{rootDir}/Templates/{fileName}");
        }
        private string MakeDns(string name)
        {
            return name.Replace("service", "") + dnsSuffix;
        }
    }

    #endregion Yaml/PS file contents


}