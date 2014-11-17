using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Util
    {
        public static int Length = 4;

        public static int Dimension
        {
            get { return Convert.ToInt32(Math.Sqrt(Length)); }
        }
    }
}
