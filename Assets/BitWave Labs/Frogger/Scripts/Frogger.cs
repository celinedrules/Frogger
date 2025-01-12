using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BitWave_Labs.Frogger.Scripts
{
    public class Frogger : MonoBehaviour
    {
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite leapSprite;
        [SerializeField] private Sprite deathSprite;
        
        private SpriteRenderer _spriteRenderer;

        private void Start() => _spriteRenderer = GetComponent<SpriteRenderer>();

        [UsedImplicitly]
        public void Move(InputAction.CallbackContext context)
        {
            if(!context.performed)
                return;
            
            Vector2 destination = (Vector2)transform.position + context.ReadValue<Vector2>();

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
        
        [UsedImplicitly]
        public void Rotate(InputAction.CallbackContext context)
        {
            if(!context.performed)
                return;
            
            Vector2 input = context.ReadValue<Vector2>();

            transform.rotation = input.y switch
            {
                > 0 => Quaternion.Euler(0, 0, 0),
                < 0 => Quaternion.Euler(0, 0, 180),
                _ => input.x switch
                {
                    > 0 => Quaternion.Euler(0, 0, -90),
                    < 0 => Quaternion.Euler(0, 0, 90),
                    _ => transform.rotation
                }
            };
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