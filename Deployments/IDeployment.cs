using KubeClient.Models;

namespace KubernetesExtension
{
    public interface IDeployment
    {
        bool HasDeploymnet();

        void CreateDeploymentFiles();

        void RemoveDeploymentFiles();

        void BuildAndDeployToCluster();
        void DeployToCluster();
        void Build();

        void DeleteDeployment();
        void CheckDeploymentStatus();

        void ScaleDeployment(int numberOfReplicas);

        DeploymentV1 GetDeploymentInfo();

        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDir { get; set; }
        public int Replicas { get; set; }

        public string DeployType { get; set; }

    }
}