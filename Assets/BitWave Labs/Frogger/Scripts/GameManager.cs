using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BitWave_Labs.Frogger.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject gameOverMenu;
        
        private Frogger _frogger;
        private Home[] _homes;
        private int _score;
        private int _lives;
        private int _time;
        private const int StartingLives = 3;
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
            gameOverMenu.SetActive(false);
            SetScore(0);
            SetLives(StartingLives);
            NewLevel();
        }

        private void NewLevel()
        {
            foreach (Home home in _homes)
            {
                home.enabled = false;
            }

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
            timeText.text = _time.ToString();

            while (_time > 0)
            {
                yield return new WaitForSeconds(1);
                _time--;
                timeText.text = _time.ToString();
            }
            
            _frogger.Die();
        }

        public void Died()
        {
            SetLives(_lives -1);
            
            if(_lives > 0)
                Invoke(nameof(Respawn), 1.0f);
            else
                Invoke(nameof(GameOver), 1.0f);
        }

        private void GameOver()
        {
            _frogger.gameObject.SetActive(false);
            gameOverMenu.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(PlayAgain());
        }

        // public void PlayAgain(InputAction.CallbackContext context)
        // {
        //     Debug.Log("PlayAgain");
        //     if(gameOverMenu.activeInHierarchy)
        //         NewGame();
        // }
        
        private IEnumerator PlayAgain()
        {
            bool playAgain = false;
        
            while (!playAgain)
            {
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                    playAgain = true;
                
                yield return null;
            }
            
            NewGame();
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
                SetLives(_lives + 1);
                Invoke(nameof(NewLevel), 1.0f);
            }
            else
            {
                Invoke(nameof(Respawn), 1.0f);
            }
        }

        private bool Cleared() => _homes.All(home => home.enabled);

        public void SetScore(int score)
        {
            _score = score;
            scoreText.text = score.ToString();
        }

        public void SetLives(int lives)
        {
            _lives = lives;
            livesText.text = lives.ToString();
        }
    }
}