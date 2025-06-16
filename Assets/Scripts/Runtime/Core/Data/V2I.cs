using System;

namespace Core
{
    [Serializable]
    public struct V2I
    {
        public int X;
        public int Y;

        public V2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static V2I operator +(V2I obj1, V2I obj2)
        {
            return new V2I(obj1.X + obj2.X, obj1.Y + obj2.Y);
        }

        public static bool operator ==(V2I obj1, V2I obj2)
        {
            if (obj1.X == obj2.X && obj1.Y == obj2.Y)
                return true;

            return false;
        }

        public static bool operator !=(V2I obj1, V2I obj2)
        {
            if (obj1.X != obj2.X || obj1.Y != obj2.Y)
                return true;

            return false;
        }
    }
}