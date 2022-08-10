using System;
using TMPro;
using UnityEngine;

public class MenuUiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _bestTimeText;
    [SerializeField]
    private TextMeshProUGUI _triesAmounttext;

    [SerializeField]
    private Canvas _leaderboardsCanvas;

    private void Start()
    {
        float currentBestTime = PlayerPrefs.GetFloat("best_time", Mathf.Infinity);
        
        _bestTimeText.text = float.IsInfinity(currentBestTime) ? $"Лучшее время: -" :
            $"Лучшее время: {PlayerPrefs.GetFloat("best_time"):0.0000}";

        _triesAmounttext.text = $"Количество попыток: {PlayerPrefs.GetInt("tries_amount", 0)}";
    }

    public void Play()
    {
        GameContext.Instance.ScenesLoader.LoadScene(GameScene.Gameplay);
    }

    public void ShowRating()
    {
        _leaderboardsCanvas.gameObject.SetActive(true);
    }

    public void HideRating()
    {
        _leaderboardsCanvas.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
