using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    Image _lifeIndicator;

    [SerializeField]
    Sprite[] _lifeSprites;

    [SerializeField]
    Image _bombIndicator;

    [SerializeField]
    Sprite[] _bombSprites;

    [SerializeField]
    TMP_Text _score, _ammo, _wave;

    [SerializeField]
    TMP_Text _gameOverText, _gameOverTips;

    [SerializeField]
    TMP_Text _warningText;

    [SerializeField]
    TMP_Text _winText;

    [SerializeField]
    GameObject _pauseMenu;

    [SerializeField]
    GameObject _bossHp;

    [SerializeField]
    Image _reloadImage, _fuelImage, _bossHpBar;

    [SerializeField]
    RectTransform _canvasRect;
    Vector2 _screenPoint;
    Vector2 _canvasPos;

    public Image ReloadImage => _reloadImage;

    public Image FuelImage => _fuelImage;

    public Image BossHpBar => _bossHpBar;

    [SerializeField]
    Player _player;

    Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        _score.text = "Score: " + _gameManager.score;

        _wave.color = new Color(1, 0, 0, 0);

        _warningText.color = new Color(1, 0, 0, 0);
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

    public void UpdateWaveText(int waveNumber)
    {
        _wave.color = new Color(1, 0, 0, 1);

        _wave.text = "Wave:" + waveNumber;

        StartCoroutine(WaveTextFadeOut(1f));
    }

    IEnumerator WaveTextFadeOut(float duration)
    {
        Color alpha = _wave.color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _wave.color = Color.Lerp(alpha, new Color(1, 0, 0, 0), elapsedTime / duration);
            yield return null;
        }
    }

    public void UpdateBombs(int bombCount)
    {
        _bombIndicator.sprite = _bombSprites[bombCount];
    }

    public void ShowGameOverUI()
    {
        _gameOverText.gameObject.SetActive(true);
        _gameOverTips.gameObject.SetActive(true);
    }

    public void ShowWinningUI()
    {
        _winText.gameObject.SetActive(true);
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

    public void CalculateReloadImage(Vector2 position)
    {
        //get the position of the Player in screen space
        _screenPoint = Camera.main.WorldToScreenPoint(position);

        //transfrom the Player's position in screen space to the position in the local space of a RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, _screenPoint, null, out _canvasPos);

        //move the reaload image to the position of the corresponding Player's position on the canvas
        _reloadImage.transform.localPosition = _canvasPos;
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
    
    public void StartWarning()
    {
        StartCoroutine(WarningSequence());
    }

    IEnumerator WarningSequence()
    {
        float elapsedTime = 0;

        while(elapsedTime < 4f)
        {
            elapsedTime += Time.deltaTime;

            _warningText.color = Color.Lerp(new Color(1, 0, 0, 0), new Color(1, 0, 0, 1), MathF.Sin(elapsedTime * 4f));

            yield return null;
        }
    }

    public void ShowBossHP()
    {
        _bossHp.SetActive(true);
    }

}
