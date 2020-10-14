using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using KubeDeploy.Models;
using KubernetesExtension;
using YamlDotNet.RepresentationModel;

namespace KubeDeploy
{

    public class ProcessFromCommandLine
    {
        private IDeployment _deployment;

        private List<ServiceInfo> _services;
        private string _deployName;
        private string _nameSpace;
        private string _registry;

        public ProcessFromCommandLine()
        {
            _deployment = new KubernetesDeploy();
            _services = new List<ServiceInfo>();
        }
        public void RunFromCommandLine(string[] args)
        {
            Parser.Default.ParseArguments<CreateOptions, FullBuildOptions>(args)
                                            .WithParsed<CreateOptions>(opts => CreateDeployment(opts))
                                            .WithParsed<FullBuildOptions>(opts => FullDeployToCluster(opts))
                                            // .WithParsed<DeployOptions>(opts => DeployToCluster(opts))
                                            // .WithParsed<BuildOptions>(opts => BuildProject(opts))
                                            // .WithParsed<RemoveDeploymnetOptions>(opts => RemoveDeploymnetFromCluster(opts))
                                            // .WithParsed<RemoveFileOptions>(opts => RemoveDeploymentFiles(opts))
                                            // .WithParsed<ScaleOptions>(opts => ScaleDeployment(opts))
                                            // .WithParsed<StatusOptions>(opts => CheckDeploymentStatus(opts))
                                            //.WithParsed<BooBatOptions>(opts => boobat(opts))
                                            .WithNotParsed(errs => HandleParseError(errs));
        }

        // create kube deploy files from yaml file
        // create kube deploy files from command line (no yaml)
        // build and push to repository
        // build, push to repository, and deploy to cluster
        // deploy to cluster
        // delete deployment
        // remove kube deploy files
        // scale pods
        // check deployment
        private void boobat(BooBatOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.DeployType = opts.DeployType;
                _deployment.DockerHubAccount = _registry;
                _deployment.Replicas = service.Replicas;
                _deployment.Port = service.Port;
                _deployment.CreateDeploymentFiles();
            }
        }

        private void CreateDeployment(CreateOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.DeployType = opts.DeployType;
                _deployment.DockerHubAccount = _registry;
                _deployment.Replicas = service.Replicas;
                _deployment.Port = service.Port;
                _deployment.CreateDeploymentFiles();
            }
            ConsoleMessage($"Deployment files have been created for {opts.Name.Trim()}");

        }

        private void FullDeployToCluster(FullBuildOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.BuildAndDeployToCluster();
                ConsoleMessage($"Full deployment has completed for {opts.Name.Trim()}");
            }
        }

        private void DeployToCluster(DeployOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.DeployToCluster();
            ConsoleMessage($"Deployment has been completed for {opts.Name.Trim()}");
        }

        private void BuildProject(BuildOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.Build();
            ConsoleMessage($"Deployment {opts.Name.Trim()} has been built and moved to docker hub");

        }
        private void RemoveDeploymnetFromCluster(RemoveDeploymnetOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.DeleteDeployment();
            ConsoleMessage($"Deployment has been removed for {opts.Name.Trim()}");
        }


        private void RemoveDeploymentFiles(RemoveFileOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.RemoveDeploymentFiles();
            ConsoleMessage($"Deployment files have been removed for {opts.Name.Trim()}");

        }

        private void ScaleDeployment(ScaleOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;

            _deployment.ScaleDeployment(opts.Replicas);

            ConsoleMessage($"Deployment {opts.Name.Trim()} has been scaled to {opts.Replicas} replicas");
        }

        private void CheckDeploymentStatus(StatusOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.CheckDeploymentStatus();
        }
        private void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors

        }

        private void ConsoleMessage(string message)
        {
            var currentColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }

        private void ParseYamlFile(IBaseOptions options)
        {
            var input = new StreamReader(options.FileName);


            // Load the stream
            var yaml = new YamlStream();
            yaml.Load(input);

            // Examine the stream
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            _deployName = mapping.Children["name"].ToString();
            _nameSpace = mapping.Children["namespace"].ToString();
            _registry = mapping.Children["registry"].ToString();

            // Gather all the service info and save it
            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("services")];
            foreach (YamlMappingNode item in items)
            {
                var binding = (YamlSequenceNode)item.Children[new YamlScalarNode("bindings")];
                var port = binding.Children[0][new YamlScalarNode("port")].ToString();
                var name = item.Children[new YamlScalarNode("name")].ToString();
                var project = item.Children[new YamlScalarNode("project")].ToString();

                var replicas = 1;
                if (item.Children.ContainsKey("replicas"))
                    replicas = int.Parse(item.Children[new YamlScalarNode("replicas")].ToString());

                _services.Add(new ServiceInfo() { Name = name, Project = project, Port = int.Parse(port), Replicas = replicas });

            }


        }

        private string GetProjectName(string project)
        {
            int pos = project.IndexOf("/");
            string newProject = project;

            if (pos >= 0)
                newProject = project.Substring(0, pos);

            return newProject;
        }
    }
}
