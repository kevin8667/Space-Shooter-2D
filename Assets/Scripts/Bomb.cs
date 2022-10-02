using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _explosion;

    [SerializeField]
    AudioClip _explosionSFX;

    AudioSource _audioSource;

    Enemy[] enemies;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL!");
        }
        else
        {
            _audioSource.clip = _explosionSFX;
        }

        Explosion();
    }

    private void Update()
    {

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && !enemy.isDestroyed)
            {
                enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, new Vector2(0, 0), 10f * Time.deltaTime);
            }
 
        }
        
    }

    void Explosion()
    {
        StartCoroutine(ChangeBGMVolume());

        _explosion.Play();

        enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            if (!enemy.isDestroyed)
            {
                enemy.speed = 0;

                enemy.GetComponent<Collider2D>().enabled = false;

                FindObjectOfType<GameManager>().AddScore(enemy.GetComponent<EnemyHealth>().ScoreIncrement);

                FindObjectOfType<SpawnManager>().destroyedEnemyNumber++;

                Destroy(enemy.gameObject, 1.3f);
            }
           
        }

        _audioSource.Play();

        Destroy(gameObject, 6f);

    }

    IEnumerator ChangeBGMVolume()
    {
        AudioSource BGM = GameObject.Find("BGMManager").GetComponent<AudioSource>();

        BGM.volume = 0.3f;

        yield return new WaitForSeconds(3.5f);

        BGM.volume = 1;
    }

}
