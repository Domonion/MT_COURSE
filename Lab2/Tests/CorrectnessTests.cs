using NUnit.Framework;
using static Tests.Util;

namespace Tests
{
    //TODO 4. somehow test structure - with direct build and vizualize
    //TODO 5. vizualize - string polskaya notation!
    [TestFixture]
    public class CorrectnessTests
    {
        [Test]
        public void TestVar()
        {
            //S ->AS`
            //S`->eps
            //A ->BA`
            //A`->eps
            //B ->CB`
            //B`->eps
            //C ->Var
            Check("a");
        }

        [Test]
        public void TestNotVar()
        {
            //C->!C
            Check("!a");
        }

        [Test]
        public void TestParenthesisVar()
        {
            //C->(S)
            Check("(a)");
        }

        [Test]
        public void TestAndSimple()
        {
            //B`->&B
            Check("a&b");
        }

        [Test]
        public void TestOrSimple()
        {
            //A`->|A
            Check("a|b");
        }

        [Test]
        public void TestXorSimple()
        {
            //S`->^S
            Check("a^b");
        }

        [Test]
        public void TestEmpty()
        {
            //should fails on lexer
            Assert.Catch(() => TestParse("{}"));
        }

        [Test]
        public void TestParenthesisNotVar()
        {
            Check("(!a)");
        }

        [Test]
        public void TestNotParenthesisVar()
        {
            Check("!(a)");
        }

        [Test]
        public void TestAndMulti()
        {
            //test order of same operators
            Check("a&b&c");
        }

        [Test]
        public void TestAndMultiParenthesis()
        {
            //differ this order
            Check("(a&b)&c");
        }

        [Test]
        public void TestAndMultiParenthesis2()
        {
            //again
            Check("a&(b&c)");
        }

        [Test]
        public void TestOrAnd()
        {
            //relations between different operations
            Check("a|b&c");
        }

        [Test]
        public void TestAndOr()
        {
            //order
            Check("a&b|c");
        }

        [Test]
        public void TestOrAndParenthesis()
        {
            //differ in tree
            Check("a|(b&c)");
        }

        [Test]
        public void TestAndOrParenthesis()
        {
            //differ in prioritet
            Check("a&(b|c)");
        }

        [Test]
        public void TestParenthesisAndOr()
        {
            //again
            Check("(a&b)|c");
        }

        [Test]
        public void TestParenthesisOrAnd()
        {
            //again
            Check("(a|b)&c");
        }

//        [Test]
//        public void TestAll1()
//        {
//            //some full test
//            Check("A^B|C&(!D)");
//        }
//
//        [Test]
//        public void TestAll2()
//        {
//            //some full test
//            Check("(!A)&B|C^D");
//        }

        [Test]
        public void TestFailS()
        {
            //check that parser fails correctly in S
            Assert.Catch(() => Check(")"));
        }

        [Test]
        public void TestFailsC()
        {
            //check fail C
            Assert.Catch(() => Check("(!)"));
        }

        [Test]
        public void TestFailB_SHTRIH()
        {
            //check fail B_SHTRIH
            Assert.Catch(() => Check("a!"));
        }
    }
}