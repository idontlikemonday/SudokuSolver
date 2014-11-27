using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Field
    {
        public Cell[,] Cells { get; set; }
        private List<Section> _rows;
        private List<Section> _columns;
        private List<Section> _squares;

        public int FilledCellsCount
        {
            get
            {
                return Cells.OfType<Cell>()
                    .Count(cell => !cell.IsBlank);
            }
        }

        public List<Cell> BlankCells
        {
            get
            {
                return Cells.OfType<Cell>()
                    .Where(x => x.IsBlank)
                    .ToList();
            }
        }

        public bool IsFinished
        {
            get
            {
                return Cells.OfType<Cell>()
                    .All(cell => !cell.IsBlank);
            }
        }

        public Field()
        {
            Cells = new Cell[Util.Length, Util.Length];

            _rows = new List<Section>();
            _columns = new List<Section>();
            _squares = new List<Section>();

            for (int i = 0; i < Util.Length; i++)
            {
                _rows.Add(new Section(SectionType.Row));
                _columns.Add(new Section(SectionType.Column));
                _squares.Add(new Section(SectionType.Square));
            }
        }

        public Field(int[,] values)
            : this()
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (values[i, j] > 0
                        && values[i, j] <= Util.Length)
                    {
                        Cells[i, j] = new Cell(values[i, j]);
                    }
                    else
                    {
                        Cells[i, j] = new Cell();
                    }
                }
            }

            var rowCells = new List<Cell>();
            var columnCells = new List<Cell>();
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    rowCells.Add(Cells[i, j]);
                    columnCells.Add(Cells[j, i]);
                }
                _rows[i].SetCells(rowCells);
                _columns[i].SetCells(columnCells);
                rowCells = new List<Cell>();
                columnCells = new List<Cell>();
            }

            var squareCells = new List<Cell>();
            int reg = 0;
            for (int i = 0; i < Cells.GetLength(0); i += Util.Dimension)
            {
                for (int j = 0; j < Cells.GetLength(1); j += Util.Dimension)
                {
                    for (int k = i; k < i + Util.Dimension; k++)
                    {
                        for (int l = j; l < j + Util.Dimension; l++)
                        {
                            squareCells.Add(Cells[k, l]);
                        }
                    }

                    _squares[reg].SetCells(squareCells);
                    reg++;
                    squareCells = new List<Cell>();
                }
            }

            reg = 0;
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if ((j % Util.Dimension == 0) && (j != 0))
                    {
                        reg++;
                    }

                    Cells[i, j].SetSections(_rows[i], _columns[j], _squares[reg]);
                }
                if (i < 2)
                {
                    reg = 0;
                }
                else if (i < 5)
                {
                    reg = 3;
                }
                else if (i < 8)
                {
                    reg = 6;
                }
            }
        }

        public Field Copy()
        {
            // TODO: Can be refactored
            int[,] copiedCellValues = new int[Util.Length, Util.Length];
            for (int i = 0; i < this.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < this.Cells.GetLength(1); j++)
                {
                    copiedCellValues[i, j] = this.Cells[i, j].Value;
                }
            }

            Field result = new Field(copiedCellValues);
            result.UpdatePossibilities();
            return result;
        }

        // TODO: Refactor following 3 methods using interface
        public void UpdatePossibilities()
        {
            _rows.ForEach(r => r.RemoveImpossiblePossibilities());
            _columns.ForEach(c => c.RemoveImpossiblePossibilities());
            _squares.ForEach(s => s.RemoveImpossiblePossibilities());
        }

        public void RecalculateSections()
        {
            _rows.ForEach(r => r.RecalculatePossibilities());
            _columns.ForEach(c => c.RecalculatePossibilities());
            _squares.ForEach(s => s.RecalculatePossibilities());
        }

        public void RecalculateUsingSinglePossibility()
        {
            _rows.ForEach(r => r.AssignSinglePossibility());
            _columns.ForEach(c => c.AssignSinglePossibility());
            _squares.ForEach(s => s.AssignSinglePossibility());
        }

        public bool IsSolved()
        {
            int squareSum = 0;
            int columnSum = 0;
            int rowSum = 0;

            for (int i = 0; i < Util.Length; i++)
            {
                rowSum = _rows[i].Cells.Sum(x => x.Value);
                columnSum = _columns[i].Cells.Sum(x => x.Value);
                squareSum = _squares[i].Cells.Sum(x => x.Value);

                if ((squareSum != Util.Sum) || (columnSum != Util.Sum) || (rowSum != Util.Sum))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CurrentlyVerified()
        {
            for (int i = 0; i < Util.Length; i++)
            {
                if (!_rows[i].SectionVerified())
                {
                    return false;
                }
                else if (!_columns[i].SectionVerified())
                {
                    return false;
                }
                else if (!_squares[i].SectionVerified())
                {
                    return false;
                }
            }
            return true;
        }

        //public void Print()
        //{
        //    Console.WriteLine();
        //    int row = 0;

        //    for (int i = 0; i < Util.Length; i++)
        //    {
        //        for (int z = 0; z < Util.Dimension; z++)
        //        {
        //            for (int j = 0; j < Util.Length; j++)
        //            {
        //                if (j % Util.Dimension == 0 && j != 0)
        //                {
        //                    Console.Write(" █ ");
        //                }
        //                else if (j != 0)
        //                {
        //                    Console.Write(" | ");
        //                }
        //                for (int l = row; l < row + Util.Dimension; l++)
        //                {
        //                    if (Cells[i, j].IsBlank)
        //                    {
        //                        Console.Write(Cells[i, j].PossibleValues.Contains(l + 1) ? (l + 1).ToString() : " ");
        //                    }
        //                    else
        //                    {
        //                        Console.ForegroundColor = ConsoleColor.Green;
        //                        if (l == 3)
        //                            Console.Write(Cells[i, j].Value);
        //                        else
        //                            Console.Write(" ");
        //                        Console.ForegroundColor = ConsoleColor.White;
        //                    }
        //                }
        //            }
        //            if (row == 0)
        //                row = 2;
        //            else if (row == 2)
        //                row = 0;
        //            Console.WriteLine();
        //        }
        //        if ((i + 1) % Util.Dimension == 0 && i != Util.Length - 1)
        //        {
        //            Console.WriteLine("▄▄▄▄▄▄▄▄█▄▄▄▄▄▄▄▄▄");
        //        }
        //        else
        //        {
        //            Console.WriteLine("---+----█----+----");
        //        }
        //    }
        //}

        public void Print()
        {
            Console.WriteLine();
            int row = 0;

            for (int j = 0; j < 9; j++)
            {
                for (int z = 0; z < 3; z++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if ((k % 3 == 0) && (k != 0))
                            Console.Write(" █ ");
                        else if (k != 0)
                            Console.Write(" | ");
                        for (int l = row; l < row + 3; l++)
                        {
                            if (Cells[j, k].IsBlank)
                                if (Cells[j, k].PossibleValues.Contains(l + 1))
                                    Console.Write(Cells[j, k].PossibleValues.Find(x => x == l + 1));
                                else
                                    Console.Write(" ");
                            else if (!Cells[j, k].IsBlank)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                if (l == 4)
                                    Console.Write(Cells[j, k].Value);
                                else
                                    Console.Write(" ");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                                Console.Write(" ");
                        }
                    }
                    if (row == 0)
                        row = 3;
                    else if (row == 3)
                        row = 6;
                    else if (row == 6)
                        row = 0;
                    Console.WriteLine();
                }
                if (((j + 1) % 3 == 0) && j != 8)
                    Console.WriteLine("▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
                else if (j != 8)
                    Console.WriteLine("----+-----+-----█-----+-----+-----█-----+-----+----");

            }
        }
    }
}
