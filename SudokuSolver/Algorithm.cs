using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Algorithm
    {
        private Field _baseField;
        private Field _currentSolutionField;
        private int _solutions;
        private List<Cell> _blankCells;

        public Algorithm(Field field)
        {
            _baseField = field;
        }

        public void Start()
        {
            _baseField.Print();
            _baseField.RecalculateSections();
            _baseField.Print();

            _currentSolutionField = _baseField.Copy();

            _blankCells = _baseField.BlankCells;

            Console.WriteLine("\tAlgorithm started...");
            while (!_currentSolutionField.IsSolved())
            {
                int numBlankCells = _currentSolutionField.FilledCellsCount;

                _currentSolutionField.RecalculateUsingSinglePossibility();
                _currentSolutionField.Print();

                if (numBlankCells == _currentSolutionField.FilledCellsCount)
                {
                    RecursiveGuess(_currentSolutionField, 0);
                }

                if (_currentSolutionField.IsFinished && !_currentSolutionField.IsSolved())
                {
                    Restart();
                }
            }

            if (_currentSolutionField.IsFinished && _currentSolutionField.IsSolved())
            {
                Console.WriteLine("\t{0} solutions", _solutions);
                _currentSolutionField.Print();
            }
        }

        public void Restart()
        {
            _currentSolutionField = _baseField.Copy();
            _blankCells = _currentSolutionField.BlankCells;
            Start();
        }

        public void RecursiveGuess(Field previous, int previousGuess)
        {
            bool blocked = false;
            bool fullyBlocked = false;
            int currentGuess = 0;
            Field tempField = null;

            if (previous.CurrentlyVerified())
            {
                if (!previous.IsFinished)
                {
                    tempField = previous.Copy();

                    _blankCells = tempField.BlankCells;
                    if (previousGuess != 0)
                    {
                        currentGuess = _blankCells.First().PossibleValues.Find(x => x > previousGuess);
                        if (currentGuess == 0)
                        {
                            fullyBlocked = true;
                        }
                    }
                    else if (currentGuess < _blankCells.First().PossibleValues.First())
                    {
                        currentGuess = _blankCells.First().PossibleValues.First();
                    }

                    if (currentGuess != 0)
                    {
                        _blankCells.First().AssignValue(currentGuess);
                    }
                    else
                    {
                        blocked = true;
                    }

                    if (tempField.IsFinished && !tempField.IsSolved())
                    {
                        blocked = true;
                    }

                    if (!blocked)
                    {
                        RecursiveGuess(tempField, 0);
                    }
                }
                else if (previous.IsSolved())
                {
                    fullyBlocked = true;

                    if (!previous.Equals(tempField))
                    {
                        _currentSolutionField = previous.Copy();
                    }

                    _solutions++;
                }
            }
            else
            {
                fullyBlocked = true;
            }

            if (!fullyBlocked)
            {
                RecursiveGuess(previous, currentGuess);
            }
        }
    }
}
