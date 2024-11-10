using UnityEngine;

namespace DefaultNamespace
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 1f;
        
        private void Update()
        {
            transform.Rotate(Vector3.up * (Time.deltaTime * _rotationSpeed));
        }
    }
}