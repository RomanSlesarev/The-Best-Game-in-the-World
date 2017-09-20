using Assets.Scripts.Core;

namespace Assets.Scripts.PathFinding.AStar
{
    public class AStarNode
    {
        public readonly Point Position;
        public readonly int DistanceToStart;
        public readonly int DistanceToGoal;
        public readonly AStarNode Parent;

        public int Cost
        {
            get { return DistanceToStart + DistanceToGoal; }
        }

        public AStarNode(Point position, int distanceToStart, int distanceToGoal, AStarNode parent)
        {

            Position = position;
            DistanceToStart = distanceToStart;
            DistanceToGoal = distanceToGoal;
            Parent = parent;
        }
    }
}
