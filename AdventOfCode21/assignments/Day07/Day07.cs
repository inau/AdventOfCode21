using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day07 : Common
    {
        public Day07() : base("Day07") {}

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

        int GetDist(int a, int b)
        {
            return (a < b) ? b - a : a - b;
        }
        
        Dictionary<int,long> CreateDistanceTable(List<int> positions, Func<int,int> fn) 
        {
            var distanceTable = new Dictionary<int,long>();

            for(int i = 4; i < positions.Count; i++)
            {
                int fuelSum = 0;
                int pos1 = positions[i];
                for (int j = 0; j < positions.Count; j++)
                {
                    int pos2 = positions[j];
                    int d = GetDist(pos1, pos2);
                    int fuel = fn(d);
                    fuelSum += fuel;
                }
                distanceTable.Add(i, fuelSum);
            }

            return distanceTable;
        }
        
        bool isRight(Dictionary<int,int> result)
        {
            return 
                result[1] == 41 &&
                result[2] == 37 && 
                result[10] == 71;
        }


        public override string GetResult1()
        {
            List<int> sequence = GetInputSequence().ToList();

            var table = CreateDistanceTable(sequence, x => x);

           // if( !isRight(table) )
           // {
           //     Console.WriteLine("NEIN NEIN 9 9 9");
           //     return "";
           // }

            var best = table.Min(v => v.Value);
            
            return $"{best}";
        }


        public override string GetResult2()
        {
            List<int> sequence = GetInputSequence().ToList();

            Func<int, int> distanceCalc = (x) =>
             {
                 int r = 0;
                 for (; x > 0; --x)
                 {
                     r += x;
                 }
                 return r;
             };

            var table = CreateDistanceTable(sequence,distanceCalc);
            var best = table.OrderBy(v => v.Value).Take(10).ToList();

            return $"{best.First().Value}";
        }
    }
}
