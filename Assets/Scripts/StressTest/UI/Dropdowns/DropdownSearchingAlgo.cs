using System.Collections.Generic;

/// <summary>
/// Child class of DropdownEnum filled with SearchingAlgo enum
/// </summary>
public sealed class DropdownSearchingAlgo : DropdownEnum<SearchingAlgo>
{
    protected override void Init()
    {
        enumValues = new Dictionary<string, SearchingAlgo>()
        {
            { "BFS", SearchingAlgo.BFS },
            { "DFS (DLS)", SearchingAlgo.DFS },
            { "Djikstra", SearchingAlgo.DJIKSTRA },
            { "IDDFS", SearchingAlgo.IDDFS }
        };

        base.Init();
    }
}
