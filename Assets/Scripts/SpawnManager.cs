using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;

    [SerializeField]
    GameObject[] _powerups;

    GameObject _player;

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
            float rand = Random.value;

            GameObject newEnemy = Instantiate(_enemyPrefab, gameObject.transform.position, Quaternion.identity);

            Enemy enemyData = newEnemy.GetComponent<Enemy>();

            if (rand < 0.5f)
            {
                enemyData.movementType = Enemy.MovementType.UpToBottom;

                SetEnemy(newEnemy, enemyData);

                yield return new WaitForSeconds(3f);
            }
            
            if(rand > 0.5f)
            {
                float rand2 = Random.Range(rand, 1f);

                if (rand2 > 0.75f)
                {
                    enemyData.movementType = Enemy.MovementType.LeftToRight;

                    SetEnemy(newEnemy, enemyData);

                    yield return new WaitForSeconds(3f);
                }
                else if (rand2 < 0.75f)
                {
                    enemyData.movementType = Enemy.MovementType.RightToLeft;

                    SetEnemy(newEnemy, enemyData);

                    yield return new WaitForSeconds(3f);
                }

            }
        }
    }


    void SetEnemy(GameObject newEnemy, Enemy enemyData)
    {
        newEnemy.transform.rotation *= Quaternion.Euler(0, 0, enemyData.movementAttrDic[enemyData.movementType].rotation);

        newEnemy.transform.position = enemyData.movementAttrDic[enemyData.movementType].startPoint;

        newEnemy.transform.parent = gameObject.transform;

        float rand = Random.value;

        if(rand < 0.3f)
        {
            enemyData.isShielded = true;
        }
    }

    IEnumerator SpawanPowerupsRoutine()
    {
        while (_player != null)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            float rand = Random.value;

            if (rand < 0.7f)
            {
                Instantiate(_powerups[Random.Range(0, 3)], spawningPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(3f, 7f));

            }
            if(rand < 0.3f)
            {
                Instantiate(_powerups[Random.Range(3, 5)], spawningPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }

        }
    }
}
