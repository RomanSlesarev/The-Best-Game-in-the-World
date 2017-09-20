using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.PathFinding;
using Assets.Scripts.PathFinding.AStar;
using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.GameManagement
{
    public class GameManager : Singleton<GameManager>
    {
        private const int TotalHP = 5;

        public PlayerController Player;
        public BotController Bot;
        public GameFieldManager GameFieldManager;
        public IPathFinding PathFinding;
        public GUIController GUIController;

        public bool Pause { get; private set; }

        private Vector2 _playerPosition;
        private Vector2 _botPosition;
        private bool _isRespawning;

        public void Start()
        {
            PathFinding = new AStar(GameFieldManager.Grid);
            _playerPosition = Player.transform.position;
            _botPosition = Bot.transform.position;
            StartGame();
        }

        public void StartGame()
        {
            Player.transform.position = _playerPosition;
            Player.HP = TotalHP;

            Bot.transform.position = _botPosition;
            Bot.HP = TotalHP;

            DestroyBullets();

            StartCoroutine(StartCountDown());
        }

        private void DestroyBullets()
        {
            var bulletsArray = FindObjectsOfType<BulletController>();

            foreach (var bullet in bulletsArray)
            {
                Destroy(bullet.gameObject);
            }
        }

        private IEnumerator StartCountDown()
        {
            Pause = true;

            while (_isRespawning)
            {
                yield return null;
            }

            for (int i = 1; i <= 3; i++)
            {
                GUIController.GameManagerText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            Pause = false;
            GUIController.GameManagerText.text = string.Empty;
        }

        private IEnumerator WaitUntilRespawn(string deadTag)
        {
            _isRespawning = true;
            GUIController.GameManagerText.text = deadTag + " is dead";
            for (int i = 1; i <= 3; i++)
            {
                yield return new WaitForSeconds(1);
            }

            GUIController.GameManagerText.text = string.Empty;
            _isRespawning = false;
        }

        public void UnitDead(string deadTag)
        {
            StartCoroutine(WaitUntilRespawn(deadTag));
            StartGame();
        }
    }

}
