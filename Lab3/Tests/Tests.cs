using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Tex2Html;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private static void DoNamedTest([CallerMemberName] string name = "")
        {
            if (!name.StartsWith("Test"))
            {
                Assert.Fail("Named test should only be called from test framework with Test[Name] pattern");
            }

            name = name.Remove(0, 4);
            const string tex2Html = @"..\..\Resources\Tex2Html";
            const string goldExtension = ".html";
            const string tempExtension = ".temp";
            const string texExtension = ".tex";
            var mathMlTemplate = Path.Combine(tex2Html, "MathMLTemplate" + goldExtension);
            var currentTexFile = Path.Combine(tex2Html, name, name + texExtension);
            var currentHtmlTempFile = Path.Combine(tex2Html, name, name + tempExtension);
            var currentHtmlGoldFile = Path.Combine(tex2Html, name, name + goldExtension);
            Assert.True(File.Exists(currentTexFile), $"Tex file does not exists: {currentTexFile}");
            var fileInfo = new FileInfo(currentTexFile);
            string html;
            using (var streamReader = fileInfo.OpenText())
            {
                var tex = streamReader.ReadToEnd();
                html = File.ReadAllText(mathMlTemplate).Replace("<!--    PLACE_CODE_HERE-->", TexToHtmlConverter.Convert(tex));
            }

            using (var tempStream = File.CreateText(currentHtmlTempFile))
            {
                tempStream.Write(html);
            }

            Assert.True(File.Exists(currentHtmlGoldFile), "No gold found");
            var gold = File.ReadAllText(currentHtmlGoldFile);
            Assert.AreEqual(gold, html, "Temp not equals gold");
            File.Delete(currentHtmlTempFile);
        }

        [Test]
        public void Test1()
        {
            DoNamedTest();
        }

        [Test]
        public void Test2()
        {
            DoNamedTest();
        }

        [Test]
        public void Test3()
        {
            DoNamedTest();
        }

        [Test]
        public void Test4()
        {
            DoNamedTest();
        }

        [Test]
        public void Test5()
        {
            DoNamedTest();
        }

        [Test]
        public void Test6()
        {
            DoNamedTest();
        }

        [Test]
        public void Test7()
        {
            DoNamedTest();
        }

        [Test]
        public void Test8()
        {
            DoNamedTest();
        }

        [Test]
        public void Test9()
        {
            DoNamedTest();
        }

        [Test]
        public void Test10()
        {
            DoNamedTest();
        }

        [Test]
        public void Test11()
        {
            DoNamedTest();
        }

        [Test]
        public void Test12()
        {
            DoNamedTest();
        }

        [Test]
        public void Test13()
        {
            DoNamedTest();
        }

        [Test]
        public void Test14()
        {
            DoNamedTest();
        }

        [Test]
        public void Test15()
        {
            DoNamedTest();
        }

        [Test]
        public void Test16()
        {
            DoNamedTest();
        }
    }
}