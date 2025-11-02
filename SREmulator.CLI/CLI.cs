using SREmulator.Localizations;

namespace SREmulator.CLI;

public static partial class CLI
{
    public static void Execute(CLIArgs args)
    {
        if (args.Help)
        {
            Console.WriteLine(Help);
            return;
        }

        if (args.Language is not null)
        {
            if (args.Language.ToLower() is "null") args.Language = string.Empty;
            LocalizationManager.SetCulture(args.Language);
        }

        CLICommands.TryExecute(args);
    }
}
