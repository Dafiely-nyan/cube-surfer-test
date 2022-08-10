using UnityEngine;

public class CarRespawner : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private float _maxDistance;

    private BoxCollider _boxCollider;
    private Transform _transform;
    private Rigidbody _rigidbody;

    private Vector3 _resetPoint;
    
    private readonly Vector3 deltaDistance = Vector3.up * 0.1f;

    public void SetResetPoint(Vector3 resetPoint)
    {
        _resetPoint = resetPoint;
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SceneContext.OnGameplayRestart += RestartHandler;
    }

    private void RestartHandler()
    {
        ResetForces();
        
        _transform.position = SceneContext.Instance.LevelGenerator.StartPoint;
        _transform.rotation = Quaternion.Euler(0,0,0);
        
        _resetPoint = SceneContext.Instance.LevelGenerator.StartPoint;
    }

    private void LateUpdate()
    {
        if (!Physics.BoxCast(_boxCollider.bounds.center + deltaDistance, _transform.localScale / 2,
            -_transform.up, out RaycastHit hit, _transform.rotation, _maxDistance, _layerMask))
        {
            ResetToPosition();
        }
    }

    private void OnDestroy()
    {
        SceneContext.OnGameplayRestart -= RestartHandler;
    }

    void ResetToPosition()
    {
        ResetForces();
        
        _transform.position = _resetPoint;
        _transform.rotation = Quaternion.Euler(0,0,0);
    }

    void ResetForces()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
