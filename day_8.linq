<Query Kind="Program" />

#load ".\AdventOfCode"

//    0:      1:      2:      3:      4:      5:      6:      7:      8:      9:
//   aaaa    ....    aaaa    aaaa    ....    aaaa    aaaa    aaaa    aaaa    aaaa
//  b    c  .    c  .    c  .    c  b    c  b    .  b    .  .    c  b    c  b    c
//  b    c  .    c  .    c  .    c  b    c  b    .  b    .  .    c  b    c  b    c
//   ....    ....    dddd    dddd    dddd    dddd    dddd    ....    dddd    dddd
//  e    f  .    f  e    .  .    f  .    f  .    f  e    f  .    f  e    f  .    f
//  e    f  .    f  e    .  .    f  .    f  .    f  e    f  .    f  e    f  .    f
//   gggg    ....    gggg    gggg    ....    gggg    gggg    ....    gggg    gggg

void Main()
{
    var input = AdventOfCode.GetLines("day_8.input");
    var notes = input.Select(x => new Notes(x)).ToList();

    notes.Select(x => x.CountKnown()).Sum().Dump("Part One");
    notes.Select(x => x.ToInt()).Sum().Dump("Part Two");
}

public class Notes
{
    public HashSet<char>[] Patterns { get; set; }
    public HashSet<char>[] Output { get; set; }
    public IDictionary<char, HashSet<char>> Cipher { get; set; } = new Dictionary<char, HashSet<char>>();

    public Notes(string line)
    {
        var parts = line.Split('|');
        Patterns = parts[0]
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.ToHashSet())
            .ToArray();

        Output = parts[1]
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.ToHashSet())
            .ToArray();

        Cipher['1'] = Patterns.Where(x => x.Count == 2).Single();
        Cipher['4'] = Patterns.Where(x => x.Count == 4).Single();
        Cipher['7'] = Patterns.Where(x => x.Count == 3).Single();
        Cipher['8'] = Patterns.Where(x => x.Count == 7).Single();

        Cipher['3'] = Find(Cipher['7'], length: 5, matches: 3);
        Cipher['6'] = Find(Cipher['7'], length: 6, matches: 2);
        Cipher['5'] = Find(Cipher['6'], length: 5, matches: 5);
        Cipher['0'] = Find(Cipher['5'], length: 6, matches: 4);

        Cipher['2'] = Patterns.Where(x => x.Count == 5).Single(x => !Cipher.Values.Contains(x));
        Cipher['9'] = Patterns.Single(x => !Cipher.Values.Contains(x));
    }

    private HashSet<char> Find(HashSet<char> known, int length, int matches)
    {
        var possibilities = new List<HashSet<char>>();

        foreach (var item in Patterns.Where(x => x != known))
        {
            if (item.Intersect(known).Count() == matches && item.Count == length)
            {
                possibilities.Add(item);
            }
        }

        return possibilities.Single();
    }

    public int CountKnown()
    {
        int count = 0;

        foreach (var item in Output)
        {
            switch (item.Count)
            {
                case 2:
                case 4:
                case 3:
                case 7:
                    count++;
                    break;
            }
        }

        return count;
    }

    public int ToInt()
    {
        var a = "";
        var map = Cipher.ToDictionary(x => string.Concat(x.Value.OrderBy(x => x)), x => x.Key);

        foreach (var s in Output)
        {
            a += map[string.Concat(s.OrderBy(x => x))];
        }

        return Convert.ToInt32(a);
    }
}
