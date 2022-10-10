using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Enemy enemy;

    Animator _anim;

    [SerializeField]
    int _scoreIncrement = 10;

    public int ScoreIncrement => _scoreIncrement;

    protected GameManager gameManager;

    protected SpawnManager spawnManager;

    [SerializeField]
    protected AudioClip explosionSFX, shieldBreakingSFX;

    [SerializeField]
    protected GameObject newExplosionPrefab;

    protected AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _anim = GetComponent<Animator>();


        if (enemy.enemyType == Enemy.EnemyType.Normal || enemy.enemyType == Enemy.EnemyType.Gunship)
        {
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
    }

    public void Damage()
    {
        if (enemy.enemyType != Enemy.EnemyType.Normal)
        {
            Instantiate(newExplosionPrefab, transform.position, Quaternion.identity);

            GetComponent<Collider2D>().enabled = false;

            enemy.isDestroyed = true;

            gameManager.AddScore(_scoreIncrement);

            spawnManager.destroyedEnemyNumber++;

            Destroy(gameObject, 0.2f);
        }
        else
        {
            _anim.SetTrigger("OnEnemyDestroy");

            PlayExplosionSFX();

            GetComponent<Collider2D>().enabled = false;

            enemy.isDestroyed = true;

            gameManager.AddScore(_scoreIncrement);

            spawnManager.destroyedEnemyNumber++;

            Destroy(gameObject, 2.7f);
        }
    }

    public virtual void PlayShieldBreakingSFX()
    {
        audioSource.clip = shieldBreakingSFX;

        audioSource.Play();
    }

    public virtual void PlayExplosionSFX()
    {
        audioSource.clip = explosionSFX;

        audioSource.Play();
    }

}
