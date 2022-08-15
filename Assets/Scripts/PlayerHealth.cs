using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    int _Health = 3;

    Player _player;

    private void Start()
    {
        _player = gameObject.GetComponent<Player>();
    }

    public void Damage()
    {

        if (_player.isShielded)
        {
            _player.isShielded = false;

            _player.shield.SetActive(false);

            return;
        }

        _Health--;

        if (_Health < 1)
        {
            Destroy(gameObject);
        }

    }


}
