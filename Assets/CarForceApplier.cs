using UnityEngine;

public class CarForceApplier : MonoBehaviour
{
    [SerializeField]
    private float _maxForce;
    [SerializeField]
    private float _minMagnitude = 0.05f;
    [SerializeField]
    private float _maxSpeedToTriggerDisplay = 0.1f;

    private ForceDirectionVisualizer _forceDirectionVisualizer;
    private Rigidbody _rigidbody;
    private CarRespawner _carRespawner;
    private Transform _transform;

    private Vector3 _currentDelta;
    private Vector3 _startDrag;
    private float _magnitude;
    private bool _facingCorrectDirection;
    
    private void Awake()
    {
        _forceDirectionVisualizer = GetComponent<ForceDirectionVisualizer>();
        _rigidbody = GetComponent<Rigidbody>();
        _carRespawner = GetComponent<CarRespawner>();
        _transform = transform;
    }

    private void Update()
    {
        if (GameContext.Paused || _rigidbody.velocity.magnitude > _maxSpeedToTriggerDisplay) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _startDrag = Input.mousePosition;
            _forceDirectionVisualizer.Show();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _currentDelta = Input.mousePosition - _startDrag;
            
            Vector3 right = _transform.right;
            
            Vector3 forward2D = new Vector3(-right.z, right.x);
            
            float verticalProjecton = Vector3.Dot(_currentDelta, Vector3.down);
            _magnitude = Mathf.Clamp01(Mathf.Abs(verticalProjecton) / (Utility.ScreenSize.y / 2));

            _facingCorrectDirection = Vector3.Dot(-_currentDelta, forward2D) >= 0;
            
            SceneContext.Instance.UiController.SetTensionStrength(_magnitude);
            _forceDirectionVisualizer.SetDirection(_currentDelta, _magnitude, _facingCorrectDirection);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_magnitude >= _minMagnitude && _facingCorrectDirection)
                ApplyForce(_magnitude);
            
            _startDrag = Vector3.zero;
            _currentDelta = Vector3.zero;
            _magnitude = 0;
            
            SceneContext.Instance.UiController.SetTensionStrength(_magnitude);
            _forceDirectionVisualizer.Hide();
        }
    }

    void ApplyForce(float percent)
    {
        float angle = Vector2.SignedAngle(-_currentDelta, Vector2.up);

        Vector3 force = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;

        _carRespawner.SetResetPoint(_transform.position);
        _rigidbody.AddForce(force * (_maxForce * percent), ForceMode.Impulse);
        _transform.eulerAngles = new Vector3(0, angle,  0);
    }
}
