<Query Kind="Program">
  <NuGetReference>xunit</NuGetReference>
  <NuGetReference>xunit.runner.utility</NuGetReference>
  <Namespace>Xunit</Namespace>
</Query>

public static class AdventOfCode
{
    public static IEnumerable<string> GetLines(string filename)
    {
        var root = Path.GetDirectoryName(Util.CurrentQueryPath);
        var input = Path.Combine(root, "inputs", filename);
        return File.ReadAllLines(input);
    }

    public static IEnumerable<string> GetLines(string filename, bool demo)
    {
        var path = MapPath(filename, demo);
        return File.ReadAllLines(path);
    }

    public static IEnumerable<string> GetLines(int day, bool demo)
    {
        return GetLines($"day_{day}", demo);
    }

    public static string ReadAll(string filename, bool demo)
    {
        var extension = demo ? "demo" : "input";
        var root = Path.GetDirectoryName(Util.CurrentQueryPath);
        var input = Path.Combine(root, "inputs", $"{filename}.{extension}");
        return File.ReadAllText(input);
    }

    public static StreamReader GetReader(string filename)
    {
        var root = Path.GetDirectoryName(Util.CurrentQueryPath);
        var input = Path.Combine(root, "inputs", filename);
        return new StreamReader(File.Open(input, FileMode.Open));
    }

    public static string MapPath(string filename, bool demo)
    {
        var root = Path.GetDirectoryName(Util.CurrentQueryPath);
        var extension = demo ? "demo" : "input";
        var file = $"{filename}.{extension}";
        return Path.Combine(root, "inputs", file);
    }

    public static string ToEmoji(this bool input)
    {
        return input ? ":heavy_check_mark:": ":x:";
    }
}

/// <summary>Runs all xunit tests and displays the results. This method is editable in the 'xunit' query.</summary>
static TestResultSummary[] RunTests(bool quietly = false, bool reportFailuresOnly = false)
{
    using var runner = Xunit.Runners.AssemblyRunner.WithoutAppDomain(typeof(UserQuery).Assembly.Location);

    int totalTests = 0, completedTests = 0, failures = 0;
    runner.OnDiscoveryComplete = info => totalTests = info.TestCasesToRun;

    var tests = new TestResultSummary[0];
    var dc = new DumpContainer(Util.WithHeading(tests, "Test Results"));
    if (!quietly) dc.Dump(collapseTo: 1, repeatHeadersAt: 0);

    runner.OnTestFailed = info => AddTestResult(info);
    runner.OnTestPassed = info => AddTestResult(info);

    using var done = new ManualResetEventSlim();
    runner.OnExecutionComplete = info =>
    {
        if (!quietly) $"Completed {info.TotalTests} tests in {Math.Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed)".Dump();
        done.Set();
    };
    runner.Start();
    done.Wait();
    return tests;

    void AddTestResult(Xunit.Runners.TestInfo testInfo)
    {
        var summary = new TestResultSummary(testInfo);
        lock (dc)
        {
            completedTests++;
            if (summary.Failed()) failures++;

            if (!reportFailuresOnly || summary.Failed())
                tests = tests
                    .Append(summary)
                    .OrderBy(t => t.Succeeded())
                    .ThenBy(t => t.TypeName)
                    .ThenBy(t => t.MethodName)
                    .ThenBy(t => t.Case)
                    .ToArray();

            dc.Content = Util.WithHeading(tests, $"Test Results - {completedTests} of {totalTests} ({failures} failures)");
        }
    }
}

class TestResultSummary
{
    Xunit.Runners.TestInfo _testInfo;
    public TestResultSummary(Xunit.Runners.TestInfo testInfo) => _testInfo = testInfo;

    public bool Succeeded() => _testInfo is Xunit.Runners.TestPassedInfo;
    public bool Failed() => _testInfo is Xunit.Runners.TestFailedInfo;

    public string TypeName => _testInfo.TypeName;
    public string MethodName => _testInfo.MethodName;

    public string Case => _testInfo.TestDisplayName.Substring(
        _testInfo.TestDisplayName.StartsWith(TypeName + "." + MethodName) ? TypeName.Length + 1 + MethodName.Length : 0);

    public decimal? Seconds => (_testInfo as Xunit.Runners.TestExecutedInfo)?.ExecutionTime;

    public object Status =>
        _testInfo is Xunit.Runners.TestPassedInfo ? Util.WithStyle("Succeeded", "color:green") :
        _testInfo is Xunit.Runners.TestFailedInfo ? Util.WithStyle("Failed", "color:red") :
        "";

    public Xunit.Runners.TestFailedInfo FailureInfo => _testInfo as Xunit.Runners.TestFailedInfo;

    public object Location => Util.VerticalRun(
        from match in Regex.Matches(FailureInfo?.ExceptionStackTrace ?? "", @"(at .+?)\s+in\s+.+?LINQPadQuery:line\s+(\d+)")
        let line = int.Parse(match.Groups[2].Value)
        select Util.HorizontalRun(true, match.Groups[1].Value, new Hyperlinq(line - 1, 0, $"line {line}")));
}
