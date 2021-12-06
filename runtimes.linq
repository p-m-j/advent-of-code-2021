<Query Kind="Program">
  <NuGetReference>ConsoleTables</NuGetReference>
  <Namespace>ConsoleTables</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var table = new ConsoleTable("Day", "Both parts included", "Runtime", "Comments");
    var day = 1;
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds", string.Empty);
    table.AddRow(day++, false.ToEmoji(), "0.001 seconds", "Overwrote part 1, before I was planning on sharing");
    table.AddRow(day++, false.ToEmoji(), "0.007 seconds", "Overwrote part 1, before I was planning on sharing");
    table.AddRow(day++, true.ToEmoji(), "0.033 seconds", string.Empty);
    table.AddRow(day++, true.ToEmoji(), "0.046 seconds", "First attempt took ~8 seconds to run for part 1");
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds", "First attempt out of memory part 2, also very long runtime");
    table.ToMarkDownString().Dump();
}
