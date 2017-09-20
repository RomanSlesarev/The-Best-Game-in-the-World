using UnityEngine;

namespace Assets.Scripts.Unit
{
    public class UnitBase : MonoBehaviour
    {
        protected float Reloading;
        protected float ReloadTime = 1f;
        protected float Speed = 10f;
        protected string EnemyBulletTag;

        public int HP;

        protected virtual void Shoot(string bulletTag)
        {
            var bullet = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, transform.position, transform.rotation);
            bullet.tag = bulletTag;
        }

        protected void LookAt(Vector2 target)
        {
            var angle = Vector2.Angle(Vector2.right, (Vector3)target - transform.position);
            transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < target.y ? angle : -angle);
        }

        protected void OnTriggerEnter2D(Collider2D enteredCollider)
        {
            if (!string.IsNullOrEmpty(EnemyBulletTag) && enteredCollider.tag == EnemyBulletTag)
            {
                --HP;
                Destroy(enteredCollider.gameObject);
            }
        }
    }
}
