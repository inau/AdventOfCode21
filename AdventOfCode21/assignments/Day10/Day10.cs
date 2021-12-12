using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day10 : Common
    {
        public Day10() : base("Day10") {}

        public IEnumerable<string> GetInputSequence(bool simpleInput = false)
        {
            return InitReader<IEnumerable<string>>(simpleInput, (reader) =>
            {
                return ParseLines(reader).ToList();
            });
        }

        class SyntaxChecker
        {
            #region mappings
            readonly Dictionary<char, char> _TokenPairs = new Dictionary<char, char>
            {
                {'(',')'},
                {'[',']'},
                {'{','}'},
                {'<','>'},
            };
            readonly Dictionary<char, int> _MismatchScore = new Dictionary<char, int>
            {
                { ')', 3},
                { ']', 57},
                { '}', 1197},
                { '>', 25137},
            };
            readonly Dictionary<char, int> _AutoCompleteScore = new Dictionary<char, int>
            {
                { ')', 1},
                { ']', 2},
                { '}', 3},
                { '>', 4},
            };
            readonly Dictionary<char, ConsoleColor> _CharColor = new Dictionary<char, ConsoleColor>
            {
                { '(', ConsoleColor.Green },
                { ')', ConsoleColor.Green },
                { '[', ConsoleColor.Red },
                { ']', ConsoleColor.Red },
                { '{', ConsoleColor.DarkCyan },
                { '}', ConsoleColor.DarkCyan },
                { '<', ConsoleColor.DarkYellow },
                { '>', ConsoleColor.DarkYellow },
            };
            #endregion

            public int CheckLine(string syntax)
            {
                Stack<char> stack = new Stack<char>();
                foreach(char c in syntax)
                {
                    if( _TokenPairs.ContainsKey(c) ) // opening char
                    {
                        stack.Push(c);
                    }
                    else // closing character
                    {
                        var expectedChar = _TokenPairs[stack.Peek()];
                        if (expectedChar == c) // expected
                        {
                            stack.Pop();
                        }
                        else
                        {
                        //    Console.WriteLine($"expected '{ expectedChar }' but found '{c}'");
                            return _MismatchScore[c];
                        }
                    }
                }
                return 0;
            }

            struct entry { public int idx; public char val; };
            public long AutoComplete(string syntax)
            {
                Stack<entry> stack = new Stack<entry>();
                int i = 0;
                foreach (char c in syntax)
                {
                    if (_TokenPairs.ContainsKey(c)) // opening char
                    {
                        stack.Push(new entry { idx = i, val = c });
                    }
                    else // closing character
                    {
                        var expectedChar = _TokenPairs[stack.Peek().val];
                        if (expectedChar == c) // expected
                        {
                            stack.Pop();
                        }
                        else
                        {
                            Console.WriteLine($"!! expected '{ expectedChar }' but found '{c}'");
                            return _MismatchScore[c];
                        }
                    }
                    ++i;
                }
                long score = 0;

                var rhs = new string(stack.Select(x => _TokenPairs[x.val]).ToArray());
              
                if(PrintDebug)
                {

                    var indexes = stack.Select(x => x.idx).ToArray();
                    for(int j = 0; j < syntax.Length; j++)
                    {
                        if( indexes.Contains(j) ) Console.ForegroundColor = _CharColor[syntax[j]];
                        else Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(syntax[j]);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    //    var lhs = new string(stack.ToArray());
                    Console.Write( $" ");
                    for (int j = 0; j < rhs.Length; j++)
                    {
                        Console.ForegroundColor = _CharColor[rhs[j]];
                        Console.Write(rhs[j]);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }

                foreach (char c in rhs)
                {
                    score *= 5;
                    score += _AutoCompleteScore[c];
                }

                return score;
            }

        }

        bool UseSimpleInput = false;
        public override string GetResult1()
        {
            var sequence = GetInputSequence(UseSimpleInput).ToList();

            SyntaxChecker checker = new SyntaxChecker();
            List<int> scores = new List<int>();
            foreach(string str in sequence)
            {
                scores.Add( checker.CheckLine(str) );
            }

            var result = scores.Sum();
            
            return $"{result}";
        }


        public override string GetResult2()
        {
            PrintDebug = false;

            if(PrintDebug) Console.WriteLine();
            var sequence = GetInputSequence(UseSimpleInput).ToList();

            SyntaxChecker checker = new SyntaxChecker();
            List<long> scores = new List<long>();
            foreach (string str in sequence)
            {
                if(checker.CheckLine(str) == 0)
                {
                    scores.Add(checker.AutoComplete(str));
                }
            }

            var result = scores.OrderBy(x => x).Skip(scores.Count/2).Take(1).ToList().First();

            if (PrintDebug) Console.WriteLine();
            return $"{result}";
        }
    }
}
