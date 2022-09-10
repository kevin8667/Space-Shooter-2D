using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    Image _lifeIndicator;

    [SerializeField]
    Sprite[] _lifeSprites;

    [SerializeField]
    TMP_Text _score, _ammo;

    [SerializeField]
    TMP_Text _gameOverText, _gameOverTips;


    [SerializeField]
    GameObject _pauseMenu;
   

    // Start is called before the first frame update
    void Start()
    {
        _score.text = "Score: " + _gameManager.score;

        //_lifeIndicator.sprite = _lifeSprites[3];
    }

    public void UpdateScore()
    {
        _score.text = "Score: "+ _gameManager.score;
    }

    public void UpdateAmmo(int ammo)
    {
        _ammo.text = "Ammo: " + ammo;
    }

    public void UpadateLives(int playerLives)
    {
        _lifeIndicator.sprite = _lifeSprites[playerLives];

    }

    public void ShowGameOverUI()
    {
        _gameOverText.gameObject.SetActive(true);
        _gameOverTips.gameObject.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeInHierarchy);
    }
}
