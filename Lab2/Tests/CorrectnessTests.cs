using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Parser;

namespace Tests
{
    [TestFixture]
    public class CorrectnessTests
    {
        private static void DoNamedTest([CallerMemberName] string name = "")
        {
            if (!name.StartsWith("Test"))
            {
                Assert.Fail("Named test should only be called from test framework with Test[Name] pattern");
            }

            name = name.Remove(0, 4);
            const string resourcesParser = @"..\..\Resources\Parser";
            const string goldExtension = ".gold";
            const string tempExtension = ".temp";
            const string parserExtension = ".txt";
            var currentInputFile = Path.Combine(resourcesParser, name, name + parserExtension);
            var currentTempFile = Path.Combine(resourcesParser, name, name + tempExtension);
            var currentGoldFile = Path.Combine(resourcesParser, name, name + goldExtension);
            Assert.True(File.Exists(currentInputFile), $"Input file does not exists: {currentInputFile}");
            var fileInfo = new FileInfo(currentInputFile);
            string text;
            using (var streamReader = fileInfo.OpenText())
            {
                var exp = new Parser.Parser(new Lexer(streamReader)).Parse();
                text = exp + "\n" + exp.GetInternalRepresentation();
            }

            using (var tempStream = File.CreateText(currentTempFile))
            {
                tempStream.Write(text);
            }

            Assert.True(File.Exists(currentGoldFile), "No gold found");
            var gold = File.ReadAllText(currentGoldFile);
            Assert.AreEqual(gold, text, "Temp not equals gold");
            File.Delete(currentTempFile);
        }

        [Test]
        public void TestVar()
        {
            //S ->AF
            //S`->eps
            //A ->BE
            //A`->eps
            //B ->CD
            //B`->eps
            //C ->Var
            DoNamedTest();
        }

        [Test]
        public void TestNotVar()
        {
            //C->!C;
            DoNamedTest();
        }

        [Test]
        public void TestParenthesisVar()
        {
            //C->(S)
            DoNamedTest();
        }

        [Test]
        public void TestAndSimple()
        {
            //D->&B
            DoNamedTest();
        }

        [Test]
        public void TestOrSimple()
        {
            //E->|A
            DoNamedTest();
        }

        [Test]
        public void TestXorSimple()
        {
            //F->^S
            DoNamedTest();
        }

        [Test]
        public void TestParenthesisNotVar()
        {
            //test C level order
            DoNamedTest();
        }

        [Test]
        public void TestNotParenthesisVar()
        {
            //test C level order
            DoNamedTest();
        }

        [Test]
        public void TestAndMulti()
        {
            //test order of same operators
            DoNamedTest();
        }

        [Test]
        public void TestAndMultiParenthesis()
        {
            //differ this order
            DoNamedTest();
        }

        [Test]
        public void TestAndMultiParenthesis2()
        {
            //again
            DoNamedTest();
        }

        [Test]
        public void TestOrAnd()
        {
            //relations between different operations
            DoNamedTest();
        }

        [Test]
        public void TestAndOr()
        {
            //order
            DoNamedTest();
        }

        [Test]
        public void TestOrAndParenthesis()
        {
            //differ in tree
            DoNamedTest();
        }

        [Test]
        public void TestAndOrParenthesis()
        {
            //differ in prioritet
            DoNamedTest();
        }

        [Test]
        public void TestParenthesisAndOr()
        {
            //again
            DoNamedTest();
        }

        [Test]
        public void TestParenthesisOrAnd()
        {
            //again
            DoNamedTest();
        }

        [Test]
        public void TestAll1()
        {
            //some full test
            DoNamedTest();
        }

        [Test]
        public void TestAll2()
        {
            //some full test
            DoNamedTest();
        }
    }
}