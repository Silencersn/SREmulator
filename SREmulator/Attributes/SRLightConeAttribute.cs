namespace SREmulator.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class SRLightConeAttribute : Attribute
{
    public string Key { get; }
    public int Rarity { get; }

    public SRLightConeAttribute(string key, int rarity)
    {
        Key = key;
        Rarity = rarity;
    }
}
