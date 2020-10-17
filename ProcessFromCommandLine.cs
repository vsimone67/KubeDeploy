using System.Runtime.InteropServices;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using KubeDeploy.Models;
using KubeDeploy.Options;
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

            try
            {
                Parser.Default.ParseArguments<CreateOptions, DeployOptions, PushOptions, BuildOptions, DeleteDeploymentOptions,
                                              CleanFileOptions, InitOptions, StatusOptions, ScaleOptions>(args)
                                                .WithParsed<CreateOptions>(opts => CreateDeployment(opts))
                                                .WithParsed<DeployOptions>(opts => DeployToCluster(opts))
                                                .WithParsed<PushOptions>(opts => PushToCluster(opts))
                                                .WithParsed<BuildOptions>(opts => BuildProject(opts))
                                                .WithParsed<DeleteDeploymentOptions>(opts => DeleteDeploymentFromCluster(opts))
                                                .WithParsed<CleanFileOptions>(opts => CleanDeploymentFiles(opts))
                                                .WithParsed<InitOptions>(opts => InitDeployment(opts))
                                                .WithParsed<ScaleOptions>(opts => ScaleDeployment(opts))
                                                .WithParsed<StatusOptions>(opts => CheckDeploymentStatus(opts))
                                                .WithNotParsed(errs => HandleParseError(errs));
            }
            catch (Exception ex)
            {
                ConsoleErrorMessage(ex.Message);
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
                _deployment.Dns = opts.Dns;
                _deployment.CreateDeploymentFiles();
                ConsoleMessage($"Deployment files have been created for {service.Name.Trim()}");
            }
        }
        private void DeployToCluster(DeployOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.DeployToCluster();
                ConsoleMessage($"{service.Name.Trim()} has been deployed to the cluster");
                _deployment.CheckDeploymentStatus();
            }
        }
        private void PushToCluster(PushOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.PushToCluster();
                ConsoleMessage($"{service.Name.Trim()} has been deployed to the cluster");
                _deployment.CheckDeploymentStatus();
            }
        }

        private void BuildProject(BuildOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.Build();
                ConsoleMessage($"Deployment {service.Name.Trim()} has been built and moved to docker hub");
            }
        }
        private void DeleteDeploymentFromCluster(DeleteDeploymentOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.DeleteDeployment();
                ConsoleMessage($"Deployment {service.Name.Trim()} has been removed from the cluster");
            }
        }
        private void CleanDeploymentFiles(CleanFileOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.RemoveDeploymentFiles();
                ConsoleMessage($"Files have been removed from {service.Name.Trim()}");
            }
        }

        private void InitDeployment(InitOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.InitDeployment();
                ConsoleMessage($"Deployment namespace and configmap for {service.Name.Trim()} has been created");
            }
        }
        private void ScaleDeployment(ScaleOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.ScaleDeployment(opts.Replicas);
                ConsoleMessage($"Deployment {service.Name} has been scaled to {opts.Replicas} replicas");
            }
        }

        private void CheckDeploymentStatus(StatusOptions opts)
        {
            ParseYamlFile(opts);

            foreach (var service in _services)
            {
                _deployment.Name = service.Name;
                _deployment.NameSpace = _nameSpace;
                _deployment.ProjectDir = GetProjectName(service.Project);
                _deployment.KubeDir = opts.KubeDirName;
                _deployment.CheckDeploymentStatus();
            }
        }
        private void HandleParseError(IEnumerable<Error> errs)
        {
        }
        private void ConsoleMessage(string message)
        {
            var currentColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }

        private void ConsoleErrorMessage(string message)
        {
            var currentColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }

        private void ParseYamlFile(IBaseOptions options)
        {
            if (File.Exists(options.FileName))
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
                    var name = item.Children[new YamlScalarNode("name")].ToString().ToLower();
                    var project = item.Children[new YamlScalarNode("project")].ToString();

                    var replicas = 1;
                    if (item.Children.ContainsKey("replicas"))
                        replicas = int.Parse(item.Children[new YamlScalarNode("replicas")].ToString());

                    if (options.Projects.Contains(name) || options.Projects.Count() == 0)
                        _services.Add(new ServiceInfo() { Name = name, Project = project, Port = int.Parse(port), Replicas = replicas });
                }
            }
            else
            {
                ConsoleErrorMessage($"File {options.FileName} does not exist");
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
