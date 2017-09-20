using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Unit.Addition;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameManagement
{
    public class GUIController : MonoBehaviour
    {
        private const float TimeActionsShow = 6f;

        private string _actionString;
        private float _timerActionsShow;

        public Text ScoreText;
        public Text ActionText;
        public Text GameManagerText;

        private void Update()
        {
            var hpString = string.Empty;
            hpString += string.Format("{0}: {1}\n", GameManager.Instance.Player.tag, GameManager.Instance.Player.HP);
            hpString += string.Format("{0}: {1}\n", GameManager.Instance.Bot.tag, GameManager.Instance.Bot.HP);

            hpString = hpString.Trim();

            ScoreText.text = hpString;

            if (_actionString != string.Empty)
            {
                ActionText.text = _actionString;
                _timerActionsShow += Time.deltaTime;
            }

            if (_timerActionsShow >= TimeActionsShow)
            {
                _timerActionsShow = 0;
                _actionString = string.Empty;
                ActionText.text = string.Empty;
            }
        }

        public string SetActionString(Queue<ActionWatcher.ActionName> actions)
        {
            if (actions.Count > 2)
            {
                foreach (var action in actions.ToList().Distinct())
                {
                    _actionString += action.ToString().ToUpper() + "x" + actions.Count(x => x == action) + "\n";
                }
            }
            actions.Clear();
            return _actionString;
        }
    }
}
