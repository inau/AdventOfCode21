using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day4 : Common
    {
        public Day4() : base("Day4") {}

        class BingoPlate
        {
            struct ActionBinding
            {
                public int Col { get; private set; }
                public int Row { get; private set; }
                public ActionBinding(int x,int y)
                {
                    Col = x;
                    Row = y;
                }
            }
            private Dictionary<int, ActionBinding> _Num2Cell = new Dictionary<int, ActionBinding>();
            private int[] _RowCount = new int[5];
            private int[] _ColCount = new int[5];
            int totalSum = 0;
            int remainder = 0;
           
            public BingoPlate(int[] boardValues)
            {
                int i = 0;
                for(int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        int v = boardValues[i++];
                        _Num2Cell[v] = new ActionBinding(row, col);
                        totalSum += v;
                    }
                }
                remainder = totalSum;
            }

            void Apply(ActionBinding bind)
            {
                _RowCount[bind.Row]++;
                _ColCount[bind.Col]++;
            }

            int bingoCount = 0;
            public int isBingoCount(int number)
            {
                bingoCount += isBingo(number) ? 1 : 0;
                return bingoCount;
            }

            public bool isBingo(int number)
            {
                if( _Num2Cell.ContainsKey(number) )
                {
                    Apply(_Num2Cell[number]);
                    _Num2Cell.Remove(number);
                    remainder -= number;
                    return _RowCount.Any(c => c == 5) || _ColCount.Any(c => c == 5); ;
                }
                return false;
            }

            public int GetRemainder() => remainder;
        }

        IEnumerable<BingoPlate> GetGameboards(StreamReader reader)
        {
            var list = new List<BingoPlate>();
            var numberSequence = new List<int>();
            for (string? s = reader.ReadLine(); s != null; s = reader.ReadLine())
            {
                if( (s == "" || s == null) && numberSequence.Count > 23)
                {
                    var seq = numberSequence.ToArray();
                    var bingo = new BingoPlate(seq);
                    list.Add(bingo);
                    numberSequence = new List<int>();
                }
                else
                {
                    var rowNum = s.Split(" ").Where(s => s != "").Select(c => int.Parse(c));
                    numberSequence.AddRange(rowNum);
                }
            }

            return list;
        }

        IEnumerable<int> GetInputNumbers(StreamReader reader)
        {
            string numberStr = reader.ReadLine() ?? "";
            for (string? s = reader.ReadLine(); s != null && s != ""; s = reader.ReadLine())
            {
                // read newline
            }
            
            return numberStr.Split(',').Select(s => int.Parse(s)).ToList() ?? new List<int>();
        }

        Tuple<IEnumerable<int>, IEnumerable<BingoPlate>> GetTaskInput()
        {
            using (var input = new StreamReader(GetInput()))
            {
                var inputSequence = GetInputNumbers(input);
                var boards = GetGameboards(input);

                return new Tuple<IEnumerable<int>, IEnumerable<BingoPlate>>(inputSequence, boards);
            }
        }

        public override string GetResult1()
        {
            var data = GetTaskInput();
            string result = "n/a";
            foreach(var nb in data.Item1)
            {
                foreach(var plate in data.Item2)
                {
                    if( plate.isBingo(nb) )
                    {
                        return $"{nb * plate.GetRemainder()}";
                    }
                }
            }

            return result;
        }

        public override string GetResult2()
        {
            var data = GetTaskInput();
            Dictionary<int,string> result = new Dictionary<int, string>();
            var boards = data.Item2.Select((board,index) => (board,index)).ToList();
            var limit = boards.Count;
            foreach (var nb in data.Item1)
            {
                foreach (var item in boards)
                {
                    if (item.board.isBingoCount(nb) == 1 && !result.ContainsKey(item.index))
                    {
                        result.Add(item.index, $"{nb * item.board.GetRemainder()}" );
                    }
                    if(result.Count == limit)
                    {
                        return result[item.index];
                    }
                }
            }

            return "";
        }
    }
}
