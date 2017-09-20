using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameManagement;
using UnityEngine;

namespace Assets.Scripts.Unit.Addition
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ActionWatcher : MonoBehaviour
    {
        private readonly Queue<ActionName> _actions = new Queue<ActionName>();
        private Rigidbody2D _rigidbody;
        private float _timer;
        private float _timeForAction = 7f;
        private float _durationOfSpin = 1f;
        private bool _shotDone;

        private void Start()
        {
            _timer = _timeForAction;
            _rigidbody = GetComponent<Rigidbody2D>();
            StartCoroutine(Rotation360());
        }

        private void Update()
        {
            if (_rigidbody.velocity != Vector2.zero && (_actions.LastOrDefault() != ActionName.Move || _actions.LastOrDefault() == ActionName.None))
            {
                _actions.Enqueue(ActionName.Move);
            }

            if (_shotDone && (_actions.LastOrDefault() != ActionName.Shot || _actions.LastOrDefault() == ActionName.None))
            {
                _actions.Enqueue(ActionName.Shot);
                _shotDone = false;
            }

            if (!GameManager.Instance.Pause && _timer <= 0)
            {
                GameManager.Instance.GUIController.SetActionString(_actions);
            }

            if (_timer > 0 && _actions.LastOrDefault() != ActionName.None)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _timer = _timeForAction;
                _actions.Clear();
            }
        }

        private IEnumerator Rotation360()
        {
            while (true)
            {
                var timer = 0f;
                var currentAngle = transform.eulerAngles.z;
                var totalRotation = 0f;
                yield return null;

                if (currentAngle == transform.eulerAngles.z)
                {
                    continue;
                }

                var sign = Mathf.Sign(Mathf.DeltaAngle(currentAngle, transform.eulerAngles.z));
                currentAngle = transform.eulerAngles.z;
                yield return null;

                while (timer < _durationOfSpin)
                {
                    timer += Time.deltaTime;

                    if (currentAngle == transform.eulerAngles.z)
                    {
                        yield return null;
                        continue;
                    }

                    if (Mathf.Sign(Mathf.DeltaAngle(currentAngle, transform.eulerAngles.z)) == sign)
                    {
                        totalRotation += Mathf.DeltaAngle(currentAngle, transform.eulerAngles.z);
                        currentAngle = transform.eulerAngles.z;
                        if (Mathf.Abs(totalRotation) >= 360)
                        {
                            if (_actions.LastOrDefault() != ActionName.Spin360)
                            {
                                _actions.Enqueue(ActionName.Spin360);
                            }
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    yield return null;
                }
            }
        }

        public enum ActionName
        {
            None,
            Move,
            Spin360,
            Ricochet,
            Shot,
        }

        public void Shot()
        {
            _shotDone = true;
        }

        public void HandleRicochet()
        {
            if (_actions.LastOrDefault() != ActionName.Ricochet)
            {
                _actions.Enqueue(ActionName.Ricochet);
            }
        }
    }
}

