using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;

    public bool isGameOVer;

    [SerializeField]
    UIManager _uIManager;

    [SerializeField]
    AudioClip _warningSFX;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        score = 0;

        isGameOVer = false;

        Time.timeScale = 1;

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource.GetComponent<AudioSource>()!= null)
        {
            _audioSource.clip = _warningSFX;
        }

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
        _uIManager.UpdateScore();
    }

    public void LoadMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void WinTheGame()
    {
        StartCoroutine(WinningSequence());
    }

    IEnumerator WinningSequence()
    {
        yield return new WaitForSeconds(2.5f);

        isGameOVer = true;

        _uIManager.ShowWinningUI();

        Time.timeScale = 0;
    }

    public void TogglePause()
    {
        _uIManager.TogglePauseMenu();

        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }

    public void WarningSequence()
    {
        _uIManager.StartWarning();

        _audioSource.Play();
    }
    
}
