using UnityEngine;

public class SplashManager : MonoBehaviour
{
    public static SplashManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void FinishSplash()
    {
        LoadingManager.Instance.LoadScene(SCENE_INDEX.Gameplay, () => { MainScreen.Show(); });
    }
}