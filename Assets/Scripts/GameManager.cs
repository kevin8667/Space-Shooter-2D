using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;

    public bool isGameOVer;

    [SerializeField]
    UIManager _UIManager;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        isGameOVer = false;

        Time.timeScale = 1;


    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOVer)
        {
            TogglePause();

        }else if (Input.GetKeyDown(KeyCode.Escape) && isGameOVer)
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.R) && isGameOVer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int addValue)
    {
        score += addValue;
        _UIManager.UpdateScore();
    }

    public void LoadMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TogglePause()
    {
        _UIManager.TogglePauseMenu();

        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }
    
}
