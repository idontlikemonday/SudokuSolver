using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Section
    {
        public SectionType Type { get; set; }

        public List<Cell> Cells { get; set; }

        public Section()
        {
            //Cells = new List<Cell>();
            //for (int i = 0; i < Util.Length; i++)
            //{
            //    Cells.Add(new Cell());
            //}
        }

        public Section(SectionType type)
        {
            Type = type;
        }

        public void AssignCells(List<Cell> cells)
        {
            Cells = cells;
        }

        public void RecalculatePossibilities()
        {
            foreach (var cell in Cells)
            {
                if (!cell.IsBlank)
                {
                    Cells.ForEach(cellToRecalc => cellToRecalc.PossibleValues.Remove(cell.Value));
                }
            }
            foreach (var cell in Cells)
            {
                if (cell.PossibleValues.Count == 1)
                {
                    cell.Value = cell.PossibleValues.First();
                    cell.PossibleValues.Remove(cell.Value);
                }
            }
        }
    }
}
