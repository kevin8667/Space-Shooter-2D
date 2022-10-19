using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    [SerializeField]
    float _speed;

    [SerializeField]
    float _range;

    Vector3 _startPosition;

    float _distance;

    [SerializeField]
    string sortingLayerName = string.Empty;

    [SerializeField]
    int orderInLayer = 0;

    [SerializeField]
    Renderer Renderer;

    void Start()
    {
        _startPosition = transform.position;

        SetSortingLayer();

    }

    void Update()
    {
        MoveFragement();
    }

    void MoveFragement()
    {
        _distance = Vector3.Distance(_startPosition, transform.position);

        transform.Translate(Vector3.back * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (_distance > _range)
        {
            Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    void SetSortingLayer()
    {
        if (sortingLayerName != string.Empty)
        {
            Renderer.sortingLayerName = sortingLayerName;
            Renderer.sortingOrder = orderInLayer;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Damage();
            }

            Destroy(gameObject);
        }
    }
}
