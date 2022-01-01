using Newtonsoft.Json;

namespace KubeClient.Models
{
    /// <summary>
    ///     PodTemplateList is a list of PodTemplates.
    /// </summary>
    [KubeListItem("PodTemplate", "v1")]
    [KubeObject("PodTemplateList", "v1")]
    public partial class PodTemplateListV1 : KubeResourceListV1<PodTemplateV1>
    {
        /// <summary>
        ///     List of pod templates
        /// </summary>
        [JsonProperty("items", ObjectCreationHandling = ObjectCreationHandling.Reuse)]
        public override List<PodTemplateV1> Items { get; } = new List<PodTemplateV1>();
    }
}
