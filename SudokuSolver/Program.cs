using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var values = new int[Util.Length, Util.Length];
            values[0, 0] = 1;
            values[0, 1] = 2;
            values[1, 2] = 1;
            values[1, 3] = 2;
            values[2, 0] = 3;
            //values[3, 3] = 4;

            var field = new Field(values);
            var solution = new Algorithm(field);
            solution.Start();

            Console.ReadKey();
        }
    }
}
