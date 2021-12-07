<Query Kind="Program" />

#load ".\AdventOfCode"

void Main()
{
    var input = AdventOfCode.GetLines("day_7.input").First();

    var crabSubs = input
        .Split(',')
        .Select(int.Parse)
        .ToArray();

    RunSimulation(crabSubs, simpleCost: true).Dump("Part One");
    RunSimulation(crabSubs, simpleCost: false).Dump("Part Two");
}

public long RunSimulation(int[] crabSubs, bool simpleCost)
{
    var groupedCrabs = crabSubs
        .GroupBy(x => x);

    var crabsAtLocations = Enumerable.Range(0, groupedCrabs.Max(x => x.Key) + 1)
        .ToDictionary(x => x, x => 0);

    foreach (var group in groupedCrabs)
    {
        crabsAtLocations[group.Key] = group.Count();
    }

    var totals = crabSubs
        .GroupBy(x => x)
        .ToDictionary(x => x.Key, x => 0);

    foreach (var key in crabsAtLocations.Keys)
    {
    
        var cost = 0;
        foreach (var i in crabsAtLocations.Keys.Where(x => x != key))
        {
            var numCrabs = crabsAtLocations[i];
            var distance = Math.Abs(key - i);
            var costPerCrab = simpleCost ? distance : ComputeCost(distance);
            cost += costPerCrab * numCrabs;
        }

        totals[key] = cost;
    }
    
    return totals.Values.Min();
}

private Dictionary<int, int> costCache = new()
{
    [1] = 1,
};


private int ComputeCost(int steps)
{
    if (costCache.TryGetValue(steps, out int answer))
    {
        return answer;
    }

    costCache[steps] = ComputeCost(steps - 1) + steps;

    return costCache[steps];
}
