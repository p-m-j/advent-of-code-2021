<Query Kind="Program" />

#load ".\AdventOfCode"

void Main()
{
    var lines = AdventOfCode.GetLines("day_2.input")
    .Select(x =>
    {
        var parts = x.Split(' ');
        return new
        {
            direction = parts[0],
            magnitude = int.Parse(parts[1])
        };
    });

    var depth = 0;
    var horizontal = 0;
    var aim = 0;

    foreach (var line in lines)
    {
        switch (line.direction)
        {
            case "forward":
                horizontal += line.magnitude;
                depth += aim * line.magnitude;
                break;
            case "down":
                aim += line.magnitude;
                break;
            case "up":
                aim -= line.magnitude;
                break;
        }
    }

    (depth * horizontal).Dump("Part Two");
}
