using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day05 : Common
    {
        public Day05() : base("Day05") {}

        public struct Line
        {
            public int x1 { get; private set; }
            public int y1 { get; private set; }
            public int x2 { get; private set; }
            public int y2 { get; private set; }
            public Line(int _x1, int _y1, int _x2, int _y2)
            {
                x1 = _x1;
                y1 = _y1;
                x2 = _x2;
                y2 = _y2;
            }
            public Line(int[] p1, int[] p2) : this(p1[0],p1[1],p2[0],p2[1])
            {
            }

            public bool IsHor => x1 == x2;
            public bool IsVer => y1 == y2;
        }

        public IEnumerable<Line> GetLines(bool onlyStraight = false, bool simpleInput = false)
        {
            return InitReader<IEnumerable<Line>>(simpleInput, (reader) =>
            {
                List<Line> lines = new List<Line>();
                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    var arguments = line.Split(" -> ").ToList();
                    var splitArgs = arguments.Select(x => x.Split(",")).ToList();
                    var x = splitArgs.Select(t => t.Select(y => int.Parse(y)).ToArray() )
                        .ToArray();

                    var itm = new Line(x[0], x[1]);

                    if (onlyStraight && itm.IsHor || itm.IsVer )
                    {
                        lines.Add(itm);
                    }
                    else lines.Add(itm);
                }
                return lines;
            });
        }

        public override string GetResult1()
        {
            var lines = GetLines(true);

            var maxX = lines.Select(line => line.x1).Concat(lines.Select(line => line.x2)).Max();
            var maxY = lines.Select(line => line.y1).Concat(lines.Select(line => line.y2)).Max();

            int[,] CostMxM = new int[maxX+1,maxY+1];

            foreach(var l in lines)
            {
                if(l.IsHor)
                {
                    var vMin = l.y1 < l.y2 ? l.y1 : l.y2;
                    var vMax = l.y1 <= l.y2 ? l.y2 : l.y1;
                    for (int y = vMin; y <= vMax; y++)
                    {
                        CostMxM[l.x1,y]++;
                    }
                }
                else if (l.IsVer)
                {
                    var vMin = l.x1 <  l.x2 ? l.x1 : l.x2;
                    var vMax = l.x1 <= l.x2 ? l.x2 : l.x1;
                    for (int x = vMin; x <= vMax; x++)
                    {
                        CostMxM[x, l.y1]++;
                    }
                }
            }

            int count = 0;
            foreach(var e in CostMxM)
            {
                count += e > 1 ? 1 : 0;
            }

            return $"{count}";
        }

        void PrintIntermitten(int[,] mxm)
        {
            for (int row = 0; row < mxm.GetLength(1); row++)
            {
                for (int col = 0; col < mxm.GetLength(0); col++)
                {
                    var v = mxm[col, row];
                    if( v > 1 )
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{v} ");
                }
                Console.WriteLine();
            }
                Console.WriteLine("-----------------");
        }

        public override string GetResult2()
        {
            var lines = GetLines(false, false);

            var maxX = lines.Select(line => line.x1).Concat(lines.Select(line => line.x2)).Max();
            var maxY = lines.Select(line => line.y1).Concat(lines.Select(line => line.y2)).Max();

            int[,] CostMxM = new int[maxX + 1, maxY + 1];

            foreach (var l in lines)
            {
                var yMin = l.y1 < l.y2 ? l.y1 : l.y2;
                var yMax = l.y1 < l.y2 ? l.y2 : l.y1;
                var xMin = l.x1 < l.x2 ? l.x1 : l.x2;
                var xMax = l.x1 < l.x2 ? l.x2 : l.x1;
                if(l.IsHor || l.IsVer)
                {
                    for (int y = yMin; y <= yMax; y++)
                    {
                        for (int x = xMin; x <= xMax; x++)
                        {
                            CostMxM[x, y]++;
                        }
                    }
                }
                else
                {
                    int dx = l.x2 - l.x1 > 0 ? 1 : -1;
                    int dy = l.y2 - l.y1 > 0 ? 1 : -1;
                    int x = l.x1;
                    int y = l.y1;
                    for (; y != (l.y2+dy) && x != (l.x2+dx); y+=dy, x+=dx)
                    {
                        CostMxM[x, y]++;
                    }
                }
            //    PrintIntermitten(CostMxM);
            }

            int count = 0;
            foreach (var e in CostMxM)
            {
                count += e > 1 ? 1 : 0;
            }
            


            return $"{count}";
        }
    }
}
