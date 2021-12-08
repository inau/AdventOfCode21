using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal abstract class Common : IAoCEntity
    {
        public string Name { get; private set; }
        public Common(string DayName)
        {
            Name = DayName;
        }

        public FileStream GetInput()
        {
            return File.OpenRead($"assignments//{Name}//{Name}.txt");
        }

        public abstract string GetResult1();
        public abstract string GetResult2();
    }
}
