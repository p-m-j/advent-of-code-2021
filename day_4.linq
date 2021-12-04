<Query Kind="Program" />

void Main()
{
    var lines = AdventOfCode.GetLines("day_4.input");

    var calls = lines
        .First()
        .Split(',')
        .Select(int.Parse);

    var boards = new List<Board>();

    foreach (var line in lines.Skip(1))
    {
        if (string.IsNullOrEmpty(line))
        {
            boards.Add(new Board());
            continue;
        }

        boards.Last().AddRow(line);
    }

    GetScorePartOne(calls, boards).Dump("Part 1");
    GetScorePartTwo(calls.GetEnumerator(), boards).Dump("Part 2");
}

private int GetScorePartOne(IEnumerable<int> calls, List<Board> boards)
{
    foreach (var call in calls)
    {
        foreach (var board in boards)
        {
            board.Mark(call);
            if (board.HasWon())
            {
                return call * board.SumUnmarked();
            }
        }
    }

    return -1;
}

private int GetScorePartTwo(IEnumerator<int> calls, List<Board> boards)
{
    calls.MoveNext();

    while (true)
    {
        foreach (var board in boards)
        {
            board.Mark(calls.Current);
        }

        boards = boards.Where(x => !x.HasWon()).ToList();

        if (boards.Count == 1)
        {
            var losingBoard = boards.Single();
            while (!losingBoard.HasWon())
            {
                calls.MoveNext();
                losingBoard.Mark(calls.Current);
            }

            return calls.Current * losingBoard.SumUnmarked();
        }

        calls.MoveNext();
    }
}

public class Cell
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int Value { get; set; }
    public bool Called { get; set; }

    public Cell(int offset, int value)
    {
        Value = value;
        Row = offset / 5;
        Col = offset % 5;
    }
}

public class Board
{
    public IDictionary<int, Cell> Cells = new Dictionary<int, Cell>();

    private int offset = 0;

    public void AddRow(string row)
    {
        var items = row
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(int.Parse);

        foreach (var item in items)
        {
            Cells.Add(item, new Cell(offset++, item));
        }
    }

    public void Mark(int value) 
    {
        if (Cells.TryGetValue(value, out var cell))
        {
            cell.Called = true;
        }
    }

    public bool HasWon()
    {
        if (Cells.Values.GroupBy(x => x.Col).Any(x => x.All(y => y.Called)))
            return true;
        if (Cells.Values.GroupBy(x => x.Row).Any(x => x.All(y => y.Called)))
            return true;

        return false;
    }

    public int SumUnmarked()
    {
        return Cells.Values.Where(x => !x.Called).Sum(x => x.Value);
    }
}
