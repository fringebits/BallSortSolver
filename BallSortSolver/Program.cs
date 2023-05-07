using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

            var resourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Resources");
            var files = Directory.EnumerateFiles(resourcePath, "board_*.txt", SearchOption.TopDirectoryOnly);

            foreach(var file in files)
            {
                board = Board.CreateFromFile(file);
                var solution = SolveBoard(board);

                using (var stream = new StreamWriter($"solution_{Path.GetFileName(file)}"))
                {
                    var index = 0;

                    if (solution.Count == 0)
                    {
                        stream.WriteLine("No solution found.");
                    }

                    foreach (var move in solution)
                    {
                        move.PrintBoard($"Move {index}/{solution.Count}");
                        move.PrintBoard(stream, $"Move {index}/{solution.Count}");
                        index++;
                    }
                }
            }
        }

        static List<Board> SolveBoard(Board board)
        {
            var solver = new Solver();
            var solution = new List<Board>();
            var ret = solver.FindSolution(board);

            if (ret == null)
            {
                Console.WriteLine($"No solution!!! Visited = {solver.Visited.Count}");
                return solution;
            }

            Console.WriteLine($"Solved!!! Visited = {solver.Visited.Count}");
            // boards are in "reverse order" at this point (walking up looking at parents).
            // make understanding the moves easier now by reversing the order.
            while (ret != null)
            {
                solution.Insert(0, ret);
                ret = ret.Parent;
            }
            
            return solution;
        }
    }
}
