using UnityEngine;

namespace Assets.Scripts.Core
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public static implicit operator Point(Vector2 vector2)
        {
            return new Point((int) vector2.x, (int) vector2.y);
        }

        public static explicit operator Vector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Point operator + (Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator - (Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
    }
}
