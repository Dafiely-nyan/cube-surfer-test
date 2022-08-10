using UnityEngine;

public class ForceDirectionVisualizer : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private float _maxSize;

    private Transform _transform;
    private RectTransform _canvasRt;

    private void Awake()
    {
        _transform = transform;
        _canvasRt = _canvas.GetComponent<RectTransform>();
    }

    public void SetDirection(Vector3 inputDirecton, float magnitude, bool display)
    {
        if (!display)
            Hide();
        else Show();
        
        _canvas.transform.position = _transform.position;
        
        float angle = Vector2.SignedAngle(-inputDirecton, Vector3.up);
        
        _canvas.transform.localEulerAngles = new Vector3(90,  angle, 0);
        _canvasRt.sizeDelta = new Vector2(_maxSize * magnitude, _canvasRt.sizeDelta.y);
    }

    public void Hide()
    {
        if (_canvas.gameObject.activeSelf)
            _canvas.gameObject.SetActive(false);
    }

    public void Show()
    {
        if (!_canvas.gameObject.activeSelf)
            _canvas.gameObject.SetActive(true);
    }
}
