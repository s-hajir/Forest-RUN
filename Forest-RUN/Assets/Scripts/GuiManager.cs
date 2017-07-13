using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {
    public GameObject mainmenu;
    public GameObject pausemenu;
    public GameObject deathmenu;
    public GameObject gameplayUI;
    public GameObject gameplayScore;
    public GameObject pausemenuScore;
    public GameObject deathmenuScore;
    private bool gameplayActive = false;

    void Start () {
        Time.timeScale = 0;
        //show MainMenu
        showMainMenu();
    }
    private void Update()
    {
        if (gameplayActive)
        {
            gameplayScore.GetComponent<Text>().text = "Coins: " + GameStateManager.coins;
        }
        if (GameStateManager.lifes == 0)
        {
            showDeathMenu();
        }
    }

    public void run()   //run or resume
    {
        gameplayActive = true;
        Time.timeScale = 1;
        gameplayUI.SetActive(true);
        mainmenu.SetActive(false);
        pausemenu.SetActive(false);
        deathmenu.SetActive(false);
    }
    public void showMainMenu()
    {
        gameplayActive = false;
        Time.timeScale = 0;
        gameplayUI.SetActive(false);
        mainmenu.SetActive(true);
        pausemenu.SetActive(false);
        deathmenu.SetActive(false);
    }
    public void showPauseMenu()
    {
        gameplayActive = false;
        Time.timeScale = 0;
        pausemenuScore.GetComponent<Text>().text = "Coins : "+GameStateManager.coins;
        gameplayUI.SetActive(false);
        mainmenu.SetActive(false);
        pausemenu.SetActive(true);
        deathmenu.SetActive(false);
    }
    public void showDeathMenu()
    {
        gameplayActive = false;
        Time.timeScale = 0;
        deathmenuScore.GetComponent<Text>().text = "GAME OVER - Coins : " + GameStateManager.coins;
        gameplayUI.SetActive(false);
        mainmenu.SetActive(false);
        pausemenu.SetActive(false);
        deathmenu.SetActive(true);
    }

    public void runAgain()   //button in deathMenu
    {
        //restart scene
        GameStateManager.lifes = 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
