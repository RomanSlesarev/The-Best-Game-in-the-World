using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError(string.Format("{0}: More than one GameManager object!", typeof(Singleton<T>)));
                        return null;
                    }
                }
                return _instance;
            }
        }

        private static T _instance;

    }
}
