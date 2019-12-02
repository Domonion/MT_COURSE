using System;
using System.IO;
using NUnit.Framework;
using Parser;

namespace Tests
{
    public static class Util
    {
        public static string TestParse(string input)
        {
            var kek = new Parser.Parser(new Lexer(new StringReader(input)));
            var now = kek.Parse();
            var str = now.GetInternalRepresentation().ToString();
            Console.WriteLine(str);
            return now.ToString();
        }

        public static void Check(string input)
        {
            Assert.True(input.Replace(" ", "") == TestParse(input));
        }
    }
}