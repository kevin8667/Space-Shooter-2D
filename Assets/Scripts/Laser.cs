using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    float _speed = 8.0f;

    [SerializeField]
    float _rotateSpeed = 200f;

    [SerializeField]
    float _duration = 2.5f;

    [SerializeField]
    bool _isHomingLaser;

    [HideInInspector]
    public bool isEnemyLaser;

    List<GameObject> _targets;

    Transform _transformMin;

    Rigidbody2D _ridigBody2D;

    float _minDistance = Mathf.Infinity;

    float _distanceToTarget;

    Vector2 _currentPos;

    private void Start()
    {
        _ridigBody2D = GetComponent<Rigidbody2D>();

        _currentPos = transform.position;

        _targets = new List<GameObject>();

    }


    void Update()
    {
        MoveNormalLaser();
    }

    void FixedUpdate()
    {
        if (_isHomingLaser)
        {
            if (_transformMin == null)
            {
                FindNearestTraget();
            }

            ChaseTarget();
        }
       
    }


    void MoveNormalLaser()
    {
        float distance = Vector2.Distance(transform.position, _currentPos);

        if (!_isHomingLaser)
        {
            if (isEnemyLaser)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);   

            }
            else
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }


            if (distance > 10f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(gameObject);
            }
        }
    }

    void FindNearestTraget()
    {
        _targets = new List<GameObject>();

        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!t.gameObject.GetComponent<Enemy>().isDestroyed)
            {
                _targets.Add(t);
            }

        }

        if(_targets.Count == 0)
        {
            _ridigBody2D.velocity = transform.up * _speed;
        }

        foreach (GameObject t in _targets)
        {
            if (t != null)
            {
                _distanceToTarget = Vector2.Distance(t.GetComponent<Transform>().position, _currentPos);

                if (_distanceToTarget < _minDistance)
                {
                    _transformMin = t.GetComponent<Transform>();

                    _minDistance = _distanceToTarget;
                }
            }
        }
    }
    

    void ChaseTarget()
    {
        if (_transformMin != null && !_transformMin.GetComponent<Enemy>().isDestroyed)
        {

            Vector2 direction = (Vector2)_transformMin.position - _ridigBody2D.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _ridigBody2D.angularVelocity = -rotateAmount * _rotateSpeed;

            _ridigBody2D.velocity = transform.up * _speed;

            Destroy(gameObject, _duration);


        }else if (_transformMin != null && _transformMin.GetComponent<Enemy>().isDestroyed)
        {

            _minDistance = Mathf.Infinity;

            FindNearestTraget();

            Vector2 direction = (Vector2)_transformMin.position - _ridigBody2D.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _ridigBody2D.angularVelocity = -rotateAmount * _rotateSpeed;

            _ridigBody2D.velocity = transform.up * _speed;

            Destroy(gameObject, _duration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && isEnemyLaser == true)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if(playerHealth != null)
            {
                playerHealth.Damage();
            }
        }
    }

}

