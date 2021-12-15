<Query Kind="Program">
  <NuGetReference>ConsoleTables</NuGetReference>
  <Namespace>ConsoleTables</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var table = new ConsoleTable("Day", "Both parts included", "Runtime", "Comments", "Cheated");
    var day = 1;
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds", string.Empty, false.ToEmoji());
    table.AddRow(day++, false.ToEmoji(), "0.001 seconds", "Overwrote part 1, before I was planning on sharing", false.ToEmoji());
    table.AddRow(day++, false.ToEmoji(), "0.007 seconds", "Overwrote part 1, before I was planning on sharing", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.033 seconds", string.Empty, false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.046 seconds", "First attempt took ~8 seconds to run for part 1", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds", "First attempt out of memory part 2, also very long runtime", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.18 seconds", "Pretty good first try, could probably cleanup but I'm happy.", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.006 seconds", "That was a right PITA", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.010 seconds", "Rushed catching up, probably much better ways to solve.", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds", "Sounded more fun than it was.", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.035 seconds", "Rushed catching up, probably much better ways to solve.", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "~6.5 seconds", "Didn't enjoy this one.", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds", "Day 13 was really good fun!!!", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds", "I didn't learn my lesson and ran out of memory first attempt at part 2.", false.ToEmoji());
    table.AddRow(day++, true.ToEmoji(), "~0.4 seconds", "Googled AStar which was first thing that came to mind, eventually pretty much stole someone else implementation for Dijkstra.", true.ToEmoji());
    table.ToMarkDownString().Dump();
}

