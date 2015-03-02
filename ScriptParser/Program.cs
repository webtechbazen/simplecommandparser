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
                var command = new Command(Console.ReadLine());
                Console.WriteLine(String.Format("Text: \"{0}\"", command.Text));
                int argnum = 0;
                foreach (var argument in command.Arguments)
                {
                    Console.WriteLine(String.Format("Arg{0}: \"{1}\"", argnum++, argument));
                    var pat = new Parentheses(argument);
                    pat.Print();
                }
            }
        }
    }
}
