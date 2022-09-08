using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    int _health = 3;

    Player _player;

    UIManager _uIManager;

    GameManager _gameManager;

    [SerializeField]
    GameObject _newExplosionPrefab;


    private void Start()
    {
        _player = gameObject.GetComponent<Player>();
        _uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();



        if (_uIManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }

        if(_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL!");
        }
        
        
    }

    public void Damage()
    {

        if (_player.isShielded)
        {
            _player.isShielded = false;

            _player.shield.SetActive(false);

            return;
        }

        _health--;

        _uIManager.UpadateLives(_health);

        if (_health < 1)
        {
            _uIManager.ShowGameOverUI();

            _gameManager.isGameOVer = true;

            _player.StopMoving();

            Instantiate(_newExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject, 0.3f);
        }

    }


}
