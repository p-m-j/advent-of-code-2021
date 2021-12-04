<Query Kind="Program">
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

public static class AdventOfCode
{
	// Write custom extension methods here. They will be available to all queries.

	public static IEnumerable<string> GetLines(string filename)
	{
		var root = Path.GetDirectoryName(Util.CurrentQueryPath);
		var input = Path.Combine(root, "inputs", filename);
		return File.ReadAllLines(input);
	}

	public static StreamReader GetReader(string filename)
	{
		var root = Path.GetDirectoryName(Util.CurrentQueryPath);
		var input = Path.Combine(root, "inputs", filename);
		return new StreamReader(File.Open(input, FileMode.Open));
	}

	public static string MapPath(string filename)
	{
		var root = Path.GetDirectoryName(Util.CurrentQueryPath);
		return Path.Combine(root, "inputs", filename);
	}
}
