using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField]
    float _sensorRadius = 3;
    float _speed = 3;
    float _range = 2;

    Vector2 _startPos;

    CircleCollider2D _collider2D;

    ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;

        _collider2D = gameObject.GetComponent<CircleCollider2D>();

        _collider2D.radius = _sensorRadius;

        explosion = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            explosion.Play();
        }

        if(Vector2.Distance(transform.position, _startPos) < _range)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
    }

    
}
