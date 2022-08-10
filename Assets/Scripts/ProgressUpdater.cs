using UnityEngine;

public class ProgressUpdater : MonoBehaviour
{
    private UiController _uiController;
    private LevelGenerator _levelGenerator;

    private Transform _transform;

    private float _initialLength;

    [SerializeField]
    private float _finishLeniency = 0.05f;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _uiController = SceneContext.Instance.UiController;
        _levelGenerator = SceneContext.Instance.LevelGenerator;

        _initialLength = (_levelGenerator.LastPoint - _levelGenerator.StartPoint).magnitude;
    }

    private void LateUpdate()
    {
        if (SceneContext.IsGameOver || GameContext.Paused) return;
        
        float currentLength = Vector3.Distance(_transform.position, _levelGenerator.LastPoint);
        float completion = 1 - Mathf.Clamp01(currentLength / _initialLength);
        
        _uiController.SetCompletion(completion);

        if (completion + _finishLeniency >= 1)
        {
            SceneContext.SetGameOver();
        }
    }
}
