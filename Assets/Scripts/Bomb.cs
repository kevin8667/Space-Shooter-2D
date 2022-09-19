using System.Collections;
using System.Collections.Generic;
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

        foreach (Enemy e in enemies)
        {
            if (e != null)
            {
                e.transform.position = Vector2.MoveTowards(e.transform.position, new Vector2(0, 0), 10f * Time.deltaTime);
            }
 
        }
        
    }

    void Explosion()
    {
        StartCoroutine(PauseBGM());

        _explosion.Play();

        enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy e in enemies)
        {
            e.speed = 0;

            e.GetComponent<Collider2D>().enabled = false;

            FindObjectOfType<GameManager>().AddScore(e.GetComponent<EnemyHealth>().ScoreIncrement);

            Destroy(e.gameObject, 1.3f);
        }

        _audioSource.Play();

        Destroy(gameObject, 6f);

    }

    IEnumerator PauseBGM()
    {
        AudioSource BGM = GameObject.Find("BGMManager").GetComponent<AudioSource>();

        BGM.volume = 0.3f;

        yield return new WaitForSeconds(3.5f);

        BGM.volume = 1;
    }

}
