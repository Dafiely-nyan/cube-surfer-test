using System;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class SceneContext : MonoBehaviour
{
    public UiController UiController;
    public LevelGenerator LevelGenerator;

    public static event Action OnGameOver;
    public static event Action OnGameplayRestart;

    public static float PlayTime { get; private set; }
    public static bool IsGameOver { get; private set; }

    public static void SetGameOver()
    {
        IsGameOver = true;
        OnGameOver?.Invoke();
    }

    public static void Restart()
    {
        IsGameOver = false;
        PlayTime = 0;
        
        OnGameplayRestart?.Invoke();
    }

    private void Update()
    {
        PlayTime += Time.deltaTime;
    }

    public static SceneContext Instance;
    private void Awake()
    {
        Instance = this;
        Restart();
    }
}
