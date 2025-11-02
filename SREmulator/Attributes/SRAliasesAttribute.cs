namespace SREmulator.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class SRAliasesAttribute : Attribute
{
    public string[] Aliases { get; }

    public SRAliasesAttribute(params string[] aliases)
    {
        Aliases = aliases;
    }
}
