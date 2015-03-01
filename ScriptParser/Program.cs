using System;

namespace ScriptParser
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var tokenizer = new Tokenizer(Console.ReadLine());
            var tokens = tokenizer.Tokenize();
            foreach (var token in tokens)
                Console.WriteLine(token.ToString());
        }
    }
}
