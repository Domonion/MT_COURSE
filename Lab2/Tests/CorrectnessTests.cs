using NUnit.Framework;
using static Tests.Util;

namespace Tests
{
    //TODO 3. annotate every test
    //TODO 4. somehow test structure - with direct build and vizualize
    //TODO 5. vizualize - string polskaya notation!
 
    //TODO 6. refactor shit
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
            Check("A");
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
            Check("A&b");
        }

        [Test]
        public void TestOrSimple()
        {
            //A`->|A
            Check("A|B");
        }

        [Test]
        public void TestXorSimple()
        {
            //S`->^S
            Check("A^B");
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
            Check("A&B&c");
        }

        [Test]
        public void TestAndMultiParenthesis()
        {
            //differ this order
            Check("(A&B)&C");
        }

        [Test]
        public void TestAndMultiParenthesis2()
        {
            //again
            Check("A&(B&C)");
        }

        [Test]
        public void TestOrAnd()
        {
            //relations between different operations
            Check("A|B&C");
        }

        [Test]
        public void TestAndOr()
        {
            //order
            Check("A&B|C");
        }

        [Test]
        public void TestOrAndParenthesis()
        {
            //differ in tree
            Check("A|(B&C)");
        }

        [Test]
        public void TestAndOrParenthesis()
        {
            //differ in prioritet
            Check("A&(B|C)");
        }

        [Test]
        public void TestParenthesisAndOr()
        {
            //again
            Check("(A&B)|C");
        }

        [Test]
        public void TestParenthesisOrAnd()
        {
            //again
            Check("(A|B)&C");
        }

        [Test]
        public void TestAll1()
        {
            //some full test
            Check("A^B|C&(!D)");
        }

        [Test]
        public void TestAll2()
        {
            //some full test
            Check("(!A)&B|C^D");
        }

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
            Assert.Catch(() => Check("A!"));
        }
    }
}