using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSortSolver
{
    class Board
    {
        public List<Tube> Tubes { get; set; }

        public int NumColors { get; set; }

        public Board(int numTubes, int numBallsPerTube, int numColors)
        {
            this.MaxBallsPerTube = numBallsPerTube;
            this.Tubes = new List<Tube>();
            for(var ii=0; ii < numTubes; ii++)
            {
                this.Tubes.Add(new Tube(this, ii));
            }
            this.NumColors = numColors;
        }

        public Board(Board board)
        {
            this.Tubes = new List<Tube>(board.NumTubes);
            this.NumColors = board.NumColors;
            this.MaxBallsPerTube = board.MaxBallsPerTube;

            foreach(var tube in board.Tubes)
            {
                this.Tubes.Add(new Tube(this, tube));
            }
        }

        public void WriteToFile(string fullpath)
        {
            using (var textWriter = File.CreateText(fullpath))
            {
                foreach(var tube in this.Tubes)
                {
                    textWriter.Write($"{tube}\n");
                }
            }
        }

        public static Board CreateFromFile(string fullpath)
        {
            if (!File.Exists(fullpath))
            {
                return null;
            }

            var lines = File.ReadLines(fullpath);
            var board = new Board(lines.Count()+2, 4, lines.Count());
            var index = 0;
            foreach(var line in lines)
            {
                board.SetTube(index, new Tube(board, index, line));
                index++;
            }

            return board;
        }

        [JsonIgnore] 
        public Board Parent { get; set; }

        // This is the move that takes us from Parent to This.
        [JsonIgnore]
        public Move Move { get; set; }

        public int MaxBallsPerTube { get; set; }

        [JsonIgnore]
        public int NumTubes => this.Tubes.Count;

        [JsonIgnore]
        public bool IsWinner => this.Tubes.All(t => t.IsEmpty || t.IsComplete);

        public void SetTube(int index, Tube tube)
        {
            this.Tubes[index] = tube;
        }

        public void SortTubes()
        {
            this.Tubes.Sort((a,b) => a.Value - b.Value);
        }

        public bool TryMove(int source, int target, out Board board)
        {
            board = null;
            if (Tube.CanMove(this.Tubes[source], this.Tubes[target]))
            {
                board = new Board(this);
                var result = Tube.TryMove(board.Tubes[source], board.Tubes[target]);
                if (!result)
                {
                    throw new Exception("TryMove should not have failed.");
                }
                return result;
            }

            return false;
        }

        public List<Board> GetNextBoards()
        {
            var result = new List<Board>();

            for (var ii=0; ii < this.Tubes.Count; ii++)
            {
                for (var kk=0; kk < this.Tubes.Count; kk++)
                {
                    if (ii == kk)
                    {
                        continue;
                    }

                    if (this.TryMove(ii, kk, out var board))
                    {
                        board.SortTubes();
                        if (!result.Contains(board))
                        {
                            board.Parent = this;
                            // ii -> kk
                            board.Move = new Move(this.Tubes[ii].Id, this.Tubes[kk].Id);
                            result.Add(board);
                        }
                    }
                }
            }

            return result;
        }

        public override string ToString()
        {
            var tubes = new List<Tube>(this.Tubes);
            tubes.Sort((x, y) => x.Id - y.Id);
            var result = new StringBuilder();
            for(var ii=MaxBallsPerTube-1; ii >= 0; ii--)
            {
                foreach(var tube in tubes)
                {
                    result.Append($"{tube.GetBallString(ii)}  ");
                }
                result.AppendLine();
            }            

            return result.ToString();
        }

        public void PrintBoard(string desc)
        {
            PrintBoard(Console.Out, desc);
        }

        public void PrintBoard(TextWriter stream, string desc)
        {
            if (!string.IsNullOrEmpty(desc))
            {
                stream.WriteLine($"--- {desc} ---");
            }
            stream.WriteLine(this.ToString());
        }

        public override bool Equals(object obj)
        {
            var board = obj as Board;

            if (board == null)
            {
                return false;
            }

            for (var ii=0; ii < this.Tubes.Count; ii++)
            {
                if (!Tube.Equals(this.Tubes[ii], board.Tubes[ii]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
