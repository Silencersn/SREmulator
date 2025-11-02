using SREmulator.SRItems;

namespace SREmulator.CLI;

public sealed class CLIArgsSource
{
    private readonly string[] _args;
    private List<string>? _warnings;
    private int _current = -1;

    public List<string> Warnings => _warnings ??= [];

    public CLIArgsSource(string[] args)
    {
        _args = args;
    }

    public void Warning(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        Warnings.Add(message);
    }

    public string Next()
    {
        if (_current + 1 >= _args.Length) return string.Empty;
        return _args[++_current];
    }

    public int NextInt32(int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        string s = Next();
        if (!int.TryParse(s, out int value))
        {
            value = Math.Clamp(default, minValue, maxValue);
            Warning($"参数错误 '{s}' （参数无法解析为整数或超出范围）（已自动设置为 '{value}'）");
        }
        int ret = Math.Clamp(value, minValue, maxValue);
        if (ret != value) Warning($"参数错误 '{s}'（参数范围为 [{minValue}, {maxValue}]）（已自动设置为 '{ret}'）");
        return ret;
    }

    public ISRWarpResultItem? NextWarpResultItem()
    {
        return NextWarpResultItem<ISRWarpResultItem>();
    }

    public T? NextWarpResultItem<T>() where T : class, ISRWarpResultItem
    {
        string name = Next();
        var item = ISRWarpResultItem.GetItemByName(name) as T;
        if (item is null) Warning($"参数错误 '{name}' （无法识别的对象）");
        return item;
    }
}
