using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSortSolver
{
    class Ball
    {
        public Ball(char color)
        {
            this.Color = color;
        }

        public char Color { get; set; }

        public override string ToString()
        {
            return this.Color.ToString();
        }

        public static bool operator==(Ball a, Ball b)
        {
            return a.Color == b.Color;
        }

        public static bool operator !=(Ball a, Ball b)
        {
            return a.Color != b.Color;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
