using UnityEngine;

namespace BitWave_Labs.Frogger.Scripts
{
    public class MoveCycle : MonoBehaviour
    {
        [SerializeField] private Vector2 direction = Vector2.right;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private int size = 1;

        private Vector3 _leftEdge;
        private Vector3 _rightEdge;

        private void Start()
        {
            if (!Camera.main)
                return;
            
            _leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            _rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
        }

        private void Update()
        {
            if (direction.x > 0 && (transform.position.x - size) > _rightEdge.x)
            {
                Vector3 position = transform.position;
                position.x = _leftEdge.x - size;
                transform.position = position;
            }
            else if (direction.x < 0 && (transform.position.x + size) < _leftEdge.x)
            {
                Vector3 position = transform.position;
                position.x = _rightEdge.x + size;
                transform.position = position;
                
            }
            else
            {
                transform.Translate(direction * (speed * Time.deltaTime));
            }
        }
    }
}