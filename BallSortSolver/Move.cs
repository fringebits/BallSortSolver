using System.IO;

namespace BallSortSolver
{
    // Last ball in the tube represents top-most ball
    class Move
    {
        public Move(int source, int target)
        {
            this.Source = source;
            this.Target = target;
        }

        public int Source { get; }

        public int Target { get; }

        public void PrintMove(TextWriter stream, string desc)
        {
            stream.WriteLine($"{desc}{this.Source+1,2} --> {this.Target+1,2}");
        }
    }
}
