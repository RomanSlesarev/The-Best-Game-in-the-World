using Assets.Scripts.Core;

namespace Assets.Scripts.PathFinding
{
    public class Node
    {
        public readonly bool IsBlocked;
        public readonly Point Position;

        public Node(Point position, bool isBlocked)
        {
            Position = position;
            IsBlocked = isBlocked;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
