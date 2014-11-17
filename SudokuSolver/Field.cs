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
        private Cell[,] _field;
        private List<Section> _rows;
        private List<Section> _columns;
        private List<Section> _squares;

        public int FilledCellsCount
        {
            get { return _field.OfType<Cell>().Count(cell => !cell.IsBlank); }
        }

        public Field(int[,] values)
        {
            _field = new Cell[Util.Length, Util.Length];
            _rows = new List<Section>();
            _columns = new List<Section>();
            _squares = new List<Section>();

            for (int i = 0; i < Util.Length; i++)
            {
                _rows.Add(new Section(SectionType.Row));
                _columns.Add(new Section(SectionType.Column));
                _squares.Add(new Section(SectionType.Square));
            }

            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    if (values[i, j] > 0
                        && values[i, j] <= Util.Length)
                    {
                        _field[i, j] = new Cell(values[i, j]);
                    }
                    else
                    {
                        _field[i, j] = new Cell();
                    }
                }
            }

            var rowCells = new List<Cell>();
            var columnCells = new List<Cell>();
            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    rowCells.Add(_field[i, j]);
                    columnCells.Add(_field[j, i]);
                }
                _rows[i].AssignCells(rowCells);
                _columns[i].AssignCells(columnCells);
                rowCells = new List<Cell>();
                columnCells = new List<Cell>();
            }

            var squareCells = new List<Cell>();
            int reg = 0;
            for (int i = 0; i < _field.GetLength(0); i += Util.Dimension)
            {
                for (int j = 0; j < _field.GetLength(1); j += Util.Dimension)
                {
                    for (int k = i; k < i + Util.Dimension; k++)
                    {
                        for (int l = j; l < j + Util.Dimension; l++)
                        {
                            squareCells.Add(_field[k, l]);
                        }
                    }

                    _squares[reg].AssignCells(squareCells);
                    ++reg;
                    squareCells = new List<Cell>();
                }
            }
        }

        public void RecalculateSections()
        {
            foreach (var row in _rows)
            {
                row.RecalculatePossibilities();
            }
            foreach (var col in _columns)
            {
                col.RecalculatePossibilities();
            }
            foreach (var square in _squares)
            {
                square.RecalculatePossibilities();
            }
        }

        public void PrintEverything()
        {
            Console.WriteLine();
            int row = 0;

            for (int i = 0; i < Util.Length; i++)
            {
                for (int z = 0; z < Util.Dimension; z++)
                {
                    for (int j = 0; j < Util.Length; j++)
                    {
                        if (j % Util.Dimension == 0 && j != 0)
                        {
                            Console.Write(" █ ");
                        }
                        else if (j != 0)
                        {
                            Console.Write(" | ");
                        }
                        for (int l = row; l < row + Util.Dimension; l++)
                        {
                            if (_field[i, j].IsBlank)
                            {
                                Console.Write(_field[i, j].PossibleValues.Contains(l + 1) ? (l + 1).ToString() : " ");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                if (l == 3)
                                    Console.Write(_field[i, j].Value);
                                else
                                    Console.Write(" ");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                    if (row == 0)
                        row = 2;
                    else if (row == 2)
                        row = 0;
                    Console.WriteLine();
                }
                if ((i + 1) % Util.Dimension == 0 && i != Util.Length - 1)
                {
                    Console.WriteLine("▄▄▄▄▄▄▄▄█▄▄▄▄▄▄▄▄▄");
                }
                else
                {
                    Console.WriteLine("---+----█----+----");
                }
            }
        }

        //public void PrintEverything()
        //{
        //    Console.WriteLine();
        //    int row = 0;

        //    for (int j = 0; j < 9; j++)
        //    {
        //        for (int z = 0; z < 3; z++)
        //        {
        //            for (int k = 0; k < 9; k++)
        //            {
        //                if ((k % 3 == 0) && (k != 0))
        //                    Console.Write(" █ ");
        //                else if (k != 0)
        //                    Console.Write(" | ");
        //                for (int l = row; l < row + 3; l++)
        //                {
        //                    if (_field[j, k].IsBlank)
        //                        if (_field[j, k].PossibleValues.Contains(l + 1))
        //                            Console.Write(_field[j, k].PossibleValues.Find(x => x == l + 1));
        //                        else
        //                            Console.Write(" ");
        //                    else if (!_field[j, k].IsBlank)
        //                    {
        //                        Console.ForegroundColor = ConsoleColor.Green;
        //                        if (l == 4)
        //                            Console.Write(_field[j, k].Value);
        //                        else
        //                            Console.Write(" ");
        //                        Console.ForegroundColor = ConsoleColor.White;
        //                    }
        //                    else
        //                        Console.Write(" ");
        //                }
        //            }
        //            if (row == 0)
        //                row = 3;
        //            else if (row == 3)
        //                row = 6;
        //            else if (row == 6)
        //                row = 0;
        //            Console.WriteLine();
        //        }
        //        if (((j + 1) % 3 == 0) && j != 8)
        //            Console.WriteLine("▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄");
        //        else if (j != 8)
        //            Console.WriteLine("----+-----+-----█-----+-----+-----█-----+-----+----");

        //    }
        //}
    }
}
