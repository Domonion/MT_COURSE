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
            var kek = new Parser.Parser(new MyLexer(new StringReader(input)));
            var now = kek.Parse();
            Console.WriteLine(now.GetInternalRepresentation());
            return now.ToString();
        }

        public static void Check(string input)
        {
            Assert.True(input == TestParse(input));
        }
    }
}