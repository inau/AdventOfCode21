using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day01 : Common
    {
        public Day01() : base("Day01") {}

        public override string GetResult1()
        {
            int? previous = null;
            int? next = null;

            int gtCount = 0;
            using (var input = new StreamReader(GetInput()))
            {
                for(string? s = input.ReadLine(); s != null; s = input.ReadLine())
                {
                    next = int.Parse( s );

                    if( previous.HasValue )
                    {
                        gtCount += (next.Value > previous.Value) ? 1 : 0;
                    }

                    previous = next;
                }

            }
            
            return gtCount.ToString();
        }

        public override string GetResult2()
        {
            int[] values = new int[4];

            int req = 0;
            int gtCount = 0;
            using (var input = new StreamReader(GetInput()))
            {
                for (string? s = input.ReadLine(); s != null; s = input.ReadLine())
                {
                    int next = int.Parse(s);
                    if(req < 3)
                    {
                        values[1+req++] = next;
                    }
                    else 
                    {
                        // slide left
                        for (int i = 1; i < req + 1; ++i)
                        {
                            values[i - 1] = values[i];
                        }
                        values[3] = next;

                        int previousSum = values.Take(3).Sum();
                        int nextSum = values.Skip(1).Take(3).Sum();
                        gtCount += (nextSum > previousSum) ? 1 : 0; 
                    }
                }
            }

            return gtCount.ToString();
        }
    }
}
