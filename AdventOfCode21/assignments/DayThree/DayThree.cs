using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class DayThree : Common
    {
        public DayThree() : base("DayThree") {}

        struct FreqTable
        {
            public string   BitReprMCB         { get; private set; }
            public string   BitReprLCB         { get; private set; }
            public int      Samples         { get; private set; }
            public int[]    BitFrequency    { get; private set; }
            public FreqTable(int i, int[] d)
            {
                Samples = i;
                BitFrequency = d;
                string s = "";
                string sd = "";
                if (d != null)
                {
                    foreach(var v in d)
                    {
                        s += (v >= (Samples / 2)) ? '1' : '0';
                        sd += (v >= (Samples / 2)) ? '0' : '1';
                    }
                }
                BitReprMCB = s;
                BitReprLCB = sd;
            }
        }

        string Filter(int index, Func<int,int,char> search, List<string> set)
        {
            if(set.Count > 1)
            {
                int popCount = 0;
                foreach (var v in set)
                {
                    if (v[index] == '1') popCount++;
                }
                char nthPos = search(popCount, set.Count-popCount);
                var reduced = set.Where(s => s[index] == nthPos).ToList();
                return Filter(index + 1, search, reduced);
            }
            else 
                return set.First();
        }

        Tuple<string,string> GetFilteredValues()
        {

            List<string> values0 = new List<string>();
            List<string> values1 = new List<string>();

            using (var input = new StreamReader(GetInput()))
            {
                for (string? s = input.ReadLine(); s != null; s = input.ReadLine())
                {
                    if ( s[0] == '1' )
                    {
                        values0.Add(s);
                    }
                    else
                    {
                        values1.Add(s);
                    }
                }
            }

            Func<int,int,char,char> maxOrDefault = (o, z, d) =>
            {
                char c = d;
                if (o > z) c = '1';
                if (z > o) c = '0';
                return c;
            };
            Func<int, int, char, char> minOrDefault = (o, z, d) =>
            {
                char c = d;
                if (o < z) c = '1';
                if (z < o) c = '0';
                return c;
            };

            var s1 = "";
            var s2 = "";
            if(values0.Count > values1.Count)
            {
                s1 = Filter(1, (ones, zero) => maxOrDefault(ones,zero,'1'), values0);
                s2 = Filter(1, (ones, zero) => minOrDefault(ones, zero, '0'), values1);
            }
            else
            {
                s1 = Filter(1, (ones, zero) => minOrDefault(ones, zero, '0'), values1);
                s2 = Filter(1, (ones, zero) => maxOrDefault(ones, zero, '1'), values0);
            }

            return new Tuple<string, string>(s1,s2);
        }

        FreqTable GetFrequencyTable()
        {
            int rowCount = 0;
            int[] items = null;
            using (var input = new StreamReader(GetInput()))
            {
                for (string? s = input.ReadLine(); s != null; s = input.ReadLine())
                {
                    if(items == null)
                    {
                        items = new int[s.Length];
                    }

                    int i = 0;
                    foreach (var c in s)
                    {
                        items[i++] += c == '1' ? 1 : 0;
                    }
                    rowCount++;
                }
            }
            return new FreqTable(rowCount, items);
        }

        public override string GetResult1()
        {
            var ft = GetFrequencyTable();
            
            int gamma = Convert.ToUInt16(ft.BitReprMCB, 2);
            int epsilon = Convert.ToUInt16(ft.BitReprLCB, 2);
            
            return $"{gamma*epsilon}";
        }

        public override string GetResult2()
        {
            var tpl = GetFilteredValues();

            int gamma = Convert.ToUInt16(tpl.Item1, 2);
            int epsilon = Convert.ToUInt16(tpl.Item2, 2);

            return $"{gamma*epsilon}";
        }
    }
}
