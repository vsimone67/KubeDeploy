using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace KubeClient.Models;

/// <summary>
///     APIServiceStatus contains derived information about an API server
/// </summary>
public partial class APIServiceStatusV1
{
    /// <summary>
    ///     Current service state of apiService.
    /// </summary>
    [MergeStrategy(Key = "type")]
    [YamlMember(Alias = "conditions")]
    [JsonProperty("conditions", ObjectCreationHandling = ObjectCreationHandling.Reuse)]
    public List<APIServiceConditionV1> Conditions { get; } = new List<APIServiceConditionV1>();

    /// <summary>
    ///     Determine whether the <see cref="Conditions"/> property should be serialised.
    /// </summary>
    public bool ShouldSerializeConditions() => Conditions.Count > 0;
}
