<Query Kind="Program">
  <NuGetReference>ConsoleTables</NuGetReference>
  <Namespace>ConsoleTables</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var table = new ConsoleTable("Day", "Both Parts", "Runtime");
    var day = 1;
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds");
    table.AddRow(day++, false.ToEmoji(), "0.001 seconds");
    table.AddRow(day++, false.ToEmoji(), "0.007 seconds");
    table.AddRow(day++, true.ToEmoji(), "0.033 seconds");
    table.AddRow(day++, true.ToEmoji(), "0.046 seconds");
    table.AddRow(day++, true.ToEmoji(), "0.001 seconds");
    table.ToMarkDownString().Dump();
}

