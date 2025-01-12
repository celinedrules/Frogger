using System.Collections;
using UnityEngine;

namespace BitWave_Labs.Frogger.Scripts
{
    public class Frogger : MonoBehaviour
    {
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite leapSprite;
        [SerializeField] private Sprite deathSprite;
        
        private SpriteRenderer _spriteRenderer;

        private void Start() => _spriteRenderer = GetComponent<SpriteRenderer>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Rotate(0, 0, 0);
                Move(Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Rotate(0, 0, 180);
                Move(Vector3.down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Rotate(0, 0, 90);
                Move(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Rotate(0, 1, -90);
                Move(Vector3.right);
            }
        }

        private void Move(Vector3 direction)
        {
            Vector3 destination = transform.position + direction;

            if(Physics2D.OverlapBox(destination, Vector2.zero, 0.0f, LayerMask.GetMask("Barrier")))
                return;
            
            Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0.0f, LayerMask.GetMask("Platform"));
            transform.SetParent(platform ? platform.transform : null);

            Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0.0f, LayerMask.GetMask("Obstacle"));
            
            if(!platform && obstacle) 
            {
                transform.position = destination;
                Die();
            }
            else
            {
                StartCoroutine(Leap(destination));
            }
        }

        private void Rotate(float x, float y, float z)
        {
            transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
        }

        private IEnumerator Leap(Vector3 destination)
        {
            Vector3 startPosition = transform.position;
            float elapsed = 0;
            const float duration = 0.125f;

            _spriteRenderer.sprite = leapSprite;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, destination, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            transform.position = destination;
            _spriteRenderer.sprite = idleSprite;
        }

        private void Die()
        {
            transform.rotation = Quaternion.identity;
            _spriteRenderer.sprite = deathSprite;
            enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (transform.parent)
                return;
            
            if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                Die();
        }
    }
}