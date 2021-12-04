<Query Kind="Program" />

void Main()
{
    using (var reader = AdventOfCode.GetReader("day_1.input"))
    {
        GetScorePartOne(reader).Dump("Part One");
    }
    
    var items = AdventOfCode.GetLines("day_1.input").Select(int.Parse).ToArray();
    GetScorePartTwo(items).Dump("Part Two");
}

private int GetScorePartOne(StreamReader input)
{
    var count = 0;
    int last = int.Parse(input.ReadLine());
    while (int.TryParse(input.ReadLine(), out int next))
    {
        if (next > last)
            count++;
        last = next;
    }
    return count;
}

private int GetScorePartTwo(int[] items)
{
    var offset = 0;
    var count = 0;
    int? last = null;

    while (offset < items.Length - 2)
    {
        var next = new ArraySegment<int>(items, offset++, 3).Sum();
        
        if (last.HasValue)
        {
            if (next > last)
                count++;
        }        
        
        last = next;
    }
    
    return count;
}
