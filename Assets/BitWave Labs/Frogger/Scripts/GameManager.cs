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

        private void Start()
        {
            _frogger = FindAnyObjectByType<Frogger>();
            _homes = FindObjectsByType<Home>(FindObjectsSortMode.None);
        }

        private void NewGame()
        {
            SetScore(0);
            SetLives(3);
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
            _frogger.Respawn();
        }

        public void HomeOccupied()
        {
            _frogger.gameObject.SetActive(false);
            Invoke(Cleared() ? nameof(NewLevel) : nameof(NewRound), 1.0f);
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