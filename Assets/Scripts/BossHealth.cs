using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossHealth : EnemyHealth
{
    [SerializeField]
    float _bossHp;

    float _bossMaxHp;

    Boss _boss;

    UIManager _uIManager;
    
    void Start()
    {
        _boss = GetComponent<Boss>();

        gameManager = FindObjectOfType<GameManager>();

        spawnManager = FindObjectOfType<SpawnManager>();

        _uIManager = FindObjectOfType<UIManager>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL!");
        }
        else
        {
            audioSource.clip = explosionSFX;
        }

        _bossMaxHp = _bossHp;
    }

    public void BossDamage(float damageAmount)
    {
        _bossHp -= damageAmount;

        _uIManager.UpdateImageFillAmount(_uIManager.BossHpBar, _bossHp / _bossMaxHp);

        if (_bossHp <= 0 && !_boss.isDestroyed)
        {
            _boss.isDestroyed = true;

            gameManager.WinTheGame();

            Instantiate(newExplosionPrefab, transform.position, Quaternion.identity);

            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 0.3f);
        }
    }

}
