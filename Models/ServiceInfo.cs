namespace KubeDeploy.Models;

public class ServiceInfo
{
    public string Name { get; set; }
    public string Project { get; set; }
    public int Port { get; set; }
    public int Replicas { get; set; }

}
