using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserGenerator
{
    internal static class StringEx
    {
        public static int FastHash(this string s)
        {
            var res = 0;
            var p = 13;
            foreach (var ch in s)
                unchecked
                {
                    res *= p;
                    res += ch.GetHashCode();
                }

            return res;
        }
    }

    internal interface IAtom
    {
        bool IsRule { get; }
        string Name { get; }
        IAtom Convert(HashSet<Rule> context);
    }

    internal class Body
    {
        public List<IAtom> Atoms { get; }
        public string Action { get; }

        public Body(List<IAtom> atoms, string action)
        {
            Action = action;
            Atoms = atoms;
        }
    }

    internal class Rule : IAtom
    {
        public static bool Check(string name, HashSet<Rule> rules, out Rule res)
        {
            var ruleFromName = new Rule(name);
            return rules.TryGetValue(ruleFromName, out res);
        }

        private Rule(string name)
        {
            Name = name;
        }

        public IAtom Convert(HashSet<Rule> context)
        {
            return this;
        }

        public string AtomRuleAction { get; }

        public bool IsRule { get; } = true;
        public string Name { get; }
        public List<Body> Bodies { get; }
        public string Attributes { get; }

        public string InheritAttributes { get; }

        public override int GetHashCode()
        {
            return Name.FastHash();
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj?.GetHashCode() && Name == (obj as IAtom)?.Name;
        }

        public Rule(string name, List<Body> bodies, string attributes, string inheritAttributes, string atomRuleAction)
        {
            Name = name;
            Bodies = bodies;
            Attributes = attributes;
            InheritAttributes = inheritAttributes;
            AtomRuleAction = atomRuleAction;
        }
    }

    internal class Token : IAtom
    {
        public bool IsRule { get; } = false;
        public string Name { get; }

        public override int GetHashCode()
        {
            return Name.FastHash();
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj?.GetHashCode() && Name == (obj as IAtom)?.Name;
        }

        public IAtom Convert(HashSet<Rule> context)
        {
            return this;
        }

        public Token(string name)
        {
            Name = name;
        }
    }

    internal class Unknown : IAtom
    {
        public Unknown(string name, string atomRuleAction)
        {
            Name = name;
            IsRule = name.ToLower() == name;
            AtomRuleAction = atomRuleAction;
        }

        public bool IsRule { get; }
        public string Name { get; }

        public string AtomRuleAction { get; }

        public override int GetHashCode()
        {
            return Name.FastHash();
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj?.GetHashCode() && Name == (obj as IAtom)?.Name;
        }

        public IAtom Convert(HashSet<Rule> context)
        {
            if (!IsRule) return new Token(Name);
            if (Rule.Check(Name, context, out var rule))
            {
                return new Rule(rule.Name, rule.Bodies, rule.Attributes, rule.InheritAttributes, AtomRuleAction);
                //return rule;
            }

            throw new Exception("there is no rule " + Name + " in the given context.");
        }
    }

    internal class ParserVisitor : ParserBaseVisitor<List<Body>>
    {
        public readonly HashSet<Rule> Rules = new HashSet<Rule>();
        public Rule Start;

        protected override List<Body> AggregateResult(List<Body> aggregate, List<Body> nextResult)
        {
            if (nextResult != null)
                aggregate?.AddRange(nextResult);
            return aggregate ?? nextResult;
        }

        public override List<Body> VisitRule1(ParserParser.Rule1Context context)
        {
            var currentRule = new Rule(context.RULE_NAME().GetText(), VisitRule_body(context.rule_body()), context.attributes()?.GetText(), context.inher_attributes()?.GetText(), null);
            if (Start == null)
            {
                Start = currentRule;
            }
            Rules.Add(currentRule);
            return null;
        }
        
        public override List<Body> VisitRule_body(ParserParser.Rule_bodyContext context)
        {
            var bodies = base.VisitRule_body(context);
            var atoms = context.atom().ToList();
            var atomsList = atoms.Select(atom =>
            {
                if (atom.RULE_NAME() != null)
                {
                    return new Unknown(atom.RULE_NAME().GetText(), atom.ACTION()?.GetText());
                }
                return new Unknown(atom.TOKEN_NAME().GetText(), null);
            }).Cast<IAtom>().ToList();
            var action = context.ACTION()?.GetText();
            return AggregateResult(new List<Body> {new Body(atomsList, action)}, bodies);
        }
    }
}