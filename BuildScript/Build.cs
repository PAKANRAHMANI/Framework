using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace _build
{
	[CheckBuildProjectConfigurations]
	[UnsetVisualStudioEnvironmentVariables]
	class Build : NukeBuild
	{
		public static int Main() => Execute<Build>(x => x.Push);

		[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
		readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

		[Solution] readonly Solution Solution;
		AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
		[Parameter] string _nugetApiUrl = "https://azure.charisma.tech/Brokerage/_packaging/Charisma_Trader_Framework_Backend/nuget/v3/index.json";
		//[GitVersion] readonly GitVersion GitVersion;
		Target Clean => _ => _
			.Before(Restore)
			.Executes(() =>
			{
				GlobDirectories(Solution.Directory, "**/bin", "**/obj").ForEach(DeleteDirectory);
			});

		Target Restore => _ => _
			.DependsOn(Clean)
			.Executes(() =>
			{
				DotNetRestore(a => a.SetProjectFile(Solution));
			});

		Target Compile => _ => _
			.DependsOn(Restore)
			.Executes(() =>
			{
				DotNetBuild(a =>
					a.SetProjectFile(Solution)
						.SetConfiguration(Configuration)
						.SetNoRestore(true));
			});
		Target RunUnitTests => _ => _
			.DependsOn(Compile)
			.Executes(() =>
			{
				var unitTestProjects = Solution.AllProjects.Where(a => a.Name.EndsWith(".Tests.Unit"));
				foreach (var testProject in unitTestProjects)
				{
					DotNetTest(a =>
						a.SetProjectFile(testProject)
							.SetNoRestore(true)
							.SetConfiguration(Configuration)
							.SetNoBuild(true));
				}
			});
		Target Pack => _ => _
			.DependsOn(RunUnitTests)
			.Executes(() =>
			{
				DotNetPack(s => s
					.SetProject(Solution)
					.SetConfiguration(Configuration)
					.EnableNoBuild()
					.EnableNoRestore()
					.SetCopyright("Charisma")
					.SetDescription("Created during CI pipeline by 'Nuke'")
					.SetPackageTags("Charisma", "Trader", "Framework")
					.SetAuthors("Trader Team")
					.SetNoDependencies(true)
					.SetOutputDirectory(ArtifactsDirectory / "NugetPackages"));
			});
		Target Push => _ => _
			.DependsOn(Pack)
			.Executes(() =>
			{
				var nugetPackages = Directory.GetFiles(ArtifactsDirectory / "NugetPackages", "*.nupkg", SearchOption.AllDirectories).ToList();


				foreach (var nupkg in nugetPackages)
				{

					Logger.Info($"Nuget Push : {nupkg}");

					DotNetNuGetPush(a =>
						a.SetSource(_nugetApiUrl)
							.SetApiKey("az")
							.SetTargetPath(nupkg)
							.SetSkipDuplicate(true)
					);

				}


			});

	}
}

