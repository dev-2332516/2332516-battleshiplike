namespace LibrairieClasse
{
    public class Coordinate(int x, int y)
    {
        private readonly int _x = x;   
        private bool _touched = false;
        
        public int Y { get;} = y;

        public Coordinate(char x, int y) : this(x - 64, y)
        {
        }

        public int GetXInt()
        {
            return _x;
        }

        public char GetXChar()
        {
            return (char)(_x + 64);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Coordinate other)
            {
                return _x == other._x && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_x, Y);
        }

        public bool IsNextTo(Coordinate aside)
        {
            return (aside.GetXInt() == _x+1 && aside.Y == Y) ||
                   (aside.GetXInt() == _x-1 && aside.Y == Y) ||
                   (aside.GetXInt() == _x && aside.Y == Y+1) ||
                   (aside.GetXInt() == _x && aside.Y == Y-1);
        }

        public void Touched()
        {
            _touched = true;
        }

        public bool IsTouched()
        {
            return _touched;
        }
    }
}
