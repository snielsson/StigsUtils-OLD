namespace StigsUtils.Github;

public class Class1 { }


// Update Gist using octokit:


// Target CI => _ => _
// 	.DependsOn(CodeCoverage)
// 	.Executes(() =>
// 	{
// 		if (GitHubActions == null) {
// 			Log.Information("Skipping Github Actions CI.");
// 			DeleteDirectory(TmpDir);
// 			return;
// 		}
// 		var gistToken = Environment.GetEnvironmentVariable("GIST_TOKEN");
// 		if (gistToken == null) {
// 			Log.Information("Skipping Github Actions because GIST_TOKEN is not defined.");
// 			return;
// 		}
// 		var gistId = "9240299ee79583c2ac27ce847c040f39";
// 		Log.Information("Updating https://gist.github.com/{gistId}", gistId);
// 		var gistUpdate = new GistUpdate { Description = $"CloudLens Github Action CI updated files on commit: {GitHubActions.Sha} at {DateTime.UtcNow}." };
// 		gistUpdate.Files["cloudlens.testresults.json"] = new GistFileUpdate {
//
// 			Content = CreateTestResultJson()
// 		};
// 		gistUpdate.Files["cloudlens.testcoverage.json"] = new GistFileUpdate {
//
// 			Content = CreateTestCoverageJson()
// 		};
// 		var client = new GitHubClient(new ProductHeaderValue("CloudlensCI")) {
// 			Credentials = new Credentials(gistToken)
// 		};
// 		client.Gist.Edit(gistId, gistUpdate).Wait();
// 		Log.Information("Updated {files} on https://gist.github.com/{gistId}", string.Join(',', gistUpdate.Files.Keys), gistId);
//
// 	});


// Using Github API with Octokit
			// This works:
			//TODO: each upload triggers a commit which triggers a build of pages. So need to do a single commit instead !
			// var g = new GitHubClient(new ProductHeaderValue("CloudLens.DevOps")) {
			// 	//TODO: use secret from env. (reset secret)!!
			// 	Credentials = new Credentials("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")
			// };
			// Repository r = g.Repository.Get(owner, name).Result;
			// var root = $"{Project}/{BuildId}";
			// var testResultJson = CreateTestResultJson(trxFilePath);
			// var testCoverageResults = CreateTestCoverageJson(coberturaFilePath);
			// var owner = "snielsson";
			// var name = "devops";
			// UploadFile(g, r.Id, root + "/testresults.json", testResultJson);
			// UploadFile(g, r.Id, root + "/testcoverageresults.json", testCoverageResults);
			// UploadFile(g, r.Id, root + "/testResults.trx", File.ReadAllText(trxFilePath));
			// UploadFile(g, r.Id, root + "/coverage.cobertura.xml", File.ReadAllText(coberturaFilePath));
			// var indexHtml = File.ReadAllText(coverageReportPath + "/index.html");
			// indexHtml = indexHtml.Replace("<html>", "<html><meta name=\"robots\" content=\"noindex,nofollow\">");
			// indexHtml = indexHtml.Replace("<h1>Summary<a class=\"button\" href=\"https://github.com/danielpalme/ReportGenerator\"", $"<h1>CloudLens Build {BuildId}<a class=\"button\" href=\"https://github.com/danielpalme/ReportGenerator\"");
			// UploadFile(g, r.Id, root + "/coveragereport.html", indexHtml);
			// var buildListHtml = CreateBuildListHtml(g, owner, name, Project);
			// CreateOrUpdateFile(g, owner, name, Project + "/index.html", BuildId, buildListHtml, true);
			// var redirectHtml = $@"<html><meta name=""robots"" content=""noindex,nofollow""><meta http-equiv=""refresh"" content=""0; url={BuildId}/coveragereport.html""></html>";
			// CreateOrUpdateFile(g, owner, name, Project + "/latest.build.redirect.html", BuildId, redirectHtml, true);
			// CreateOrUpdateFile(g, owner, name, Project + "/latest.testresults.json", BuildId, testResultJson, true);
			// CreateOrUpdateFile(g, owner, name, Project + "/latest.testcoverageresults.json", BuildId, testCoverageResults, true);
			// CreateOrUpdateFile(g, owner, name, Project + "/latest.buildstatus.json", BuildId, CreateStatusJson(!FailedTargets.Any()), true);
