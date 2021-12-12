<Query Kind="Program">
  <Namespace>Xunit</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var lines = AdventOfCode.GetLines("day_11.input").ToList();
    RunSimulationPartOne(lines);
    RunSimulationPartTwo(lines);
}

public void RunSimulationPartOne(IEnumerable<string> lines)
{
    var grid = new OctopusGrid(lines);

    var count = 0;
    for (var i = 0; i < 100; i++)
    {
        count += grid.Step();
    }

    count.Dump("Part One");
}

public void RunSimulationPartTwo(IEnumerable<string> lines)
{
    var grid = new OctopusGrid(lines);

    var step = 1;
    while (true)
    {
        if (grid.Step() == grid.Octopuses.Count)
        {
            break;
        }
        step++;
    }

    step.Dump("Part One");
}

public class Octotpus
{
    public int Power { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    private bool _flashed = false;
    private OctopusGrid _grid;
    private HashSet<(int x, int y)> _adjacent = new();

    public Octotpus(int offset, int power, OctopusGrid grid)
    {
        X = offset % grid.Width;
        Y = offset / grid.Height;
        Power = power;
        _grid = grid;

        _adjacent.Add((X - 1, Y - 1));
        _adjacent.Add((X + 0, Y - 1));
        _adjacent.Add((X + 1, Y - 1));

        _adjacent.Add((X - 1, Y + 1));
        _adjacent.Add((X + 0, Y + 1));
        _adjacent.Add((X + 1, Y + 1));

        _adjacent.Add((X - 1, Y + 0));
        _adjacent.Add((X + 1, Y + 0));
    }

    public void Flip()
    {
        Charge();
    }

    public void Flop()
    {
        _flashed = false;
    }


    public void OnFlash(Octotpus flasher)
    {
        if (object.ReferenceEquals(flasher, this))
            return;

        if (_adjacent.Contains((flasher.X, flasher.Y)))
            Charge();
    }

    private void Charge()
    {
        if (_flashed)
            return;

        Power++;

        if (Power > 9)
        {
            Power = 0;
            _flashed = true;
            _grid.NotifyFlash(this);
        }
    }
}

public class OctopusGrid
{
    public List<Octotpus> Octopuses { get; } = new(100);

    public int Width { get; }
    public int Height => Width;
    private int _flashes = 0;

    public OctopusGrid(IEnumerable<string> lines, int width = 10)
    {
        Width = width;

        var count = 0;
        foreach (var line in lines)
        {
            foreach (char c in line)
            {
                Octopuses.Add(new Octotpus(count++, c - '0', this));
            }
        }
    }

    public int Step()
    {
        _flashes = 0;

        foreach (var o in Octopuses)
        {
            o.Flip();
        }

        foreach (var o in Octopuses)
        {
            o.Flop();
        }

        return _flashes;
    }

    public void NotifyFlash(Octotpus flasher)
    {
        _flashes++;
        foreach (var o in Octopuses)
        {
            o.OnFlash(flasher);
        }
    }

    public object ToDump()
    {
        var sb = new StringBuilder();
        sb.AppendLine(Environment.NewLine);
        var count = 0;
        foreach (var o in Octopuses)
        {
            if (count > 0 && count % Width == 00)
            {
                sb.Append(Environment.NewLine);
            }
            sb.Append(o.Power);

            count++;
        }
        sb.AppendLine(Environment.NewLine);
        return sb.ToString();
    }
}
