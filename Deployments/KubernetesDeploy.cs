using System;
using System.IO;
using System.Linq;
using KubeClient.Models;
using Kubernetes;

namespace KubernetesExtension
{
    public class KubernetesDeploy : DeployBase, IDeployment
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDir { get; set; }
        public int Replicas { get; set; }
        public bool AddConfig { get; set; }
        const string DockerHubAccount = "vsimone67";

        public void BuildAndDeployToCluster()
        {
            var appName = MakeDeploymentName(Name);
            BuildDockerandPublishDockerImage(appName, ProjectDir, KubeDir);
        }

        public void CreateDeploymentFiles()
        {

            Directory.CreateDirectory($"{ProjectDir}\\{KubeDir}");

            string deployYaml = GetKubeYamlText(AddConfig);
            string configYaml = GetSettingsForScript();
            deployYaml = deployYaml.Replace("NAMEGOESHERE", MakeDeploymentName(Name));
            deployYaml = deployYaml.Replace("NAMESPACEGOESHERE", NameSpace);

            configYaml = configYaml.Replace("NAMEGOESHERE", MakeDeploymentName(Name));
            configYaml = configYaml.Replace("NAMESPACEGOESHERE", NameSpace);

            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\deployment.yaml", deployYaml);
            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\createconfigs.ps1", configYaml);
            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\deploy.ps1", GetPowerShellDeployScript());
            File.WriteAllText($"{ProjectDir}\\{KubeDir}\\createnamespace.ps1", GetNamespaceScript());

        }

        public void DeployToCluster()
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

        public void DeleteDeployment()
        {
            var yamlDir = $"{ProjectDir}\\{KubeDir}";
            var kubeCommand = "delete -f deployment.yaml";

            Utils.RunProcess("kubectl.exe", kubeCommand, yamlDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        public void CheckDeploymentStatus()
        {

            var appName = MakeDeploymentName(Name);
            var projectDir = Path.GetDirectoryName(ProjectDir);
            var yamlDir = $"{projectDir}\\{KubeDir}";
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

        public DeploymentV1 GetDeploymentInfo()
        {
            KuberntesConnection kubeConnection = new KuberntesConnection();
            var deployments = kubeConnection.GetAllDeployments();
            var appName = MakeDeploymentName(Name);
            return deployments.Items.Where(exp => exp.Metadata.Name.ToUpper() == appName.ToUpper()).FirstOrDefault();
        }

        protected void DeployAllToCluster()
        {

            var yamlDir = $"{ProjectDir}\\{KubeDir}";
            var kubeCommand = "apply -f deployment.yaml";

            Utils.RunProcess("kubectl.exe", kubeCommand, yamlDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        protected void UpdateDeployment()
        {
            var appName = MakeDeploymentName(Name);
            var yamlDir = $"{ProjectDir}\\{KubeDir}";
            var knamespace = GetNameSpaceFromYaml(ProjectDir, KubeDir);
            var kubeCommand = $"set image deployment/{appName} {appName}-pod={DockerHubAccount}/{appName} --namespace {knamespace} --record";
            Utils.RunProcess("kubectl.exe", kubeCommand, yamlDir, true, Process_OutputDataReceived, Process_ErrorDataReceived);
        }

        public void RemoveDeploymentFiles()
        {
            Directory.Delete($"{ProjectDir}\\{KubeDir}", true);
        }

        protected override void Process_DockerBuildComplete(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Delay(5000).Wait();
            DeployToCluster();
        }

        #region Yaml/PS file contents

        private string GetNamespaceScript()
        {
            return $"kubectl create namespace { NameSpace}";
        }

        private string GetKubeYamlText(bool addConfig)
        {
            string header = @"apiVersion: v1
kind: Namespace
metadata:
  name: NAMESPACEGOESHERE
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: NAMEGOESHERE
  namespace: NAMESPACEGOESHERE
spec:
  selector:
    matchLabels:
      app: NAMEGOESHERE
  replicas: 1
  minReadySeconds: 10
  template:
    metadata:
      labels:
        app: NAMEGOESHERE
    spec:
      containers:
        - name: NAMEGOESHERE-pod
          image: vsimone67/NAMEGOESHERE:latest
          imagePullPolicy: ""Always""
          ports:
            - name: http
              containerPort: 80
";
            string configMaps = @"          env:
            - name: ""appdirectory""
              value: ""/app/settings/""
          volumeMounts:
            - name: configs
              mountPath: ""/app/settings""
      volumes:
            - name: configs
              projected:
                sources:
                  - configMap:
                      name: appsettings-NAMEGOESHERE
                  - secret:
                      name: appsettings-secret-NAMEGOESHERE";

            string service = @"
---
apiVersion: v1
kind: Service
metadata:
  name: NAMEGOESHERE-svc
  namespace: NAMESPACEGOESHERE
spec:
  ports:
    - name: http
      port: 80
      protocol: TCP
      targetPort: 80
  selector:
    app: NAMEGOESHERE
  type: LoadBalancer";

            return header + ((addConfig) ? configMaps : "") + service;
        }
    }

    #endregion Yaml/PS file contents
}