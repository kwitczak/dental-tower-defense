using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {


    public SceneFader sceneFader;
    public string menuSceneName = "MainMenu";

    public void Retry ()
    {
        // Restart currently loaded scene/level
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu ()
    {
        sceneFader.FadeTo(menuSceneName);
    }
}
