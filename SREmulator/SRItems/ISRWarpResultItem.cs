namespace SREmulator.SRItems;

public interface ISRWarpResultItem : IEquatable<ISRWarpResultItem>
{
    public SRItemRarity Rarity { get; }
    public string Name { get; }
    public int Id { get; }

    bool IEquatable<ISRWarpResultItem>.Equals(ISRWarpResultItem? other)
    {
        return Id == other?.Id;
    }

    public static ISRWarpResultItem? GetItemByName(string? name)
    {
        return (ISRWarpResultItem?)SRCharacters.GetItemByName(name) ?? SRLightCones.GetItemByName(name);
    }
}
