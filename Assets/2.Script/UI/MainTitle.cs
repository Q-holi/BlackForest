using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    public void Start_Game()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void Main_Title()
    {
        SceneManager.LoadScene("MainTitle");
    }

    public void Option()
    {
        SceneManager.LoadScene("Option");
    }

    public void Continue()
    {
        SceneManager.LoadScene("MainGame");
        Time.timeScale = 1;
    }

    public void GameExit()
    {
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
