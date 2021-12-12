<Query Kind="Program" />

#load ".\AdventOfCode"

void Main()
{
    var lines = AdventOfCode.GetLines("day_09", demo: false);
    var floorMap = new FloorMap(lines);

    var lowPoints = floorMap.GetLowPoints().ToList();
    GetScorePartOne(lowPoints).Dump("Part One");

    var basins = floorMap.GetBasinSizes(lowPoints);
    GetScoprePartTwo(basins).Dump("Part Two");
}

public int GetScorePartOne(IEnumerable<Point> lowPoints)
{
    return lowPoints
        .Select(x => x.Value + 1)
        .Sum();
}

public int GetScoprePartTwo(IEnumerable<int> basins)
{
    return basins
        .OrderByDescending(x => x)
        .Take(3)
        .Aggregate(1, (x, y) => x * y);
}

public struct Point
{
    public int X { get; }
    public int Y { get; }
    public int Value { get; }

    public Point(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;
    }
}

public class FloorMap
{
    public List<List<int>> Rows { get; } = new();

    public int Width => Rows.First().Count;
    public int Height => Rows.Count;

    public FloorMap(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            Rows.Add(line.Select(x => (int)x - '0').ToList());
        }
    }

    public IEnumerable<Point> GetLowPoints()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (IsLowPoint(x, y))
                    yield return new Point(x, y, Get(x, y));
            }
        }
    }

    public IEnumerable<int> GetBasinSizes(IEnumerable<Point> lowpoints)
    {
        foreach (var point in lowpoints)
        {
            HashSet<Point> basin = new HashSet<UserQuery.Point> { point };
            PopulateBasin(point, basin);

            yield return basin.Count;
        }
    }

    public bool IsLowPoint(int x, int y)
    {
        var val = Get(x, y);
        return GetAdjacent(x, y).All(x => x.Value > val);
    }

    public void PopulateBasin(Point point, HashSet<Point> basin)
    {
        foreach (var p in GetAdjacent(point.X, point.Y))
        {
            if (basin.Contains(p)) continue;
            if (p.Value > 8) continue;
            if (p.Value < point.Value) continue;

            basin.Add(p);
            PopulateBasin(p, basin);
        }
    }

    public IEnumerable<Point> GetAdjacent(int x, int y)
    {
        if (IsValid(x, y - 1))
            yield return new Point(x, y - 1, Get(x, y - 1));
        if (IsValid(x, y + 1))
            yield return new Point(x, y + 1, Get(x, y + 1));
        if (IsValid(x - 1, y))
            yield return new Point(x - 1, y, Get(x - 1, y));
        if (IsValid(x + 1, y))
            yield return new Point(x + 1, y, Get(x + 1, y));
    }

    public bool IsValid(int x, int y)
    {
        if (x < 0 || y < 0)
            return false;
        if (x >= Width || y >= Height)
            return false;
        return true;
    }

    public int Get(int x, int y)
    {
        return Rows[y][x];
    }
}
