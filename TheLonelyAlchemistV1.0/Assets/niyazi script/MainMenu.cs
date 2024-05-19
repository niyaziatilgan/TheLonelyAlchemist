using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button LoadGameButton;

    private void Start()
    {
        LoadGameButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.StartLoadedGame();
        });
    }
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }
}
