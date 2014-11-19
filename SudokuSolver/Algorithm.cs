using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Algorithm
    {
        private Field _field;

        public Algorithm(Field field)
        {
            _field = field;
        }

        public void Start()
        {
            _field.Print();
            Console.WriteLine(_field.FilledCellsCount);

            _field.RecalculateSections();
            _field.Print();
            Console.WriteLine(_field.FilledCellsCount);

            _field.RecalculateSections();
            _field.Print();
            Console.WriteLine(_field.FilledCellsCount);

            _field.RecalculateUsingSinglePossibility();
            _field.Print();
            Console.WriteLine(_field.FilledCellsCount);

            _field._field[3, 3].PossibleValues.Remove(1);
            _field.RecalculateSections();
            _field.RecalculateUsingSinglePossibility();
            _field.Print();
            Console.WriteLine(_field.FilledCellsCount);

        }
    }
}
