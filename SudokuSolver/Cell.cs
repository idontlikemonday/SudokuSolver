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
        public Section Row { get; set; }
        public Section Column { get; set; }
        public Section Square { get; set; }

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

        public void SetSections(Section r, Section c, Section s)
        {
            Row = r;
            Column = c;
            Square = s;
        }

        /// <summary> Assigns value to cell, clears possible values and recalculates sections </summary>
        public void AssignValue(int value)
        {
            if (IsBlank)
            {
                Value = value;
                PossibleValues.Clear();

                Row.RecalculatePossibilities();
                Column.RecalculatePossibilities();
                Square.RecalculatePossibilities();

                Row.AssignSinglePossibility();
                Column.AssignSinglePossibility();
                Square.AssignSinglePossibility();
            }
        }
    }
}
