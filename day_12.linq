<Query Kind="Program">
  <Namespace>Xunit</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var lines = AdventOfCode.GetLines("day_12.input").ToList(); ;
    var cs = new CaveSystem(lines);

    {
        HashSet<string> paths = new();
        cs.PopulatePaths(cs.Edges, "start", paths, 1);
        paths.Count.Dump("Part One");
    }
    
    {
        HashSet<string> paths = new();
        cs.PopulatePaths(cs.Edges, "start", paths, 2);
        paths.Count.Dump("Part Two");
    }
}

public class CaveSystem
{
    public Dictionary<string, HashSet<string>> Edges = new();

    public CaveSystem(List<string> lines)
    {
        Edges = lines.SelectMany(x => x.Split('-')).ToHashSet().ToDictionary(x => x, x => new HashSet<string>());
        foreach (var line in lines)
        {
            var pair = line.Split('-');
            Connect(pair[0], pair[1]);
        }
        foreach(var key in Edges.Keys)
        {
            Edges[key].Remove("start");
        }
    }

    private void Connect(string a, string b)
    {
        Edges[a].Add(b);
        Edges[b].Add(a);
    }
    
    private HashSet<string> AlreadyTried = new();

    public void PopulatePaths(Dictionary<string, HashSet<string>> caves, string start, HashSet<string> paths, int maxSmallVisits, string currentPath = "start")
    {
        if(!ValidateHistory(currentPath, maxSmallVisits))
        {
            return;
        }

        foreach (var path in caves[start])
        {
            PopulatePaths(caves, path, paths, maxSmallVisits, currentPath + $",{path}");
        }

        if (start == "end")
        {
            paths.Add(currentPath);
        }
    }
    
    public static bool ValidateHistory(string a, int maxSmallVisits)
    {
        var history = a.Split(',')
            .GroupBy(x => x)
            .Select(x => new { x.Key, Count = x.Count()})
            .Where(x => x.Key.ToUpper() != x.Key)
            .ToList();        

        if (history.Any(x => x.Key == "start" && x.Count > 1))
        {
            return false;
        }
        if (history.Any(x => x.Key == "end" && x.Count > 1))
        {
            return false;
        }
        if (history.Where(x => x.Count > 1).Count() > 1)
        {
            return false;
        }
        if(history.Any(x => x.Count > maxSmallVisits))
        {
            return false;
        }

        return true;
    }
}

#region private::Tests

[Theory]
[InlineData("start,A,c,A,c,b,b", false)]
[InlineData("start,A,c,A,c,b,c", false)]
[InlineData("start,A,c,A,c,b", true)]
void ValidateHistory_Works(string history, bool expected)
{
   Assert.Equal(expected, CaveSystem.ValidateHistory(history, 2));
}


#endregion
