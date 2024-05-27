using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button LoadGameButton;

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");

        SaveManager.Instance.ActivateLoadingScreen();
        SaveManager.Instance.isLoading = true;

        SaveManager.Instance.startGameCo();

    }

    public void ExitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }
}
