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

    GameManager _gameManager;

    SpawnManager _spawnManager;

    [SerializeField]
    AudioClip _explosionSFX, _shieldBreakingSFX;

    [SerializeField]
    GameObject _newExplosionPrefab;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _anim = GetComponent<Animator>();


        if (enemy.enemyType == Enemy.EnemyType.Normal)
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                Debug.LogError("The Audio Source is NULL!");
            }
            else
            {
                _audioSource.clip = _explosionSFX;
            }
        }
            

        

    }

    public void Damage()
    {
        if (enemy.enemyType != Enemy.EnemyType.Normal)
        {
            Instantiate(_newExplosionPrefab, transform.position, Quaternion.identity);

            GetComponent<Collider2D>().enabled = false;

            enemy.isDestroyed = true;

            _gameManager.AddScore(_scoreIncrement);

            _spawnManager.destroyedEnemyNumber++;

            Destroy(gameObject, 0.2f);
        }
        else
        {
            _anim.SetTrigger("OnEnemyDestroy");

            PlayExplosionSFX();

            GetComponent<Collider2D>().enabled = false;

            enemy.isDestroyed = true;

            _gameManager.AddScore(_scoreIncrement);

            _spawnManager.destroyedEnemyNumber++;

            Destroy(gameObject, 2.7f);
        }
    }

    public void PlayShieldBreakingSFX()
    {
        _audioSource.clip = _shieldBreakingSFX;

        _audioSource.Play();
    }

    public void PlayExplosionSFX()
    {
        _audioSource.clip = _explosionSFX;

        _audioSource.Play();
    }

}
