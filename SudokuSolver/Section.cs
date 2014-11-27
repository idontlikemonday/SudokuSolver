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

        public void RecalculatePossibilities()
        {
            RemoveImpossiblePossibilities();
            AssignIfSinglePossibilityLeft();
        }

        /// <summary> Removes possible value from cells, if another cell into current section has the same value </summary>
        public void RemoveImpossiblePossibilities()
        {
            foreach (var cell in Cells)
            {
                if (!cell.IsBlank)
                {
                    Cells.Where(cellToRecalc => cellToRecalc.PossibleValues.Count > 1).ToList()
                        .ForEach(cellToRecalc => cellToRecalc.PossibleValues.Remove(cell.Value));
                }
            }
        }

        /// <summary> Assigns value if only one possible variant left </summary>
        private void AssignIfSinglePossibilityLeft()
        {
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
                    .Where(cell => cell.IsBlank)
                    .Where(cell => cell.PossibleValues.Contains(i + 1));

                if (possibleValueCells.Count() == 1)
                {
                    possibleValueCells.First().AssignValue(i + 1);
                }
            }
        }

        public bool SectionVerified()
        {
            int allValues = Cells.OfType<Cell>()
                .Where(x => x.Value > 0)
                .Select(x => x.Value).ToArray()
                .Count();

            int distinctValues = Cells.OfType<Cell>()
                .Where(x => x.Value > 0)
                .Select(x => x.Value).ToArray()
                .Distinct()
                .Count();

            if (allValues == distinctValues)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
