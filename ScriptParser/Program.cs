using System;

namespace ScriptParser
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(">");
                var parser = new CommandParser(Console.ReadLine());
                var tokens = parser.Parse();
                foreach (var token in tokens)
                    Console.WriteLine(token.ToString());
            }
        }
    }
}
