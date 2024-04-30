using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField]private float _zoomSpeed = 14f;
    [SerializeField] private float _interpolationSpeed = 5f;
    [SerializeField] private float _minOrthoSize = 10f;
    [SerializeField] private float _maxOrthoSize = 30f;
    [SerializeField] private float _targetOrthoSize;

    private void Start()
    {
        // Initialize the target ortho size to the camera's current ortho size
        _targetOrthoSize = virtualCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        // Adjust the target ortho size based on scroll wheel input
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            _targetOrthoSize -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
            _targetOrthoSize = Mathf.Clamp(_targetOrthoSize, _minOrthoSize, _maxOrthoSize);
        }

        // Smoothly interpolate the camera's ortho size towards the target ortho size
        virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(virtualCamera.m_Lens.OrthographicSize, _targetOrthoSize, _interpolationSpeed * Time.deltaTime);
    }
}
