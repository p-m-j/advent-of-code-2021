<Query Kind="Program" />

public static class AdventOfCode
{
    public static IEnumerable<string> GetLines(string filename)
    {
        var root = Path.GetDirectoryName(Util.CurrentQueryPath);
        var input = Path.Combine(root, "inputs", filename);
        return File.ReadAllLines(input);
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

    public static string MapPath(string filename)
    {
        var root = Path.GetDirectoryName(Util.CurrentQueryPath);
        return Path.Combine(root, "inputs", filename);
    }

    public static string ToEmoji(this bool input)
    {
        return input ? ":heavy_check_mark:": ":x:";
    }
}
