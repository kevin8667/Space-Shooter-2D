using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;

    GameObject _player;

    [SerializeField]
    GameObject[] _powerups;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

        if(_player != null)
        {
            StartCoroutine(SpawnRoutine());
            StartCoroutine(SpawanPowerupsRoutine());
        }
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_player != null)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, spawningPos, Quaternion.identity);

            newEnemy.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator SpawanPowerupsRoutine()
    {
        while (_player != null)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            Instantiate(_powerups[Random.Range(0, 3)], spawningPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }
}
