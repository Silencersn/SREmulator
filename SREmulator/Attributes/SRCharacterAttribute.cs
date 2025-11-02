namespace SREmulator.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class SRCharacterAttribute : Attribute
{
    public string Key { get; }
    public int Rarity { get; }

    public SRCharacterAttribute(string key, int rarity)
    {
        Key = key;
        Rarity = rarity;
    }
}
