using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4.5f;

    [SerializeField]
    int _scoreIncrement = 10;

    GameManager _gameManager;

    Animator _anim;

    public bool isDestroyed;

    [SerializeField]
    AudioClip _explosionSFX;

    AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _anim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL!");
        }
        else
        {
            _audioSource.clip = _explosionSFX;
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f && !isDestroyed)
        {
            transform.position = new Vector3(Random.Range(-8f, 8f), 7f, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Player")
        {

            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if(playerHealth != null)
            {
                playerHealth.Damage();
            }

            _anim.SetTrigger("OnEnemyDestroy");

            _audioSource.Play();

            GetComponent<Collider2D>().enabled = false;

            isDestroyed = true;

            Destroy(gameObject, 2.7f);


        }
        else if (other.tag == "Laser")
        {

            Destroy(other.gameObject);

            _anim.SetTrigger("OnEnemyDestroy");

            _audioSource.Play();

            GetComponent<Collider2D>().enabled = false;

            _gameManager.AddScore(_scoreIncrement);

            isDestroyed = true;

            Destroy(gameObject, 2.7f);

        }

    }



}
