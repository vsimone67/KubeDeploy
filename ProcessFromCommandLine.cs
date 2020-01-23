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
            Parser.Default.ParseArguments<CreateOptions, FullBuildOptions, DeployOptions, BuildOptions, RemoveDeploymnetOptions, RemoveFileOptions>(args)

            .WithParsed<CreateOptions>(opts => CreateDeployment(opts))
            .WithParsed<FullBuildOptions>(opts => FullDeployToCluster(opts))
            .WithParsed<DeployOptions>(opts => DeployToCluster(opts))
            .WithParsed<BuildOptions>(opts => BuildProject(opts))
            .WithParsed<RemoveDeploymnetOptions>(opts => RemoveDeploymnetFromCluster(opts))
            .WithParsed<RemoveFileOptions>(opts => RemoveDeploymentFiles(opts))
            .WithNotParsed(errs => HandleParseError(errs));
        }

        private void CreateDeployment(CreateOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.CreateDeploymentFiles();
            Console.WriteLine($"Deployment files have been created for {opts.Name.Trim()}");

        }

        private void FullDeployToCluster(FullBuildOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.BuildAndDeployToCluster();
            Console.WriteLine($"Full deployment has completed for {opts.Name.Trim()}");
        }

        private void DeployToCluster(DeployOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.DeployToCluster();
            Console.WriteLine($"Deployment has been completed for {opts.Name.Trim()}");
        }

        private void BuildProject(BuildOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            Console.WriteLine("This needs to be added to the interface and kube");
        }
        private void RemoveDeploymnetFromCluster(RemoveDeploymnetOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            _deployment.DeleteDeployment();
            Console.WriteLine($"Deployment has been removed for {opts.Name.Trim()}");
        }


        private void RemoveDeploymentFiles(RemoveFileOptions opts)
        {
            _deployment.Name = opts.Name.TrimStart();
            _deployment.NameSpace = opts.NameSpace.TrimStart();
            _deployment.ProjectDir = opts.ProjectDir.TrimStart();
            _deployment.KubeDir = opts.KubeDirName;
            Console.WriteLine("Needs to add this to the interface and kube");
        }
        private void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors

        }
    }
}
