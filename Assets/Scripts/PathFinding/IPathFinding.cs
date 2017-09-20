using System.Collections.Generic;
using Assets.Scripts.Core;

namespace Assets.Scripts.PathFinding
{
    public interface IPathFinding
    {
        List<Point> FindPath(Point startPoint, Point goalPoint);
    }
}
