using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public enum MovementType
    {
        UpToBottom,
        LeftToRight,
        RightToLeft

    };


    [SerializeField]
    float _speed = 4.5f;

    [SerializeField]
    int _scoreIncrement = 10;

    [System.Serializable]
    public struct MovementAttributes
    {
        public MovementType movementType;
        public Vector2 startPoint;
        public float randomRangeMin, randomRangeMax;
        public float moveDistance;
        public float rotation;
    }

    [SerializeField]
    MovementAttributes[] _movementAttr;

    [HideInInspector]
    public MovementType movementType;

    Vector2 startPos;

    public IDictionary<MovementType, MovementAttributes> movementAttrDic;

    GameManager _gameManager;

    Animator _anim;

    [HideInInspector]
    public bool isDestroyed;

    [SerializeField]
    AudioClip _explosionSFX;

    AudioSource _audioSource;

     void Awake()
    {
        movementAttrDic = new Dictionary<MovementType, MovementAttributes>();

        for (int i = 0; i < _movementAttr.Length; i++)
        {
            switch (i)
            {
                case 0:
                    _movementAttr[i].startPoint = new Vector2(Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax), _movementAttr[i].startPoint.y);
                    movementAttrDic.Add(_movementAttr[i].movementType, _movementAttr[i]);
                    break;
                case 1:
                    _movementAttr[i].startPoint = new Vector2(_movementAttr[i].startPoint.x, Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax));
                    movementAttrDic.Add(_movementAttr[i].movementType, _movementAttr[i]);
                    break;
                case 2:
                    _movementAttr[i].startPoint = new Vector2(_movementAttr[i].startPoint.x, Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax));
                    movementAttrDic.Add(_movementAttr[i].movementType, _movementAttr[i]);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;

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

        MoveEnemy();

    }

    private void MoveEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, startPos);

        if (movementType == MovementType.UpToBottom && distance >= movementAttrDic[movementType].moveDistance && !isDestroyed)
        {
            ResetPosition();
        }
        else if (movementType == MovementType.LeftToRight && distance >= movementAttrDic[movementType].moveDistance && !isDestroyed)
        {
            ResetPosition();
        }
        else if (movementType == MovementType.RightToLeft && distance >= movementAttrDic[movementType].moveDistance && !isDestroyed)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        RandomizeStartPoint();

        transform.position = movementAttrDic[movementType].startPoint;

        startPos = transform.position;
    }

    void RandomizeStartPoint()
    {
        for (int i = 0; i < _movementAttr.Length; i++)
        {
            switch (i)
            {
                case 0:
                    _movementAttr[i].startPoint = new Vector2(Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax), _movementAttr[i].startPoint.y);
                    break;
                case 1:
                    _movementAttr[i].startPoint = new Vector2(_movementAttr[i].startPoint.x, Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax));
                    break;
                case 2:
                    _movementAttr[i].startPoint = new Vector2(_movementAttr[i].startPoint.x, Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax));
                    break;
            }
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
