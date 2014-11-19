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

        public Section(SectionType type)
        {
            Type = type;
        }

        public void SetCells(List<Cell> cells)
        {
            Cells = cells;
        }

        /// <summary> Removes possible value from cells, if another cell into current section has the same value. Then if only one possible value left, assign it </summary>
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
                    cell.AssignValue(cell.PossibleValues.First());
                }
            }
        }

        /// <summary> Assigns a value to the cell, that has the only possible value among the cells into current section </summary>
        public void AssignSinglePossibility()
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                var possibleValueCells = Cells
                    .OfType<Cell>()
                    .Where(cell => cell.PossibleValues.Contains(i + 1));

                if (possibleValueCells.Count() == 1)
                {
                    possibleValueCells.First().AssignValue(i + 1);
                }
            }
        }
    }
}
