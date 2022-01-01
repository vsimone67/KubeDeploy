using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace KubeClient.Models
{
    /// <summary>
    ///     ContainerStateRunning is a running state of a container.
    /// </summary>
    public partial class ContainerStateRunningV1
    {
        /// <summary>
        ///     Time at which the container was last (re-)started
        /// </summary>
        [YamlMember(Alias = "startedAt")]
        [JsonProperty("startedAt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartedAt { get; set; }
    }
}
