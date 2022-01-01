using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace KubeClient.Models;

/// <summary>
///     PersistentVolumeClaimVolumeSource references the user's PVC in the same namespace. This volume finds the bound PV and mounts that volume for the pod. A PersistentVolumeClaimVolumeSource is, essentially, a wrapper around another type of volume that is owned by someone else (the system).
/// </summary>
public partial class PersistentVolumeClaimVolumeSourceV1
{
    /// <summary>
    ///     ClaimName is the name of a PersistentVolumeClaim in the same namespace as the pod using this volume. More info: https://kubernetes.io/docs/concepts/storage/persistent-volumes#persistentvolumeclaims
    /// </summary>
    [YamlMember(Alias = "claimName")]
    [JsonProperty("claimName", NullValueHandling = NullValueHandling.Include)]
    public string ClaimName { get; set; }

    /// <summary>
    ///     Will force the ReadOnly setting in VolumeMounts. Default false.
    /// </summary>
    [YamlMember(Alias = "readOnly")]
    [JsonProperty("readOnly", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ReadOnly { get; set; }
}
