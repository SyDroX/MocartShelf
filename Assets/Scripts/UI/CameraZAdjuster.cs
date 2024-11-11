using UnityEngine;

namespace UI
{
    public class CameraZAdjuster : MonoBehaviour
    {
        [SerializeField] private float     _landscapePositionZ;
        [SerializeField] private float     _portraitPositionZ;
        [SerializeField] private Transform _cameraTransform;

        private void Start()
        {
            Vector3 position = _cameraTransform.position;

            if (Screen.width > Screen.height)
            {
                position.z = _landscapePositionZ;
            }
            else
            {
                position.y++;
                position.z = _portraitPositionZ;
            }

            _cameraTransform.position = position;
        }
    }
}