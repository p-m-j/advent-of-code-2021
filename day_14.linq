<Query Kind="Program">
  <Namespace>Xunit</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var lines = AdventOfCode.GetLines(14, true);
    var start = lines.First();

    var map = lines
        .Skip(2)
        .Select(x => x.Split(" -> "))
        .ToDictionary(x => x[0], x => x[1].First());

    GetScore(start, map, 10).Dump("Part One");
    GetScore(start, map, 40).Dump("Part Two");
}


private long GetScore(string start, Dictionary<string, char> map, int steps)
{
    var scores = start.GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());

    var currentGen = new Dictionary<string, long>();

    for (var i = 0; i < start.Length - 1; i += 1)
    {
        var key = string.Join("", new ArraySegment<char>(start.ToArray(), i, 2));
        currentGen[key] = 1;
    }

    for (var i = 0; i < steps; i++)
    {
        var nextGen = new Dictionary<string, long>();

        foreach (var item in currentGen)
        {
            var a = map[item.Key];
            nextGen[$"{item.Key[0]}{a}"] = nextGen.GetValueOrDefault($"{item.Key[0]}{a}", 0) + currentGen[item.Key];
            nextGen[$"{a}{item.Key[1]}"] = nextGen.GetValueOrDefault($"{a}{item.Key[1]}", 0) + currentGen[item.Key];
            scores[a] = scores.GetValueOrDefault(a, 0) + currentGen[item.Key];
        }
        currentGen = nextGen;
    }

    return scores.Values.Max() - scores.Values.Min();
}
