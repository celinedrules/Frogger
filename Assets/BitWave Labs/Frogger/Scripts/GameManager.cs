using System.Collections;
using System.Linq;
using UnityEngine;

namespace BitWave_Labs.Frogger.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private Frogger _frogger;
        private Home[] _homes;
        private int _score;
        private int _lives;
        private int _time;
        private const int RowAdvancedPoints = 10;
        private const int BonusMultiplier = 20;
        private const int HomeOccupiedPoints = 50;
        private const int LevelClearedPoints = 1000;

        private void Awake()
        {
            _frogger = FindAnyObjectByType<Frogger>();
            _homes = FindObjectsByType<Home>(FindObjectsSortMode.None);
        }

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            SetScore(0);
            SetLives(3);
            NewLevel();
        }

        private void NewLevel()
        {
            foreach (Home home in _homes)
            {
                home.enabled = false;
            }

            NewRound();
        }

        private void NewRound()
        {
            Respawn();
        }

        private void Respawn()
        {
            _frogger.Respawn();
            StopAllCoroutines();
            StartCoroutine(Timer(30));
        }

        private IEnumerator Timer(int duration)
        {
            _time = duration;

            while (_time > 0)
            {
                yield return new WaitForSeconds(1);
                _time--;
            }
            
            _frogger.Die();
        }

        public void AdvancedRow()
        {
            SetScore(_score + RowAdvancedPoints);
        }
        
        public void HomeOccupied()
        {
            _frogger.gameObject.SetActive(false);
            int bonus = _time * BonusMultiplier;
            
            SetScore(_score + bonus + HomeOccupiedPoints);

            if (Cleared())
            {
                SetScore(_score + LevelClearedPoints);
                Invoke(nameof(NewLevel), 1.0f);
            }
            else
            {
                Invoke(nameof(NewRound), 1.0f);
            }
        }

        private bool Cleared() => _homes.All(home => home.enabled);

        public void SetScore(int score)
        {
            _score = score;
        }

        public void SetLives(int lives)
        {
            _lives = lives;
        }
    }
}