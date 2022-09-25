using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int health = 3;

    public int shieldHealth = 0;

    Player _player;

    UIManager _uIManager;

    GameManager _gameManager;

    CameraManager _cameraManager;

    [SerializeField]
    GameObject _newExplosionPrefab;

    Color _shieldColor;

    [SerializeField]
    AudioClip _shieldBreakingSFX;

    private void Start()
    {
        _player = gameObject.GetComponent<Player>();

        _uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        _shieldColor = _player.shield.GetComponent<SpriteRenderer>().color;

        if (_uIManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }

        if(_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL!");
        }

        if (_shieldColor == null)
        {
            Debug.LogError("The Shield is NULL!");
        }
        
    }

    public void Damage()
    {

        if (_player.isShielded)
        {
            ShieldDamage();

            return;
        }

        health--;
        if(health >= 0)
        {
            _uIManager.UpadateLives(health);
        }

        _cameraManager.CamaraShake();

        if (health < 1)
        {
            _uIManager.ShowGameOverUI();

            _gameManager.isGameOVer = true;

            _player.StopMoving();

            Instantiate(_newExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject, 0.3f);
        }

    }


    void ShieldDamage()
    {
        if (shieldHealth > 0)
        {
            shieldHealth -= 1;

            SetShield();
        }

        
    }

    public void ShieldRecover()
    {
        if (shieldHealth < 3)
        {
            shieldHealth += 1;

            SetShield();
        }
    }

    public void SetShield()
    {
        switch (shieldHealth)
        {
            case 3:
                _player.shield.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 2:
                _player.shield.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 1:
                _player.shield.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 0:
                AudioSource.PlayClipAtPoint(_shieldBreakingSFX, GameObject.Find("Main Camera").transform.position, 0.6f);
                _player.isShielded = false;
                _player.shield.SetActive(false);
                break;
        }
    }

}
