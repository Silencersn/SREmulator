using System.Text;

namespace SREmulator.CLI;

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                if (args.Length is 0)
                {
                    Console.WriteLine(CLI.Help);
                    Console.ReadKey(true);
                    return;
                }

                CLI.Execute(CLIArgs.Parse(args));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //Console.ReadKey(true);
        }
    }
