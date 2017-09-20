using Assets.Scripts.GameManagement;
using UnityEngine;

namespace Assets.Scripts.Unit.Addition
{
    public class DodgeScript : MonoBehaviour
    {
        private Vector2 _bulletPosition;
        private CircleCollider2D _collider;
        private CircleCollider2D _parentCollider;


        private void Start()
        {
            _collider = GetComponent<CircleCollider2D>();
            _parentCollider = transform.parent.transform.GetComponent<CircleCollider2D>();
        }

        private void OnTriggerStay2D(Collider2D enteredCollider)
        {

            if (enteredCollider.tag == BulletController.TagBulletPlayer)
            {
                _bulletPosition = enteredCollider.transform.position - transform.position;
                var a = _bulletPosition.y;
                var b = _bulletPosition.x;
                var c = b * enteredCollider.transform.right.y - a * enteredCollider.transform.right.x;
                var closestPoint = Mathf.Abs(c) / Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
                if (closestPoint < _parentCollider.radius / _collider.radius)
                {
                    var tangentVector = new Vector2(-a * c / (Mathf.Pow(a, 2) + Mathf.Pow(b, 2)),
                        -b * c / (Mathf.Pow(a, 2) + Mathf.Pow(b, 2)));
                    tangentVector = new Vector2(-tangentVector.x, tangentVector.y);
                    tangentVector = tangentVector.normalized;
                    GameManager.Instance.Bot.GetDodgeVector(tangentVector);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D quittedCollider)
        {
            if (quittedCollider.tag == BulletController.TagBulletPlayer)
            {
                GameManager.Instance.Bot.GetDodgeVector(Vector2.zero);
            }
        }
    }
}
