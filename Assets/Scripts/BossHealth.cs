using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossHealth : EnemyHealth
{
    [SerializeField]
    int _bossHp;

    Boss _boss;
    
    void Start()
    {
        _boss = GetComponent<Boss>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL!");
        }
        else
        {
            audioSource.clip = explosionSFX;
        }
    }

    public void BossDamage(int damageAmount)
    {
        _bossHp -= damageAmount;

        if (_bossHp < 1 && !_boss.isDestroyed)
        {
            _boss.isDestroyed = true;

            gameManager.WinTheGame();

            Instantiate(newExplosionPrefab, transform.position, Quaternion.identity);

            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 0.3f);
        }
    }

}
