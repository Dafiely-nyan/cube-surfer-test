using System;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameContext : MonoBehaviour
{
    public ScenesLoader ScenesLoader;

    public static bool Paused { get; private set; }
    
    public static void SetPause(bool paused)
    {
        Time.timeScale = paused ? 0 : 1;
        Paused = paused;
    }

    public static GameContext Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            QualitySettings.vSyncCount = 0;
        }
    }
}
