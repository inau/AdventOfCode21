using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day09 : Common
    {
        public Day09() : base("Day09") {}

        public class DataMatrix
        {
            public class Coordinate
            {
                public DataMatrix?  Parent { get; init; }
                public int          Col { get; init; }
                public int          Row { get; init; }
                public int          Value { get => Parent.Data[GetHashCode()]; }

                public override int GetHashCode() { return Parent.Indexer(Col, Row); }
                public override bool Equals(object obj) { return GetHashCode() == obj.GetHashCode(); }
            }
            public int Columns { get; private set; }
            public int Rows { get; private set; }
            public int[]? Data { get; private set; }

            public DataMatrix(int colLim, int rowLim, int[] backingData)
            {
                Columns = colLim;
                Rows = rowLim;
                Data = backingData;

                if( (Columns * Rows) != backingData.Length )
                {
                    throw new ArgumentException("The col and row dimensions dont match up with the size of the backing data");
                }
            }

            private int Indexer(int col, int row)
            {
                if(isColumnValid(col) && isRowValid(row))
                {
                    return row * Columns + col;
                }
                else throw new ArgumentOutOfRangeException("The (col,row) value exceeds the map");
            }
            private int GetValue(Coordinate c) => GetValue(c.Col, c.Row);
            private int GetValue(int col, int row)
            {
                return Data[Indexer(col, row)];
            }

            private List<Coordinate> GetAdjacentIndexes(Coordinate coord) => GetAdjacentIndexes(coord.Col, coord.Row);
            private List<Coordinate> GetAdjacentIndexes(int col, int row)
            {
                List<Coordinate> adj = new List<Coordinate>();
                for(int c = -1; c < 2; c+= 2)
                {
                    var colIdx = col + c;
                    if(isColumnValid(colIdx))
                    {
                        adj.Add(new Coordinate { Col = colIdx, Row = row, Parent = this });
                    }
                }
                for(int r = -1; r < 2; r+= 2)
                {
                    var rowIdx = row + r;
                    if (isRowValid(rowIdx))
                    {
                        adj.Add(new Coordinate { Col = col, Row = rowIdx, Parent = this });
                    }
                }
                return adj;
            }

            private bool isColumnValid(int col) => col >= 0 && col < Columns;
            private bool isRowValid(int row) => row >= 0 && row < Rows;

            public bool IsLocalMinima(int col, int row)
            {
                var cell = GetValue(col, row);

                var adj = GetAdjacentIndexes(col, row);
                bool isLocalMinima = true;
                foreach(var coord in adj)
                {
                    int cut = GetValue(coord);
                    isLocalMinima &= (cell < cut);
                }

                return isLocalMinima;
            }
            public int GetRiskLevel(int col, int row) => 1 + GetValue(col, row);

            public bool includeInBasin(Coordinate c) => includeInBasin(c.Col, c.Row);
            public bool includeInBasin(int col,int row)
            {
                int val = GetValue(col, row);
                bool include = val < 9;
            //    if(include) Console.Write($" {val}");
                return include;
            }

            public int bfsBasin(Coordinate coord)
            {
                Queue<Coordinate> queue = new Queue<Coordinate>();
                HashSet<Coordinate> visited = new HashSet<Coordinate>();

                queue.Enqueue(coord);
                //visited.Add(coord);

                List<Coordinate> list = new List<Coordinate>();

                int basinC0unt = 0;
                while(queue.Count > 0)
                {
                    var item = queue.Dequeue();

                    if( includeInBasin(item) && !visited.Contains(item))
                    {
                        visited.Add(item);
                        list.Add(item);
                        basinC0unt++;

                        var adj = GetAdjacentIndexes(item);
                        foreach(var _coord in adj)
                        {
                            if( !visited.Contains(_coord) )
                            {
                                queue.Enqueue(_coord);
                            }
                        }
                    }

                }
                return basinC0unt;
            }

            public int GetBasin(int col, int row)
            {
                //Console.WriteLine("\n--- B ---");
                return bfsBasin(new Coordinate { Col = col, Row = row, Parent = this });
            }
        }

        public DataMatrix GetInputSequence(bool simpleInput = false)
        {
            return InitReader<DataMatrix>(simpleInput, (reader) =>
            {
                var colLength = 0;
                var rowLength = 0;
                List<int> values = new List<int>();
                
                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    var tokens = line.ToCharArray().Select(x=>int.Parse(""+x)).ToList();
                    if(colLength == 0) colLength = tokens.Count;
                    rowLength++;
                    values.AddRange(tokens);
                }
                return new DataMatrix(colLength,rowLength,values.ToArray());
            });
        }

        bool UseSimpleInput = false;
        public override string GetResult1()
        {
            List<int> riskAssesment = new List<int>();

            var mxm = GetInputSequence(UseSimpleInput);

            for(int col = 0; col < mxm.Columns; ++col)
            {
                for(int row = 0; row < mxm.Rows; ++row)
                {
                    if( mxm.IsLocalMinima(col,row) )
                    {
                        riskAssesment.Add(mxm.GetRiskLevel(col,row));
                    }
                }
            }

            var result = riskAssesment.Sum().ToString();
            
            return $"{result}";
        }


        public override string GetResult2()
        {
            List<int> basins = new List<int>();

            var mxm = GetInputSequence(UseSimpleInput);

            for (int col = 0; col < mxm.Columns; ++col)
            {
                for (int row = 0; row < mxm.Rows; ++row)
                {
                    if (mxm.IsLocalMinima(col, row))
                    {
                        basins.Add(mxm.GetBasin(col, row));
                    }
                }
            }

            var result = basins.OrderByDescending(x => x)
                .Take(3).Aggregate((acc,v) => acc * v).ToString();

            return $"{result}";
        }
    }
}
