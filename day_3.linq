<Query Kind="Program" />

void Main()
{
	var lines = AdventOfCode.GetLines("day_3.input");

	var oxygen = "";
	var co2 = "";
	var len = lines.First().Length;

	IEnumerable<string> oxyLines = lines.ToArray();
	for (var i = 0; i < len; i++)
	{
		oxygen += GetBit(oxyLines, i, true, '1');
		oxyLines = oxyLines.Where(x => x.StartsWith(oxygen));
	}

	IEnumerable<string> co2Lines = lines.ToArray();
	for (var i = 0; i < len; i++)
	{
		co2 += GetBit(co2Lines, i, false, '0');
		co2Lines = co2Lines.Where(x => x.StartsWith(co2));
	}

	var o = Convert.ToInt32(oxygen, 2);
	var c = Convert.ToInt32(co2, 2);

	(o * c).Dump("Part Two");
}

private char GetBit(IEnumerable<string> lines, int position, bool descending, char onEqual)
{
	if (lines.Count() == 1)
	{
		return lines.First()[position];
	}

	var grouped = lines
		.Select(x => x[position])
		.GroupBy(x => x);

	grouped = descending
	? grouped.OrderByDescending(x => x.Count())
	: grouped.OrderBy(x => x.Count());


	if (grouped.First().Count() == grouped.Last().Count())
	{
		return onEqual;
	}

	return grouped.First().Key;
}
