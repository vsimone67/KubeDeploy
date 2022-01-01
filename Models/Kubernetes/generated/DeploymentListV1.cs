using Newtonsoft.Json;

namespace KubeClient.Models
{
    /// <summary>
    ///     DeploymentList is a list of Deployments.
    /// </summary>
    [KubeListItem("Deployment", "apps/v1")]
    [KubeObject("DeploymentList", "apps/v1")]
    public partial class DeploymentListV1 : KubeResourceListV1<DeploymentV1>
    {
        /// <summary>
        ///     Items is the list of Deployments.
        /// </summary>
        [JsonProperty("items", ObjectCreationHandling = ObjectCreationHandling.Reuse)]
        public override List<DeploymentV1> Items { get; } = new List<DeploymentV1>();
    }
}
