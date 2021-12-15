<Query Kind="Program">
  <Namespace>Xunit</Namespace>
</Query>

#load ".\AdventOfCode"

void Main()
{
    var lines = AdventOfCode.GetLines("day_15.input");

    var graph = new Dictionary<(int, int), Node>();

    var y = 0;
    foreach (var line in lines)
    {
        var x = 0;
        foreach (var c in line)
        {
            graph.Add((x, y), new Node(x, y, c));
            x++;
        }
        y++;
    }

    var width = lines.First().Length;
    var height = lines.Count();

    Dijkstra(graph, graph[(width - 1, height - 1)]).Dump("Part One");

    var scaled = Scale(graph, 5);
    var scaledWidth = width * 5;
    var scaledHeight = height * 5;

    Dijkstra(scaled, scaled[(scaledWidth - 1, scaledHeight - 1)]).Dump("Part Two");
}

Dictionary<(int, int), Node> Scale(Dictionary<(int, int), Node> graph, int repeat)
{
    var width = graph.Values.Max(x => x.X) + 1;
    var height = graph.Values.Max(x => x.Y) + 1;
    var nodes = graph.Values.ToList();

    var result = new Dictionary<(int, int), Node>();
    foreach (var node in nodes)
    {
        for (var i = 0; i < repeat; i++)
        {
            for (var j = 0; j < repeat; j++)
            {
                var horizontal = new Node(node.X + width * i, node.Y + height * j, (node.Risk + i + j - 1) % 9 + 1);
                result.Add(horizontal.Key, horizontal);
            }
        }
    }

    return result;
}


public static IEnumerable<Node> GetAdjacent(Node node, Dictionary<(int, int), Node> graph)
{
    Node adjacent;
    if (graph.TryGetValue((node.X - 1, node.Y + 0), out adjacent))
        yield return adjacent;
    if (graph.TryGetValue((node.X + 1, node.Y + 0), out adjacent))
        yield return adjacent;
    if (graph.TryGetValue((node.X + 0, node.Y - 1), out adjacent))
        yield return adjacent;
    if (graph.TryGetValue((node.X + 0, node.Y + 1), out adjacent))
        yield return adjacent;
}


int Dijkstra(Dictionary<(int, int), Node> graph, Node target)
{
    var q = new PriorityQueue<Node, int>();
    graph[(0, 0)].Distance = 0;

    q.Enqueue(graph[(0, 0)], 0);

    while (q.Count > 0)
    {
        var u = q.Dequeue();
        if (u.Visited)
        {
            continue;
        }

        u.Visited = true;

        if (u == target)
        {
            return target.Distance;
        }

        foreach (var v in GetAdjacent(u, graph))
        {
            var alt = u.Distance + v.Risk;
            if (alt < v.Distance)
            {
                v.Distance = alt;
            }

            if (v.Distance < int.MaxValue)
            {
                q.Enqueue(v, v.Distance);
            }
        }
    }
    return target.Distance;
}

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Risk { get; set; }
    public int Distance { get; set; } = int.MaxValue;
    public bool Visited { get; set; } = false;

    public (int, int) Key => (X, Y);

    public Node(int x, int y, char risk)
    {
        X = x;
        Y = y;
        Risk = risk - '0';
    }

    public Node(int x, int y, int risk)
    {
        X = x;
        Y = y;
        Risk = risk;
    }
}