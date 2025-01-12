using UnityEngine;

namespace BitWave_Labs.Frogger.Scripts
{
    public class Frogger : MonoBehaviour
    {
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
            transform.position += direction;
        }

        private void Rotate(float x, float y, float z)
        {
            transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
        }
    }
}