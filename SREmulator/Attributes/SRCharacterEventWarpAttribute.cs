namespace SREmulator.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public sealed class SRCharacterEventWarpAttribute : Attribute
{
    public string Key { get; }
    public int Index { get; }
    public int VerMajor { get; }
    public int VerMinor { get; }
    public string Up5 { get; }
    public string Up41 { get; }
    public string Up42 { get; }
    public string Up43 { get; }

    public SRCharacterEventWarpAttribute(string key, int index, int major, int minor, string up5, string up41, string up42, string up43)
    {
        Key = key;
        Index = index;
        VerMajor = major;
        VerMinor = minor;
        Up5 = up5;
        Up41 = up41;
        Up42 = up42;
        Up43 = up43;
    }
}
