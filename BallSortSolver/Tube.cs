using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSortSolver
{
    // Last ball in the tube represents top-most ball
    class Tube
    {
        public Tube(Board board, int id)
        {
            this.MaxBalls = board.MaxBallsPerTube;
            this.Balls = new List<Ball>(this.MaxBalls);
            this.Id = id;
        }

        public Tube(Board board, int id, string balls)
            : this(board, id)
        {
            foreach(var color in balls)
            {
                this.Balls.Add(new Ball(color));
            }
        }

        public Tube(Board board, Tube tube)
            : this(board, tube.Id)
        {
            this.Balls = tube.Balls.Select(b => new Ball(b.Color)).ToList();
        }

        public List<Ball> Balls { get; set; }

        public int MaxBalls { get; set; }

        public bool IsEmpty => !Balls.Any();

        public bool IsFull => Balls.Count == MaxBalls;

        public int Space => MaxBalls - Balls.Count;

        public int Value => this.Balls.Sum(b => b.Color);

        public int Id { get; set; }

        public Ball Top => this.Balls.Last();

        public bool IsComplete => this.IsFull && this.Balls.All(b => b == this.Balls.First());

        public List<Ball> GetTopBalls()
        {
            var result = new List<Ball>();
            if (this.IsEmpty)
            {
                return result;
            }

            result.Add(this.Top);
            for(var ii=this.Balls.Count - 2; ii >= 0; ii--)
            {
                if (this.Balls[ii] != result[0])
                {
                    break;
                }

                result.Add(this.Balls[ii]);
            }

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string GetBallString(int index)
        {
            if (index < this.Balls.Count)
            {
                return this.Balls[index].ToString();
            }

            return ".";
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach(var ball in this.Balls)
            {
                result.Append(ball.ToString());
            }
            for(var ii=result.Length; ii < this.MaxBalls; ii++)
            {
                result.Append(".");
            }

            return result.ToString();
        }

        public override bool Equals(object obj)
        {
            var tube = obj as Tube;
            if (tube == null)
            {
                return false;
            }

            if (tube.Balls.Count != this.Balls.Count)
            {
                return false;
            }

            for (var ii = 0; ii < this.Balls.Count; ii++)
            {
                if (tube.Balls[ii] != this.Balls[ii])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool TryMove(Tube source, Tube target)
        {
            if (!CanMove(source, target))
            {
                return false;
            }

            // remove these balls from the source
            var move = source.GetTopBalls();
            foreach (var ball in move)
            {
                source.Balls.Remove(ball);
                target.Balls.Add(ball);
            }

            return true;
        }

        public static bool CanMove(Tube source, Tube target)
        {
            if (source.IsEmpty)
            {
                return false;
            }

            if (target.IsFull)
            {
                return false;
            }

            if (target.IsEmpty)
            {
                return true;
            }

            var move = source.GetTopBalls();

            if (target.Space < move.Count)
            {
                return false;
            }

            if (target.Top != move.First())
            {
                return false;
            }

            return true;
        }
    }
}
