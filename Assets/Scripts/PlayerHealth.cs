using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    int _health = 3;

    Player _player;

    UIManager _UIManager;

    private void Start()
    {
        _player = gameObject.GetComponent<Player>();

        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
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
            Destroy(gameObject);
        }

    }


}
