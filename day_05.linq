<Query Kind="Program" />

#load ".\AdventOfCode"

void Main()
{
    var inputs = AdventOfCode.GetLines("day_5.input");

    var lines = inputs
        .Select(x => new Line(x))
        .ToArray();

    GetScore(lines, considderDiagonals:false).Dump("Part One");
    GetScore(lines, considderDiagonals:true).Dump("Part Two");
}

private int GetScore(Line[] lines, bool considderDiagonals)
{
    var floorMap = new Dictionary<(int, int), int>();

    foreach (var line in lines)
    {
        foreach (var point in line.GetAllPoints(considderDiagonals))
        {
            if (!floorMap.ContainsKey(point))
            {
                floorMap[point] = 1;
                continue;
            }

            floorMap[point] += 1;
        }
    }

    return floorMap.Values.Count(x => x > 1);
}

public class Line
{
    public int X1 { get; }
    public int Y1 { get; }
    public int X2 { get; }
    public int Y2 { get; }

    public bool IsHoriztonal => Y1 == Y2 && X1 != X2;
    public bool IsVertical => X1 == X2 && Y1 != Y2;
    public bool IsDiagonal => X1 != X2 && Y1 != Y2;

    public Line(string input)
    {
        var parts = input
            .Replace(" -> ", ",")
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        X1 = parts[0];
        Y1 = parts[1];
        X2 = parts[2];
        Y2 = parts[3];
    }

    public IEnumerable<(int, int)> GetAllPoints(bool considderDiagonals = false)
    {
        if (IsHoriztonal)
        {
            return GetHoriztonalPoints();
        }

        if (IsVertical)
        {
            return GetVerticalPoints();
        }

        return considderDiagonals
            ? GetDiagonalPoints()
            : Enumerable.Empty<(int, int)>();
    }

    private IEnumerable<(int, int)> GetHoriztonalPoints()
    {
        var x = Math.Min(X1, X2);

        while (x <= Math.Max(X1, X2))
        {
            yield return (x++, Y1);
        }
    }

    private IEnumerable<(int, int)> GetVerticalPoints()
    {
        var y = Math.Min(Y1, Y2);

        while (y <= Math.Max(Y1, Y2))
        {
            yield return (X1, y++);
        }
    }

    private IEnumerable<(int, int)> GetDiagonalPoints()
    {
        var x = X1;
        var y = Y1;

        while (x != X2)
        {
            while (y != Y2)
            {
                yield return (X1 > X2 ? x-- : x++, Y1 > Y2 ? y-- : y++);
            }
        }
        
        yield return (x, y);
    }
}