using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cell
    {
        public int Value { get; set; }
        public List<int> PossibleValues { get; set; }

        public bool IsBlank
        {
            get { return Value == 0; }
        }

        public Cell()
        {
            Value = 0;
            PossibleValues = new List<int> {1, 2, 3, 4};
        }

        public Cell(int value)
        {
            Value = value;
            PossibleValues = new List<int>();
        }
    }
}
