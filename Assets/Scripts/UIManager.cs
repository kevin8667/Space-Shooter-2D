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

    [SerializeField]
    Image reloadImage, fuelImage;

    public Image ReloadImage => reloadImage;

    public Image FuelImage => fuelImage;

    [SerializeField]
    Player _player;

    Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        _score.text = "Score: " + _gameManager.score;
    }

    public void UpdateScore()
    {
        _score.text = "Score: "+ _gameManager.score;
    }

    public void UpdateAmmo(int ammo)
    {
        _ammo.text = "Ammo: " + ammo + "/" + _player.maxAmmo;
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

    public void UpdateImageFillAmount(Image image, float ratio)
    {
        image.fillAmount = ratio;
    }

    public void StartImageFlicker(Image image, float interval, bool trigger)
    {
        _coroutine = StartCoroutine(ImageFlicker(image, interval, trigger));
    }

    public void StoprImageFlicker()
    {
        StopCoroutine(_coroutine);
    }

    IEnumerator ImageFlicker(Image image, float interval, bool trigger) 
    {
        while (trigger)
        {
            image.enabled = false;

            yield return new WaitForSeconds(interval);

            image.enabled = true;

            yield return new WaitForSeconds(interval);

            image.enabled = false;

        }

    }

}
