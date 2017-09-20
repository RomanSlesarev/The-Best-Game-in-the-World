using System;
using Assets.Scripts.GameManagement;
using UnityEngine;

namespace Assets.Scripts
{
    public class BulletController : MonoBehaviour
    {

        public static readonly string TagBulletPlayer = "BulletPlayer";
        public static readonly string TagBulletEnemy = "BulletEnemy";
        public Action OnBulletRicochet;

        private Rigidbody2D _rigidbody;
        private float _force = 1000f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.AddForce(transform.right * _force);
        }


        private void FixedUpdate()
        {
            //TODO: убрать магическое 10 на Infinity
            var raycast = Physics2D.Raycast(transform.position, transform.right, float.PositiveInfinity);

            if (raycast.collider != null && raycast.collider.tag == "Wall" && raycast.distance < 0.5f)
            {
                var reflectionDir = Vector2.Reflect(transform.right, raycast.normal);
                //TODO: убрать обнуление velocity
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(reflectionDir * _force);
                var angle = Mathf.Atan2(reflectionDir.y, reflectionDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                if (OnBulletRicochet != null)
                {
                    OnBulletRicochet();
                }
            }
        }


        private void OnTriggerExit2D(Collider2D leftcollider)
        {
            if (leftcollider.tag == GameFieldManager.Tag)
            {
                Destroy(gameObject);
            }
        }
    }
}
