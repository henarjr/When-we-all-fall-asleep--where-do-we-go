using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinuousMusic : MonoBehaviour
{
    private static ContinuousMusic instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name != "IntroScene" && scene.name != "StartScreen")
        {
            Destroy(this.gameObject);
        }
    }
}