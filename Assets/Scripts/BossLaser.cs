using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [Header("Genaral Settings")]
    [SerializeField]
    float _speed = 8f;

    public float range = 10f;

    public bool isEnemyLaser;

    [SerializeField]
    float _sinFrequency;

    [SerializeField]
    public float sinAmplitude;

    Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;

        if (sinAmplitude > 0)
        {
            transform.position -= transform.right * 0.5f;
        }
        else if (sinAmplitude < 0)
        {
            transform.position += transform.right * 0.5f;
        }

        StartCoroutine(WaveMovementRoutine());
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(transform.position, _startPosition);

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (distance > range)
        {
            Destroy(gameObject);
        }

    }

    IEnumerator WaveMovementRoutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;

            transform.position = transform.position + transform.right * Mathf.Sin(elapsedTime * _sinFrequency) * sinAmplitude * 0.1f;

            yield return null;
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
