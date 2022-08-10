using UnityEngine;

public class CarFollower : MonoBehaviour
{
    [SerializeField]
    private Transform _carTransform;
    [SerializeField]
    private float _distance;
    [SerializeField]
    private float _angle = 45f;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }
    
    private void LateUpdate()
    {
        Vector3 carPos = _carTransform.position;
        _transform.position = new Vector3(carPos.x - _distance * Mathf.Cos(_angle * Mathf.Deg2Rad),
            carPos.y + _distance * Mathf.Sin(_angle * Mathf.Deg2Rad), carPos.z);
        _transform.LookAt(_carTransform);
    }
}
