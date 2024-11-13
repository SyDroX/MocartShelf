using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.up * (Time.deltaTime * _rotationSpeed));
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }
}