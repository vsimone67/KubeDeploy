using System.Collections.Generic;
using CommandLine;

namespace KubeDeploy
{
    public interface ICreateOptions : IBaseOptions
    {

        [Option("deploy-type", Required = false, HelpText = "The deploy type you are createing (basic, full, traefik, traefik-basic)", Default = "traefik")]
        public string DeployType { get; set; }

    }

    public interface IScaleOptions : IBaseOptions
    {
        [Option('r', "replicas", Required = false, HelpText = "Number of replicas to scale the deplolyment to")]
        public int Replicas { get; set; }
    }

    public interface IBaseOptions
    {
        [Option('f', "filename", Required = false, HelpText = "Yaml file to use for deploy", Default = "tye.yaml")]
        string FileName { get; set; }

        [Option('i', "include", Required = false, HelpText = "Only act on specified projects")]
        IEnumerable<string> Projects { get; set; }

        [Option("kubedir", Required = false, HelpText = "Name to Use For Proj Dir When Created", Default = "k8")]
        public string KubeDirName { get; set; }
    }
}
