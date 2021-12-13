<Query Kind="Program">
  <Namespace>Xunit</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var lines = AdventOfCode.GetLines("day_13", false);
    var sheet = new Sheet(lines);

    Sheet.FoldAround(sheet.Points, sheet.Folds.First()).Count.Dump("Part One");
    PrintCode(sheet);
}

public void PrintCode(Sheet sheet)
{
    "!".Dump("Part Two");
    
    var set = sheet.Points;
    foreach (var fold in sheet.Folds)
    {
        set = Sheet.FoldAround(set, fold);
    }

    var width = set.Max(x => x.X + 1);
    var height = set.Max(x => x.Y + 1);

    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            Console.Write(set.Contains(new Point(x, y)) ? "#" : ".");
        }
        Console.WriteLine();
    }
}

public record Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public record Fold
{
    public string Axis { get; set; }
    public int Value { get; set; }

    public Fold(string axis, int value)
    {
        Axis = axis;
        Value = value;
    }
}

public class Sheet
{
    public HashSet<Point> Points { get; } = new();
    public List<Fold> Folds { get; } = new();

    public Sheet(IEnumerable<string> lines)
    {
        var coords = true;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                coords = false;
                continue;
            }

            if (coords)
            {
                var parts = line.Split(',');
                Points.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
            }
            else
            {
                var match = Regex.Match(line, @"(\w)=(\d+)");
                Folds.Add(new Fold(match.Groups[1].Value, int.Parse(match.Groups[2].Value)));
            }
        }
    }

    public static Point Reflect(Point p, Fold f)
    {
        switch (f.Axis)
        {
            case "x":
                return ReflectHorizontal(p, f.Value);
            case "y":
                return ReflectVertical(p, f.Value);
        }

        throw new ApplicationException("WAT?");
    }

    public static Point ReflectVertical(Point p, int y)
    {
        if (p.Y < y)
        {
            return p;
        }

        var distance = Math.Abs(y - p.Y);
        return new Point(p.X, y - distance);
    }

    public static Point ReflectHorizontal(Point p, int x)
    {
        if (p.X < x)
        {
            return p;
        }

        var distance = Math.Abs(x - p.X);
        return new Point(x - distance, p.Y);
    }

    public static HashSet<Point> FoldAround(HashSet<Point> points, Fold fold)
    {
        var results = new HashSet<Point>();
        foreach (var p in points)
        {
            results.Add(Reflect(p, fold));
        }
        return results;
    }
}



#region private::Tests

[Fact]
void Reflect_WithVerticalFold_WorksAsExpected()
{
    var result = Sheet.Reflect(new Point(0, 14), new Fold("y", 7));

    Assert.Equal(new Point(0, 0), result);
}

[Fact]
void Reflect_WithHorizontalFold_WorksAsExpected()
{
    var result = Sheet.Reflect(new Point(14, 0), new Fold("x", 7));

    Assert.Equal(new Point(0, 0), result);
}


#endregion
