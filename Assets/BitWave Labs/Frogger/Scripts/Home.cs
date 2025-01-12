using UnityEngine;

namespace BitWave_Labs.Frogger.Scripts
{
    public class Home : MonoBehaviour
    {
        [SerializeField] private GameObject frog;

        private void OnEnable() => frog.SetActive(true);
        private void OnDisable() => frog.SetActive(false);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            enabled = true;
            Frogger frogger = other.GetComponent<Frogger>();
            frogger.gameObject.SetActive(false);
            frogger.Invoke(nameof(frogger.Respawn), 1.0f);
        }
    }
}