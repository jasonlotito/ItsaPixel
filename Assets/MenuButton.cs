using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {
    public GameObject creditsWindow;

    public void OnButtonPress()
    {
        StartGame();
    }

    public void OnCloseCreditsWindow()
    {
        creditsWindow.SetActive(false);
    }

    public void OnCreditsPress()
    {
        creditsWindow.SetActive(true);
    }

    private static void StartGame()
    {
        SceneManager.LoadScene("Playground");
    }

    
}
