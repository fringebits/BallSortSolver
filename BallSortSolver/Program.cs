using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSortSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board(14, 4, 12);
            board.SetTube(0, new Tube(board, 0, "rrrb"));
            board.SetTube(1, new Tube(board, 1, "bbbr"));
            board.SortTubes();
            board.WriteToFile("C:\\users\\mikeg\\Desktop\\board_test.txt");

            board.PrintBoard("Test Board");

            Assert.AreEqual(board, board);
            var copy = new Board(board);
            copy.PrintBoard("Copy Board");
            Assert.AreEqual(board, copy);

            var next = board.GetNextBoards();
            Assert.AreEqual(2, next.Count);

            board = Board.CreateFromFile("C:\\users\\mikeg\\Desktop\\board_069.txt");
            SolveBoard(board);
        }

        static void SolveBoard(Board board)
        {
            var solver = new Solver();
            var ret = solver.FindSolution(board);
            Console.WriteLine($"Solved = {ret}");
            while (ret != null)
            {
                ret.PrintBoard("parent");
                ret = ret.Parent;
            }
        }
    }
}
