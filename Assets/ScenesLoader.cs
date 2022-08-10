using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    private bool _loading;
    
    public void LoadScene(GameScene scene)
    {
        if (!_loading)
        {
            StartCoroutine(LoadSceneAsync(scene));
        }
    }

    IEnumerator LoadSceneAsync(GameScene scene)
    {
        _loading = true;

        yield return SceneManager.LoadSceneAsync((int) scene);

        _loading = false;
    }
}
