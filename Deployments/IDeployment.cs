using KubeClient.Models;

namespace KubernetesExtension
{
    public interface IDeployment
    {
        //bool HasDeploymentConfiguration();

        bool HasDeploymnet();

        void CreateDeploymentFiles();

        //void RemoveDeploymentFiles();

        void BuildAndDeployToCluster();
        void DeployToCluster();

        void DeleteDeployment();
        void CheckDeploymentStatus();

        void ScaleDeployment(int numberOfReplicas);

        DeploymentV1 GetDeploymentInfo();

        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string ProjectDir { get; set; }
        public string KubeDir { get; set; }

    }
}