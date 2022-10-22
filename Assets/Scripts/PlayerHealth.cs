using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int health = 3;

    public int shieldHealth = 0;

    bool _isInvincible, _isDestroyed;

    Player _player;

    UIManager _uIManager;

    GameManager _gameManager;

    CameraManager _cameraManager;

    [SerializeField]
    GameObject _newExplosionPrefab;

    Color _shieldColor;

    [SerializeField]
    AudioClip _shieldBreakingSFX, _damageSFX;

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

        if (!_isInvincible)
        {
            _isInvincible = true;

            health--;

            _cameraManager.CamaraShake();

            StartCoroutine(DamageSequence());
            StartCoroutine(DamageSequenceTimer());
        }
        
        if (health >= 0)
        {
            _uIManager.UpadateLives(health);
        }

        if (health < 1 && !_isDestroyed)
        {
            _isDestroyed = true;

            _gameManager.LoseTheGame();

            _player.StopMoving();

            Instantiate(_newExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject, 0.3f);
        }

    }


    IEnumerator DamageSequence()
    {
        AudioSource.PlayClipAtPoint(_damageSFX, GameObject.Find("Main Camera").transform.position, 1f);

        while (_isInvincible)
        {
            GetComponent<SpriteRenderer>().enabled = false;

            yield return new WaitForSeconds(0.1f);

            GetComponent<SpriteRenderer>().enabled = true;

            yield return new WaitForSeconds(0.1f);
        }

    }

    IEnumerator DamageSequenceTimer()
    {
        _isInvincible = true;

        yield return new WaitForSeconds(1f);

        _isInvincible = false;

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
