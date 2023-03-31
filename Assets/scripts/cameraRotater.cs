using UnityEngine;

public class cameraRotater : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Transform _rotator;

    private void Start()
    {
        _rotator = GetComponent<Transform>();
    }

    private void Update()
    {
        _rotator.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}
