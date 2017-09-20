using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.PathFinding.AStar
{
    public class AStar : IPathFinding
    {
        private const int DiagonalStepCost = 14;
        private const int StraightStepCost = 10;

        private List<AStarNode> _openNodes;
        private HashSet<Point> _closedNodes;
        //TODO: сделать private
        public AStarNode[,] _nodesGrid;

        private readonly Grid _grid;

        public AStar(Grid grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            _grid = grid;
        }

        public List<Point> FindPath(Point startPoint, Point goalPoint)
        {
            _openNodes = new List<AStarNode>();
            _closedNodes = new HashSet<Point>();
            _nodesGrid = new AStarNode[_grid.SizeX, _grid.SizeY];

            var firstNode = new AStarNode(startPoint, 0, GetDistance(startPoint, goalPoint), null);

            _nodesGrid[firstNode.Position.X, firstNode.Position.Y] = firstNode;
            _openNodes.Add(firstNode);

            while (_openNodes.Count > 0 && _openNodes.Count < _grid.SizeX * _grid.SizeY)
            {
                var minimalCostNode = _openNodes.FirstOrDefault();
                foreach (var currentNode in _openNodes)
                {
                    if (minimalCostNode != null && currentNode.Cost <= minimalCostNode.Cost)
                    {
                        minimalCostNode = currentNode;
                    }
                }

                if (minimalCostNode.DistanceToGoal == 0)
                {
                    return BuildPath(minimalCostNode);
                }

                OpenNode(_nodesGrid, minimalCostNode, goalPoint);
                _openNodes.Remove(minimalCostNode);

                _closedNodes.Add(minimalCostNode.Position);
            }
            return new List<Point> { startPoint };
        }

        private List<Point> BuildPath(AStarNode endNode)
        {
            var path = new List<Point>();
            var currentNode = endNode;
            while (currentNode.Parent != null)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        private void OpenNode(AStarNode[,] nodesGrid, AStarNode currentNode, Point goalPoint)
        {
            for (var y = -1; y <= 1; ++y)
            {
                for (var x = -1; x <= 1; ++x)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    var openedNodePosition = new Point(currentNode.Position.X + x, currentNode.Position.Y + y);

                    if (_closedNodes.Contains(openedNodePosition))
                    {
                        continue;
                    }

                    if (openedNodePosition.X >= 0 && openedNodePosition.Y >= 0 && openedNodePosition.X < _grid.SizeX && openedNodePosition.Y < _grid.SizeY
                        && !_grid[openedNodePosition.X, openedNodePosition.Y].IsBlocked)
                    {
                        var openedNode = new AStarNode(openedNodePosition,
                            currentNode.DistanceToStart + GetDistance(currentNode.Position, openedNodePosition),
                            GetDistance(openedNodePosition, goalPoint), currentNode);

                        var oldNode = nodesGrid[openedNodePosition.X, openedNodePosition.Y];
                        if (oldNode != null)
                        {
                            if (oldNode.Cost > openedNode.Cost)
                            {
                                _openNodes.Remove(oldNode);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        nodesGrid[openedNodePosition.X, openedNodePosition.Y] = openedNode;
                        _openNodes.Add(openedNode);
                    }
                }
            }
        }

        public static int GetDistance(Point firstPoint, Point secondPoint)
        {
            var differencePoint = secondPoint - firstPoint;
            differencePoint.X = Mathf.Abs(differencePoint.X);
            differencePoint.Y = Mathf.Abs(differencePoint.Y);
            var diagonalSteps = differencePoint.X > differencePoint.Y ? differencePoint.Y : differencePoint.X;
            var straightSteps = differencePoint.X > differencePoint.Y
                ? differencePoint.X - differencePoint.Y
                : differencePoint.Y - differencePoint.X;
            return diagonalSteps * DiagonalStepCost + straightSteps * StraightStepCost;
        }
    }
}
