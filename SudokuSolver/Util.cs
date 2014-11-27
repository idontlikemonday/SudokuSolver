using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Util
    {
        public static int Length = 9;

        public static int Dimension
        {
            get { return Convert.ToInt32(Math.Sqrt(Length)); }
        }

        public static int Sum
        {
            get
            {
                int result = 0;
                for (int i = 1; i <= Length; i++)
                {
                    result += i;
                }
                return result;
            }
        }
    }
}
