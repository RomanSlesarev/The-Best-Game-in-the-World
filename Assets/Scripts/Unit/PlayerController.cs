using System.Linq;
using Assets.Scripts.GameManagement;
using Assets.Scripts.Unit.Addition;
using UnityEngine;

namespace Assets.Scripts.Unit
{
    [RequireComponent(typeof(ActionWatcher))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : UnitBase
    {
        public bool AimGizmos;
        public const string Tag = "Player";

        private Rigidbody2D _rigidbody2D;
        private ActionWatcher _actionWatcher;
        
        
        private void Start()
        {
            _actionWatcher = GetComponent<ActionWatcher>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            gameObject.tag = Tag;
            EnemyBulletTag = BulletController.TagBulletEnemy;
        }

        private void Update()
        {
            if (GameManager.Instance.Pause)
            {
                _rigidbody2D.velocity = Vector2.zero;
                return;
            }

            if (HP <= 0)
            {
                GameManager.Instance.UnitDead(Tag);
            }

            var force = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            _rigidbody2D.velocity = new Vector2(force.x * Speed, force.y * Speed);

            var mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            LookAt(mousePosition);

            if (Input.GetMouseButtonDown(0) && Reloading <= 0)
            {
                Reloading = ReloadTime;
                Shoot(BulletController.TagBulletPlayer);
                _actionWatcher.Shot();
            }

            if (Reloading > 0)
            {
                Reloading -= Time.deltaTime;
            }
        }

        public void OnDrawGizmos()
        {
            if (!AimGizmos)
            {
                return;
            }

            AimRayHit aimRayHit;
            AimRay.Shot(transform.position, transform.right, 100, LayerMask.GetMask("WallLayer") | LayerMask.GetMask("EnemyLayer"), "Enemy", out aimRayHit);

            var firstPoint = aimRayHit.LinePath.FirstOrDefault();
            for (int i = 1; i < aimRayHit.LinePath.Count; i++)
            {
                var secondPoint = aimRayHit.LinePath[i];
                Gizmos.DrawLine(firstPoint, secondPoint);
                firstPoint = secondPoint;
            }
        }

        protected override void Shoot(string bulletTag)
        {
            var bullet = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, transform.position, transform.rotation);
            bullet.tag = bulletTag;

            var bulletController = bullet.GetComponent<BulletController>();
            bulletController.OnBulletRicochet = _actionWatcher.HandleRicochet;
        }
    }
}
