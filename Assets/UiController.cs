using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _totalTimeText;
    [SerializeField]
    private TextMeshProUGUI _completionText;
    [SerializeField]
    private TextMeshProUGUI _tensionStrengthText;

    [SerializeField]
    private Button _continueButton;

    [SerializeField]
    private Canvas _menuCanvas;
    
    public void SetTensionStrength(float value)
    {
        _tensionStrengthText.text = $"Сила натяжения: {(int)(value * 100)}%";
    }

    void SetTotalTime()
    {
        if (Time.timeScale > 0)
            _totalTimeText.text = $"Время: {SceneContext.PlayTime:0.0000}";
    }

    public void SetCompletion(float value)
    {
        _completionText.text = $"Пройдено: {(int)(value * 100)}%";
    }

    public void OpenMenu()
    {
        SetMenuWindowActive(true);
        
        GameContext.SetPause(true);
    }

    public void CloseMenu()
    {
        SetMenuWindowActive(false);
        
        GameContext.SetPause(false);
    }

    public void Exit()
    {
        GameContext.SetPause(false);
        
        PlayerPrefs.SetInt("tries_amount", PlayerPrefs.GetInt("tries_amount", 0) + 1);
        float currentBest = PlayerPrefs.GetFloat("best_time", Mathf.Infinity);
        
        if (SceneContext.PlayTime < currentBest && SceneContext.IsGameOver)
            PlayerPrefs.SetFloat("best_time", SceneContext.PlayTime);

        GameContext.Instance.ScenesLoader.LoadScene(GameScene.Menu);
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("tries_amount", PlayerPrefs.GetInt("tries_amount", 0) + 1);
        SceneContext.Restart();
        CloseMenu();
    }

    void SetMenuWindowActive(bool active)
    {
        if (active && !_menuCanvas.gameObject.activeSelf)
            _menuCanvas.gameObject.SetActive(true);
        else if (!active && _menuCanvas.gameObject.activeSelf)
            _menuCanvas.gameObject.SetActive(false);

        _continueButton.interactable = !SceneContext.IsGameOver;;
    }
    
    private void Update()
    {
        SetTotalTime();
    }

    private void Start()
    {
        SceneContext.OnGameOver += GameOverHandler;
    }

    private void OnDestroy()
    {
        SceneContext.OnGameOver -= GameOverHandler;
    }

    private void GameOverHandler()
    {
        OpenMenu();
    }
}
