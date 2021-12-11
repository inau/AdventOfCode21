using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class DayXX : Common
    {
        public DayXX() : base("DayXX") {}

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
