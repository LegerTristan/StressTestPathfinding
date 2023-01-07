using System.Collections.Generic;

/// <summary>
/// Child class of DropdownEnum filled with AlgoType enum
/// </summary>
public sealed class DropdownAlgoTypes : DropdownEnum<AlgoType>
{
    protected override void Init()
    {
        enumValues = new Dictionary<string, AlgoType>()
        {
            { "Search", AlgoType.SEARCH },
            { "Destination", AlgoType.DESTINATION }
        };

        base.Init();
    }
}
