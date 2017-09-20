using Assets.Scripts.Core;
using Assets.Scripts.PathFinding;
using UnityEngine;

namespace Assets.Scripts.GameManagement
{
    public class GameFieldManager : MonoBehaviour
    {
        public int SizeX = 50;
        public int SizeY = 50;
        public float Scale = 1f;

        public bool GridGizmos = false;
        public bool WallsGizmos = false;
        public static string Tag = "GameField";

        public Grid Grid;

        private void Awake()
        {
            Grid = new Grid(SizeX, SizeY, Scale);
            for (int y = 0; y < SizeY; ++y)
            {
                for (int x = 0; x < SizeX; ++x)
                {
                    var colliderOnPoint = Physics2D.OverlapCircle(Grid.ToWorldPosition(new Point(x, y)), 0.7f, LayerMask.GetMask("WallLayer"));
                    var isBlocked = colliderOnPoint != null && colliderOnPoint.tag == "Wall";
                    Grid[x, y] = new Node(new Point(x, y), isBlocked);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (GridGizmos)
            {
                Gizmos.color = Color.cyan;
                //TODO: исправить раковые опухоли циклов
                for (float i = -SizeX * 0.5f; i <= SizeX * 0.5f; ++i)
                {
                    var x = i * Scale;
                    var y0 = -SizeY * 0.5f * Scale;
                    var y1 =SizeY * 0.5f * Scale;
                    Gizmos.DrawLine(new Vector3(x, y0), new Vector3(x, y1));
                }

                for (float i = -SizeY * 0.5f; i <= SizeY * 0.5f; ++i)
                {
                    var y = i * Scale;
                    var x0 = -SizeX * 0.5f * Scale;
                    var x1 = SizeX * 0.5f * Scale;
                    Gizmos.DrawLine(new Vector3(x0, y), new Vector3(x1, y));
                }
            }

            if (!Application.isPlaying)
            {
                return;
            }

            if (WallsGizmos)
            {
                Gizmos.color = new Color(1, 1, 0, 0.5f);
                var cubeSize = new Vector3(Scale, Scale);
                for (int y = 0; y < SizeY; ++y)
                {
                    for (int x = 0; x < SizeX; ++x)
                    {
                        if (Grid[x, y].IsBlocked)
                        {
                            Gizmos.DrawCube(Grid.ToWorldPosition(new Point(x, y)), cubeSize);
                        }
                    }
                }
            }
        }
    }
}
