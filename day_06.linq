<Query Kind="Program" />

#load ".\AdventOfCode"

void Main()
{
    var input = AdventOfCode.GetLines("day_6.input").First();

    var fishes = input
        .Split(',')
        .Select(int.Parse)
        .ToArray();

    RunSimulation(fishes, 80).Dump("Part One");
    RunSimulation(fishes, 256).Dump("Part Two");
}

public long RunSimulation(int[] fishes, int days)
{
    var currentGeneration = CreateNewGeneration();
    var nextGeneration = CreateNewGeneration();

    foreach (var group in fishes.GroupBy(x => x))
    {
        currentGeneration[group.Key] = group.Count();
    }

    for (var i = 0; i < days; i++)
    {
        nextGeneration[8] = currentGeneration[0];
        nextGeneration[7] = currentGeneration[8];
        nextGeneration[6] = currentGeneration[7] + currentGeneration[0];
        nextGeneration[5] = currentGeneration[6];
        nextGeneration[4] = currentGeneration[5];
        nextGeneration[3] = currentGeneration[4];
        nextGeneration[2] = currentGeneration[3];
        nextGeneration[1] = currentGeneration[2];
        nextGeneration[0] = currentGeneration[1];

        currentGeneration = nextGeneration;
        nextGeneration = CreateNewGeneration();
    }

    return currentGeneration.Values.Sum();
}

private IDictionary<int, long> CreateNewGeneration()
{
    return Enumerable.Range(0, 9).ToDictionary(x => x, x => 0L);
}
