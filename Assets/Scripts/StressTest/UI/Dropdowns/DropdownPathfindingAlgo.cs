using System.Collections.Generic;

/// <summary>
/// Child class of DropdownEnum filled with PathfindingAlgo enum
/// </summary>
public sealed class DropdownPathfindingAlgo : DropdownEnum<PathfindingAlgo>
{
    protected override void Init()
    {
        enumValues = new Dictionary<string, PathfindingAlgo>()
        {
            { "A*", PathfindingAlgo.ASTAR },
            { "Djikstra", PathfindingAlgo.DJIKSTRA },
            { "Greedy", PathfindingAlgo.GREEDY },
            { "BFS", PathfindingAlgo.BFS },
            { "DFS", PathfindingAlgo.DFS },
            { "IDDFS", PathfindingAlgo.IDDFS }
        };

        base.Init();
    }
}
