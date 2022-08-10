using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Camera MainCamera { get; private set; }
    public static Vector2 ScreenSize { get => new Vector2(MainCamera.pixelWidth, MainCamera.pixelHeight); }

    private void Awake()
    {
        MainCamera = Camera.main;
    }
}
