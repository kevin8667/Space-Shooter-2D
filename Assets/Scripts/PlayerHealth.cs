using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    int _health = 3;

    Player _player;

    UIManager _UIManager;

    [SerializeField]
    GameObject _newExplosionPrefab;



    private void Start()
    {
        _player = gameObject.GetComponent<Player>();
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        

        if (_UIManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
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

        _UIManager.UpadateLives(_health);

        if (_health < 1)
        {
            _UIManager.ShowGameOverUI();

            _player.StopMoving();

            Instantiate(_newExplosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject, 0.3f);
        }

    }


}
