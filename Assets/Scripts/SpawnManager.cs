using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;

    GameObject _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

        if(_player != null)
        {
            StartCoroutine(SpawnRoutine());
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnRoutine()
    {
        while (_player.activeInHierarchy)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, spawningPos, Quaternion.identity);

            newEnemy.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(3f);
        }
    }
}
