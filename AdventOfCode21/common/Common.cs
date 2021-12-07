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
        protected string name { get; private set; }
        public Common(string DayName)
        {
            name = DayName;
        }

        public FileStream GetInput()
        {
            return File.OpenRead($"assignments//{name}//{name}.txt");
        }

        public abstract string GetResult1();
        public abstract string GetResult2();
    }
}
