using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class FragBomb : MonoBehaviour
{
    [SerializeField]
    int _fragmentNumber;

    [SerializeField]
    float _burstRadius;

    [SerializeField]
    GameObject _explosion;
    
    [SerializeField]
    GameObject _fragment;

    [HideInInspector]
    public Vector3 deployPoint;

    bool _isPositioned;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, deployPoint, 10f * Time.deltaTime);
        
        if(transform.position == deployPoint && !_isPositioned)
        {
            _isPositioned = true;

            StartCoroutine(ExplosionSequence());
        }
    }

    void InstantiateFragments(int num, Vector3 point, float radius)
    {
        for (int i = 0; i < num; i++)
        {
            float radians = 2 * Mathf.PI / num * i;

            float vertical = Mathf.Sin(radians);

            float horizontal = Mathf.Cos(radians);

            Vector3 spawnDirection = new Vector3(horizontal, vertical);

            Vector3 spawnPosition = point + spawnDirection * radius;

            GameObject fragment = Instantiate(_fragment, spawnPosition, Quaternion.identity);

            fragment.transform.LookAt(point);

            fragment.transform.parent = transform;
        }
    }

    IEnumerator ExplosionSequence()
    {
        yield return new WaitForSeconds(1f);

        GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);

        explosion.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1f);

        InstantiateFragments(_fragmentNumber, transform.position, 0.5f);

        Destroy(explosion);

        GetComponent<MeshRenderer>().enabled = false;
        
    }
}
