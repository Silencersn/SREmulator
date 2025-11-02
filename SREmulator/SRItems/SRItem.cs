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

public abstract class SRItem : IEquatable<SRItem>
{
    public abstract SRItemRarity Rarity { get; }
    public abstract string Name { get; }
    public virtual int Id { get; } = 0;

    private string? _rarityStarsText = null;
    public string RarityStarsText => _rarityStarsText ??= new string('✦', (int)Rarity);

    public override bool Equals(object? obj)
    {
        if (obj is SRItem item) return Equals(item);
        return false;
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public bool Equals(SRItem? other)
    {
        return this == other;
    }

    public override string ToString()
    {
        return Name;
    }

    public static bool operator ==(SRItem? item1, SRItem? item2)
    {
        if (item1 is null) return item2 is null;
        if (item2 is null) return false;
        if (item1.Id is 0 && item2.Id is 0) return item1.Name == item2.Name;
        return item1.Id == item2.Id;
    }
    public static bool operator !=(SRItem? item1, SRItem? item2)
    {
        return !(item1 == item2);
    }
}
