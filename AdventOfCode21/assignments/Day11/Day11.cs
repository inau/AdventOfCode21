using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day11 : Common
    {
        public Day11() : base("Day11") {}

        public class OctopusMatrix
        {
            public long FlashCount { get; private set; }
            public bool AllFlashed { get; private set; }
            public int Columns { get; private set; }
            public int Rows { get; private set; }
            public int[]? Data { get; private set; }

            public OctopusMatrix(int col, int row, int[] backingData)
            {
                Columns = col;
                Rows = row;
                Data = backingData;

                if ((Columns * Rows) != backingData.Length)
                {
                    throw new ArgumentException("The col and row dimensions dont match up with the size of the backing data");
                }
            }

            private bool isColumnValid(int col) => col >= 0 && col < Columns;
            private bool isRowValid(int row) => row >= 0 && row < Rows;
            private int Indexer(int col, int row)
            {
                if (isColumnValid(col) && isRowValid(row))
                {
                    return row * Columns + col;
                }
                else throw new ArgumentOutOfRangeException("The (col,row) value exceeds the map");
            }
            private List<Tuple<int, int>> GetAdjacentIndexes(int col, int row)
            {
                List<Tuple<int, int>> adj = new List<Tuple<int, int>>();
                for (int c = -1; c < 2; c++)
                {
                    for (int r = -1; r < 2; r++)
                    {
                        var colIdx = col + c;
                        var rowIdx = row + r;

                        if ( (col != colIdx || row != rowIdx) && isColumnValid(colIdx) && isRowValid(rowIdx))
                        {
                            adj.Add(new Tuple<int,int>(colIdx, rowIdx));
                        }
                    }
                }
                return adj;
            }
            public void SimulateStep()
            {
                Queue<Tuple<int,int>> indexes = new Queue<Tuple<int, int>>();
                HashSet<int> flashed = new HashSet<int>();
                
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        indexes.Enqueue(new Tuple<int, int>(col, row));
                    }
                }


                while(indexes.Count > 0)
                {
                    var cell = indexes.Dequeue();
                    var idx = Indexer(cell.Item1, cell.Item2);
                    
                    Data[idx]++;
                    if (Data[idx] > 9 && !flashed.Contains(idx))
                    {
                        flashed.Add(idx);
                        foreach (var tpl in GetAdjacentIndexes(cell.Item1, cell.Item2))
                        {
                            var index = Indexer(tpl.Item1, tpl.Item2);
                            Data[index]++;
                            if (Data[index] > 9)
                            {
                                indexes.Enqueue(tpl);
                            }
                        }
                    } 
                }

                AllFlashed = flashed.Count == Data.Length;
                foreach(var fidx in flashed)
                {
                    Data[fidx] = 0;
                    FlashCount++;
                }
            }

            public void PrintMatrix(int iteration)
            {
                    Console.WriteLine($"Step * {iteration}");
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        Console.Write(Data[Indexer(col, row)]);
                    }
                    Console.WriteLine(" * ");
                }
            }
        }

        public OctopusMatrix GetInputSequence(bool simpleInput = false)
        {
            return InitReader(simpleInput, (reader) =>
            {
                var colLength = 0;
                var rowLength = 0;
                List<int> values = new List<int>();

                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    var tokens = line.ToCharArray().Select(x => int.Parse("" + x)).ToList();
                    if (colLength == 0) colLength = tokens.Count;
                    rowLength++;
                    values.AddRange(tokens);
                }
                return new OctopusMatrix(colLength, rowLength, values.ToArray());
            });
        }

        bool UseSimpleInput = false;
        public override string GetResult1()
        {
            var mxm = GetInputSequence(UseSimpleInput);

            if(PrintDebug) mxm.PrintMatrix(0);
            for(int round = 0; round < 100; round++)
            {
                mxm.SimulateStep();
                if (PrintDebug) mxm.PrintMatrix(1+round);
            }

            var result = mxm.FlashCount;
            
            return $"{result}";
        }


        public override string GetResult2()
        {
            var mxm = GetInputSequence(UseSimpleInput);

            if (PrintDebug) mxm.PrintMatrix(0);
            int round = 0;
            for (; !mxm.AllFlashed; round++)
            {
                mxm.SimulateStep();
                if (PrintDebug) mxm.PrintMatrix(1 + round);
            }
            var result = round;

            return $"{result}";
        }
    }
}
