using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.GameManagement;
using Assets.Scripts.PathFinding;
using Assets.Scripts.Unit.Addition;
using UnityEngine;

namespace Assets.Scripts.Unit
{   
    [RequireComponent(typeof(Rigidbody2D))]
    public class BotController : UnitBase
    {
        public const string Tag = "Enemy";

        private const float DecisionTime = 0.5f;

        public bool PathGizmos = false;

        private Rigidbody2D _rigidbody2D;
        private GameManager _gameManager;
        private Grid _grid;
        private List<Point> _path;
        private Vector2 _dodgeVector;
        private Vector2 _randomVector;

        private float _timer;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _grid = _gameManager.GameFieldManager.Grid;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            gameObject.tag = Tag;
            _timer = DecisionTime;

            EnemyBulletTag = BulletController.TagBulletPlayer;
        }

        private void Update()
        {

            if (GameManager.Instance.Pause)
            {
                _rigidbody2D.velocity = Vector2.zero;
                return;
            }

            if (_grid == null)
            {
                return;
            }

            if (HP <= 0)
            {
                GameManager.Instance.UnitDead(Tag);
            }

            _rigidbody2D.velocity = Vector2.zero;

            var aimRays = FindTarget(PlayerController.Tag);

            if (aimRays.Capacity != 0)
            {
                if (Reloading <= 0)
                {
                    ShootTarget(aimRays);
                    Reloading = ReloadTime;
                }

                if (_dodgeVector != Vector2.zero)
                {
                    _rigidbody2D.velocity = _dodgeVector * Speed;
                }
                else
                {
                    if (_timer <= 0)
                    {
                        var x = Random.Range(-1, 2);
                        var y = Random.Range(-1, 2);
                        _randomVector = new Vector2(x, y).normalized * Speed;
                        _timer = DecisionTime;
                    }
                    else
                    {
                        _rigidbody2D.velocity = _randomVector;
                    }
                }
            }
            else
            {
                if (_dodgeVector != Vector2.zero)
                {
                    _rigidbody2D.velocity = _dodgeVector * Speed;
                }
                else
                {
                    GoToTarget();
                }

            }

            if (Reloading > 0)
            {
                Reloading -= Time.deltaTime;
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
        }

        public void GetDodgeVector(Vector2 vector)
        {
            _dodgeVector = vector;
        }

        private void GoToTarget()
        {
            Point startPosition;
            Point goalPosition;
            var isPlayerInBounds = _grid.TryToGridPosition(transform.position, out startPosition);
            var isEnemyInBounds = _grid.TryToGridPosition(_gameManager.Player.transform.position, out goalPosition);

            if (isPlayerInBounds && isEnemyInBounds)
            {
                _path = _gameManager.PathFinding.FindPath(startPosition, goalPosition);

                if (_path.Count > 1)
                {
                    _rigidbody2D.velocity =
                        (_grid.ToWorldPosition(_path[0]) - transform.position).normalized * Speed;
                }
            }
        }

        private List<AimRayHit> FindTarget(string targetTag)
        {
            var aimRays = new List<AimRayHit>();
            for (int i = 0; i < 360; i++)
            {
                AimRayHit aimRayHit;
                var deg = i * Mathf.Deg2Rad;
                var hit = AimRay.Shot(transform.position, new Vector2(Mathf.Cos(deg), Mathf.Sin(deg)), 30,
                    LayerMask.GetMask("WallLayer") | LayerMask.GetMask("PlayerLayer"), targetTag, out aimRayHit);
                if (hit)
                {
                    aimRays.Add(aimRayHit);
                }
            }
            return aimRays;
        }

        private void ShootTarget(List<AimRayHit> aimRays)
        {
            var shortestRay = aimRays.FirstOrDefault();
            foreach (var aimRay in aimRays)
            {
                if (aimRay.Distance > shortestRay.Distance)
                {
                    shortestRay = aimRay;
                }
            }
            var lookAtPosition = new Vector2(transform.position.x, transform.position.y) + shortestRay.Direction;
            LookAt(lookAtPosition);
            Shoot(BulletController.TagBulletEnemy);
        }

        private void OnDrawGizmos()
        {
            if (PathGizmos && _path != null)
            {
                Gizmos.color = Color.red;
                var cubeSize = new Vector3(_grid.Scale, _grid.Scale);
                foreach (var pathPoint in _path)
                {
                    var worldPosition = _grid.ToWorldPosition(pathPoint);
                    Gizmos.DrawCube(worldPosition, cubeSize);
                }
            }
        }
    }
}
