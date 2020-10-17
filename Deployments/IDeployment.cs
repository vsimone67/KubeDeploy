using KubeClient.Models;

namespace KubernetesExtension
{
    public interface IDeployment
    {
        bool HasDeploymnet();

        void CreateDeploymentFiles();

        void RemoveDeploymentFiles();

        void DeployToCluster();
        void PushToCluster();
        void Build();

        void DeleteDeployment();
        void CheckDeploymentStatus();

        void ScaleDeployment(int numberOfReplicas);

        void InitDeployment();

        DeploymentV1 GetDeploymentInfo();

        string Name { get; set; }
        string NameSpace { get; set; }
        string ProjectDir { get; set; }
        string KubeDir { get; set; }
        int Replicas { get; set; }
        string DeployType { get; set; }
        string DockerHubAccount { get; set; }
        int Port { get; set; }
        string Dns { get; set; }



    }
}