using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Genaral Settings")]
    [SerializeField]
    float _speed = 8f;

    [SerializeField]
    float _homingSpeed = 12f;

    [Header("Normal Laser Settings")]
    public float range = 10f;

    public float power = 1f;

    [Header("Homing Laser Settings")]
    [SerializeField]
    float _rotateSpeed = 200f;

    [SerializeField]
    float _duration = 2.5f;

    public bool isHomingLaser;

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

        if (isHomingLaser)
        {
            power /= 2;
        }

    }


    void Update()
    {
        MoveNormalLaser();
    }

    void FixedUpdate()
    {
        if (isHomingLaser)
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

        if (!isHomingLaser)
        {
            if (isEnemyLaser)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);   

            }
            else
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }


            if (distance > range)
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


        if (_targets.Count == 0 || GameObject.FindGameObjectsWithTag("Enemy") == null)
        {
            _ridigBody2D.velocity = transform.up * _speed;

            Destroy(gameObject, _duration);
        }


        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Enemy"))
        {

            if (t.GetComponent<Enemy>() != null && !t.GetComponent<Enemy>().isDestroyed)
            {
                _targets.Add(t);

            }

            if (t.GetComponent<Boss>() != null && !t.GetComponent<Boss>().isDestroyed)
            {
                _targets.Add(t);
            }

            if (t.GetComponent<ShieldBit>() != null && !t.GetComponent<ShieldBit>().isDestroyed)
            {
                _targets.Add(t);
            }

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
        if(_transformMin != null)
        {
            if(_transformMin.GetComponent<Enemy>() != null)
            {
                if (!_transformMin.GetComponent<Enemy>().isDestroyed)
                {
                    MoveHomingLaser();
                }
                else if (_transformMin.GetComponent<Enemy>().isDestroyed)
                {

                    _minDistance = Mathf.Infinity;

                    FindNearestTraget();

                    MoveHomingLaser();
                }
            }

            if (_transformMin.GetComponent<Boss>() != null)
            {
                if (!_transformMin.GetComponent<Boss>().isDestroyed)
                {
                    MoveHomingLaser();
                }
                else if (_transformMin.GetComponent<Boss>().isDestroyed)
                {

                    _minDistance = Mathf.Infinity;

                    FindNearestTraget();

                    MoveHomingLaser();
                }
            }

            if (_transformMin.GetComponent<ShieldBit>() != null)
            {
                if (!_transformMin.GetComponent<ShieldBit>().isDestroyed)
                {
                    MoveHomingLaser();
                }
                else if (_transformMin.GetComponent<ShieldBit>().isDestroyed)
                {

                    _minDistance = Mathf.Infinity;

                    FindNearestTraget();

                    MoveHomingLaser();
                }
            }
        }

       
    }

    void MoveHomingLaser()
    {
        Vector2 direction = (Vector2)_transformMin.position - _ridigBody2D.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        _ridigBody2D.angularVelocity = -rotateAmount * _rotateSpeed;

        _ridigBody2D.velocity = transform.up * _homingSpeed;

        Destroy(gameObject, _duration);
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

            Destroy(gameObject);
        }
    }

}

