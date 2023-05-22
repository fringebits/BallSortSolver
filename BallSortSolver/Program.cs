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
    internal class Levels
    {
        private Dictionary<string, char> ColorMap = new Dictionary<string, char>()
        {
            { "0", 'r' }, // red
            { "1", 'b' }, // blue
            { "2", 'w' }, // brown
            { "3", 'y' }, // yellow
            { "4", 'o' }, // orange
            { "5", 'p' }, // purple
            { "6", 'k' }, // pink
            { "7", 'l' }, // light-blue
            { "8", 'd' }, // dark-green
            { "9", 'e' }, // grey
            {"10", 'g' }, // green
            {"11", 'a' }, // aqua
        };

        public Levels(string filename)
        {
            this.Boards = new List<Board>();
            this.LoadBoards(filename);
        }

        public List<Board> Boards { get; }

        private void LoadBoards(string filename)
        {
            var levels = File.ReadAllText(filename);
            foreach(var level in levels.Split('-'))
            {
                var board = new Board(14, 4, 12);
                var tubeIndex = 0;
                foreach(var tubeString in level.Split(':'))
                {
                    var tube = new StringBuilder();
                    foreach(var color in tubeString.Split(','))
                    {
                        var value = color.Trim('\r', '\n');
                        tube.Append(ColorMap[value]);
                    }

                    board.SetTube(tubeIndex, new Tube(board, tubeIndex, new string(tube.ToString().Reverse().ToArray())));
                    tubeIndex++;
                }

                this.Boards.Add(board);
            }
        }
    };


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

            var levelFile = Path.Combine(resourcePath, "levels.txt");
            using (var stream = new StreamWriter($"solution_{Path.GetFileName(levelFile)}"))
            {
                var levels = new Levels(levelFile);

                var levelIndex = 1;
                foreach(var level in levels.Boards)
                {
                    var solution = SolveBoard(level);

                    if (solution.Count == 0)
                    {
                        Console.WriteLine($"Level {levelIndex}: No Solution");
                        stream.WriteLine($"Level {levelIndex}: No Solution");
                    }
                    else
                    { 
                        Console.WriteLine($"Level {levelIndex}: Solved in {solution.Count} moves");
                        stream.WriteLine($"Level {levelIndex}: Solved in {solution.Count} moves");

                        var index = 0;
                        foreach (var move in solution)
                        {
                            if (move.Move != null)
                            {
                                move.Move.PrintMove(stream, $"\t{index,3}.  ");
                                move.PrintBoard(stream, null);
                            }
                            index++;
                        }
                    }

                    levelIndex++;
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
                //Console.WriteLine($"No solution!!! Visited = {solver.Visited.Count}");
                return solution;
            }

            //Console.WriteLine($"Solved!!! Visited = {solver.Visited.Count}");
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
