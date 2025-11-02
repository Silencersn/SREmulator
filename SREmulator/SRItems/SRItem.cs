namespace SREmulator.SRItems;

public enum SRItemRarity
{
    None = 0,
    Star1 = 1,
    Star2 = 2,
    Star3 = 3,
    Star4 = 4,
    Star5 = 5
}

public abstract class SRItem
{
    public abstract SRItemRarity Rarity { get; }
    public abstract string Name { get; }

    public override string ToString()
    {
        return Name;
    }
}
