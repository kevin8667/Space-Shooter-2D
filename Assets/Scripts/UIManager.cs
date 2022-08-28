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
    TMP_Text _score;

    [SerializeField]
    TMP_Text _gameOverText;
        
   

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

    public void UpadateLives(int playerLives)
    {
        _lifeIndicator.sprite = _lifeSprites[playerLives];

    }

    public void ShowGameOverUI()
    {
        _gameOverText.gameObject.SetActive(true);
    }
}
