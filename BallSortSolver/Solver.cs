using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSortSolver
{
    class Solver
    {
        public Solver()
        {
            this.Visited = new List<Board>();
        }

        public Board OriginBoard { get; set; }

        public List<Board> Visited { get; set; }

        public Board FindSolution(Board origin)
        {
            var open = new List<Board>();
            open.Add(origin);
            this.Visited.Add(origin);

            //origin.PrintBoard("Starting board");
            if (origin.IsWinner)
            {
                return origin;
            }

            while (open.Any())
            {
                var board = open.First();
                open.RemoveAt(0);
                //origin.PrintBoard($"Current Board, Open={open.Count}");

                var children = board.GetNextBoards();
                //Console.WriteLine($"Found {children.Count} moves.");

                //var index = 0;
                foreach(var child in children)
                {
                    //child.PrintBoard($"Child Board {index}");
                    if (this.Visited.Contains(child))
                    {
                        // we've already seen this board
                        continue;
                    }

                    this.Visited.Add(child);

                    if (child.IsWinner)
                    {
                        child.PrintBoard("Winner!!!");
                        return child;
                    }

                    //open.Add(child);
                    open.Insert(0, child);
                }
            }

            return null;
        }
    }
}