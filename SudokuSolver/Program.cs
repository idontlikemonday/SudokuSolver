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
            //values[0, 0] = 1;
            //values[0, 1] = 2;
            //values[1, 2] = 1;
            //values[1, 3] = 2;
            //values[2, 0] = 3;
            //values[3, 3] = 4;

            values[0, 0] = 4;
            //values[0, 1] = 7;
            //values[0, 3] = 1;
            //values[1, 0] = 5;
            //values[1, 1] = 3;
            //values[1, 6] = 1;
            //values[2, 6] = 3;

            values[0, 2] = 2;
            values[0, 4] = 3;
            values[0, 5] = 6;
            values[0, 8] = 5;
            values[1, 3] = 9;
            values[2, 1] = 6;
            values[2, 4] = 2;
            values[2, 7] = 4;
            values[3, 7] = 1;
            values[4, 0] = 1;
            values[4, 3] = 5;
            values[4, 5] = 7;
            values[4, 8] = 6;
            values[5, 0] = 3;
            values[6, 4] = 6;
            values[6, 7] = 8;
            values[7, 1] = 1;
            values[7, 5] = 2;
            values[8, 0] = 6;
            values[8, 3] = 4;
            values[8, 4] = 1;
            values[8, 6] = 2;

            var field = new Field(values);
            var solution = new Algorithm(field);
            solution.Start();

            Console.ReadKey();
        }
    }
}
