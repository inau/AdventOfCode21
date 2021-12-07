using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.common
{
    internal interface IAoCEntity
    {
        FileStream GetInput();
        string GetResult1();
        string GetResult2();
    }
}
