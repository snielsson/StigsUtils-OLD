// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Reflection;
using Microsoft.Build.Construction;
using StigsUtils.FileUtils;

internal sealed class XUnitService {
	public static void Run(DirPath? dirPath = null) {
		if (dirPath == null) {
			dirPath = new DirPath(Assembly.GetExecutingAssembly().Location);
		}
		var slnFile = dirPath.SearchUp("*.sln");
		if (slnFile == null) throw new ArgumentException($"No .sln file found in {dirPath} or parent directories.");
		Run(slnFile, "Debug");
	}

	public static void Run(FilePath solutionFilePath, string buildType) {
		if (solutionFilePath == null) throw new ArgumentNullException(nameof(solutionFilePath));
		SolutionFile? solutionFile = SolutionFile.Parse(solutionFilePath) ?? throw new ArgumentException($"Could not parse {solutionFilePath} as a solution file.");
		foreach (ProjectInSolution projectInSolution in solutionFile.ProjectsInOrder) {
			var project = ProjectRootElement.Open(projectInSolution.AbsolutePath);
			string targetFramework = project.Properties.SingleOrDefault(x => x.Name == "TargetFramework")?.Value ?? throw new Exception("Could not determine bin dir for " + project.FullPath);
			var projectName = Path.GetFileNameWithoutExtension(projectInSolution.AbsolutePath);
			var assemblyName = project.Properties.SingleOrDefault(x => x.Name == "AssemblyName")?.Value ?? projectName;
			var assemblyFilePath = Path.Combine(project.DirectoryPath, "bin",buildType, targetFramework,assemblyName+".dll");		
			bool e= File.Exists(assemblyFilePath);
			Console.WriteLine(assemblyFilePath);
		}
		
		
		//from XUnit console runner:
		//= Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetLocalCodeBase());
		
		
		
		// var assembly = Assembly.GetAssembly(typeof(UnitTest1));
		// var assemblyRunner = AssemblyRunner.WithoutAppDomain()

		// XunitFrontController controller = new XunitFrontController(
		// 	AppDomainSupport.IfAvailable, assembly?.Location);
		// AssemblyRunner.WithoutAppDomain()
		// controller.RunTests();
		// using (var runner = AssemblyRunner.WithAppDomain(testAssembly))
		// {
		// 	runner.OnDiscoveryComplete = OnDiscoveryComplete;
		// 	runner.OnExecutionComplete = OnExecutionComplete;
		// 	runner.OnTestFailed = OnTestFailed;
		// 	runner.OnTestSkipped = OnTestSkipped;
		//       
		// 	Console.WriteLine("Discovering...");
		// 	runner.Start(typeName);
		//       
		// 	finished.WaitOne();  // A ManualResetEvent
		// 	finished.Dispose();
		//       
		// 	return result;
	}
}