using Newtonsoft.Json;

namespace Helm.Release;

public class HelmReleases
{
    [JsonProperty("Next")]
    public string Next { get; set; }

    [JsonProperty("Releases")]
    public List<HelmRelease> Releases { get; set; }
}
