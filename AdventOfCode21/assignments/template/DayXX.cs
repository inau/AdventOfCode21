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
            return InitReader(simpleInput, (reader) =>
            {
                IEnumerable<int> returnData = new List<int>();
                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    returnData = line?.Split(",").Select(x=>int.Parse(x)).ToList();
                }
                return returnData;
            });
        }

        bool UseSimpleInput = false;
        public override string GetResult1()
        {
            var sequence = GetInputSequence(UseSimpleInput).ToList();

            var result = "";
            
            return $"{result}";
        }


        public override string GetResult2()
        {
            var sequence = GetInputSequence(UseSimpleInput).ToList();

            var result = "";

            return $"{result}";
        }
    }
}
