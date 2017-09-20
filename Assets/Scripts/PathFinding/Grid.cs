using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.PathFinding
{
    public class Grid
    {
        public readonly float Scale;
        public readonly int SizeX;
        public readonly int SizeY;

        private Node[,] _gridCells;

        public Grid(int sizeX, int sizeY, float scale)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Scale = scale;

            _gridCells = new Node[SizeX, SizeY];
        }

        public Node this[int X, int Y]
        {
            get { return _gridCells[X, Y]; }

            set { _gridCells[X, Y] = value; }
        }

        public Node this[Point point]
        {
            get { return _gridCells[point.X, point.Y]; }

            set { _gridCells[point.X, point.Y] = value; }
        }

        public Vector3 ToWorldPosition(Point cellPosition)
        {
            var vectorPosition = (Vector2)cellPosition;
            return (vectorPosition  - new Vector2(SizeX - 1, SizeY - 1) * 0.5f) * Scale;
        }

        public bool TryToGridPosition(Vector2 worldPosition, out Point gridPosition)
        { //TODO: запилить Point алсо атак(задебажить)
            var vectorGridPosition = worldPosition / Scale + new Vector2(SizeX - 1, SizeY - 1) * 0.5f;
            gridPosition = new Point((int)Mathf.Round(vectorGridPosition.x), (int)Mathf.Round(vectorGridPosition.y));

            if (gridPosition.X >= SizeX || gridPosition.X < 0
                || gridPosition.Y >= SizeY || gridPosition.Y < 0)
            {
                return false;
            }
            return true;
        }
    }

    //public float GetDistance(Vector2 startPosition, Vector2 endPosition)
    //{
    //    return Vector2.Distance(ToWorldPosition((Point)startPosition), ToWorldPosition((Point)endPosition));
    //}
}