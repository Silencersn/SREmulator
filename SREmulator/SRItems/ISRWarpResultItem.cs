namespace SREmulator.SRItems;

public interface ISRWarpResultItem
{
    public SRItemRarity Rarity { get; }
    public string Name { get; }

    public static ISRWarpResultItem? GetItemByName(string? name)
    {
        return (ISRWarpResultItem?)SRCharacters.GetItemByName(name) ?? SRLightCones.GetItemByName(name);
    }
}
