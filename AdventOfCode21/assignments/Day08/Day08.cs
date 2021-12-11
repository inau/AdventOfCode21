using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day08 : Common
    {
        public Day08() : base("Day08") {}

        enum segments { A='a',B='b',C='c',D='d',E='e',F='f',G='g' }
        class DisplayDigit
        {
            struct Info
            {
                public segments eSegment;
                public bool active;
            }
            Info?[] flags = new Info?[Enum.GetValues(typeof(segments)).Length];

            public void PrintDigit()
            {
                Console.WriteLine(GetDisplay());
            }
            public int RowSz => 6;
            public char[] GetDisplay()
            {
                char[] display = new char[(RowSz) * 8];
                for(int i = 0; i < display.Length; ++i)
                {
                    display[i] = '.';
                }
                for (int i = RowSz; i < display.Length; i+=(RowSz + 1))
                {
                    display[i] = '\n';
                }
                foreach (var info in flags)
                {
                    if(info != null)
                    {
                        updateDisplay(info, display);
                    }
                }

                return display;
            }

            void SetIndexes(in char[] display, IEnumerable<int> values, segments item)
            {
                foreach(int v in values)
                {
                    display[v] = (char)item;
                }
            }

            void updateDisplay(in Info? i, in char[] display)
            {
                if(display != null && i != null)
                {
                    switch(i.Value.eSegment)
                    {
                        case segments.A:
                            {
                                SetIndexes(display, new int[] { 1,2,3,4 }, segments.A);
                            }
                            break;
                        case segments.B:
                            {
                                SetIndexes(display, new int[] { 7, 14 }, segments.B);
                            }
                            break;
                        case segments.C:
                            {
                                SetIndexes(display, new int[] { 12, 19 }, segments.C);
                            }
                            break;
                        case segments.D:
                            {
                                SetIndexes(display, new int[] { 22,23,24,25 }, segments.D);
                            }
                            break;
                        case segments.E:
                            {
                                SetIndexes(display, new int[] { 28, 35 }, segments.E);
                            }
                            break;
                        case segments.F:
                            {
                                SetIndexes(display, new int[] { 33, 40 }, segments.F);
                            }
                            break;
                        case segments.G:
                            {
                                SetIndexes(display, new int[] { 43, 44,45,46 }, segments.G);
                            }
                            break;
                    }
                }
            }

            public DisplayDigit(string input)
            {
                int low = (int)segments.A;
                foreach (char inC in input.ToLower())
                {
                    var index = inC-low;
                    flags[index] = new Info
                    {
                        eSegment = (segments)inC,
                        active = true
                    };
                }
            }
        }

        void PrintDigits(List<DisplayDigit> digits)
        {
            int rowSz = digits.First().RowSz;

            for( int r = 0; r < digits.Count; r+=5)
            {
                var displaydata = digits.Skip(r).Take(5).Select(f => f.GetDisplay()).ToList();
                for( int d = 0; d < 8; ++d )
                {
                    int skipchar = d * (rowSz + 1);
                    string rowStr = "";
                    displaydata.ForEach(f =>
                    {
                        rowStr += new string(f.Skip(skipchar).Take(rowSz).ToArray()) + "   ";
                    });
                    Console.WriteLine(rowStr);
                }
                Console.WriteLine();
            }
        }

        public IEnumerable<int> GetInputSequence(bool simpleInput = false)
        {
            return InitReader<IEnumerable<int>>(simpleInput, (reader) =>
            {
                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    return line.Split(",").Select(x=>int.Parse(x)).ToList();
                }
                return null;
            });
        }

        public override string GetResult1()
        {
            List<int> sequence = GetInputSequence().ToList();

            //var digit = new DisplayDigit("abcdefg");
            //digit.PrintDigit();
            var digits = new List<DisplayDigit>
            {
                new DisplayDigit("abcdefg"),
                new DisplayDigit("abcdef"),
                new DisplayDigit("abcde"),
                new DisplayDigit("abcd"),
                new DisplayDigit("abc"),
                new DisplayDigit("ab"),
                new DisplayDigit("a"),
            };
            PrintDigits(digits);

            var result = "";
            
            return $"{result}";
        }


        public override string GetResult2()
        {
            List<int> sequence = GetInputSequence().ToList();

            var result = "";

            return $"{result}";
        }
    }
}
