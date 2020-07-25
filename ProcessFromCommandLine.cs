using System;
using System.Collections.Generic;
using CommandLine;
using KubernetesExtension;

namespace KubeDeploy
{
    public class ProcessFromCommandLine
    {
        private IDeployment _deployment;

        public ProcessFromCommandLine()
        {
            _deployment = new KubernetesDeploy();
        }
        public void RunFromCommandLine(string[] args)
        {
            Parser.Default.ParseArguments<CreateOptions, FullBuildOptions, DeployOptions, BuildOptions,
                                          RemoveDeploymnetOptions, RemoveFileOptions, ScaleOptions,
                                          StatusOptions>(args)

            .WithParsed<CreateOptions>(opts => CreateDeployment(opts))
            .WithParsed<FullBuildOptions>(opts => FullDeployToCluster(opts))
            .WithParsed<DeployOptions>(opts => DeployToCluster(opts))
            .WithParsed<BuildOptions>(opts => BuildProject(opts))
            .WithParsed<RemoveDeploymnetOptions>(opts => RemoveDeploymnetFromCluster(opts))
            .WithParsed<RemoveFileOptions>(opts => RemoveDeploymentFiles(opts))
            .WithParsed<ScaleOptions>(opts => ScaleDeployment(opts))
            .WithParsed<StatusOptions>(opts => CheckDeploymentStatus(opts))
            .WithNotParsed(errs => HandleParseError(errs));
        }

        private void CreateDeployment(CreateOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.DeployType = opts.DeployType;
            _deployment.CreateDeploymentFiles();
            ConsoleMessage($"Deployment files have been created for {opts.Name.Trim()}");

        }

        private void FullDeployToCluster(FullBuildOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.BuildAndDeployToCluster();
            ConsoleMessage($"Full deployment has completed for {opts.Name.Trim()}");
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
    }
}
