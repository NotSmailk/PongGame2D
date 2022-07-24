using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadPvPGame()
    {
        SceneManager.LoadScene("PlayerVsPlayer");
    }

    public void LoadPvEGame()
    {
        SceneManager.LoadScene("GameAgainstBot");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
