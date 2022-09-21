using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    int _waveNumber = 3;

    int _currentWave = 0;

    int _enemyNumber = 0;

    public int destroyedEnemyNumber = 0;

    [SerializeField]
    int[] _enemiesInWaves;

    [SerializeField]
    GameObject[] _enemyPrefab;

    [SerializeField]
    GameObject[] _powerups;

    GameObject _player;

    [SerializeField]
    UIManager _uIManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

        if(_enemiesInWaves.Length > _waveNumber || _enemiesInWaves.Length < _waveNumber)
        {
            Debug.LogError("The list length of 'Enemies In Waves should be the same as the wave number!");
        }

        if(_player != null)
        {
            _uIManager.UpdateWaveText(_currentWave+1);

            StartCoroutine(SpawnRoutine());
            StartCoroutine(SpawnPowerupsRoutine());
            StartCoroutine(SpawnExtraPowerupsRoutine());
        }
        
    }

    void Update()
    {

        if(destroyedEnemyNumber == _enemiesInWaves[_currentWave] && _currentWave+1 < _waveNumber)
        {
            destroyedEnemyNumber = 0;

            StartCoroutine(ResetEnemySpawn());
        }

    }


    IEnumerator SpawnRoutine()
    {
        while (_player != null && _enemyNumber < _enemiesInWaves[_currentWave])
        {
           
            float rand = Random.value;

            int randEnemy = Random.Range(0, _enemyPrefab.Length);

            Debug.Log(randEnemy);

            GameObject newEnemy = Instantiate(_enemyPrefab[randEnemy], gameObject.transform.position, Quaternion.identity);

            _enemyNumber++;

            Enemy enemyData = newEnemy.GetComponent<Enemy>();

            if (rand < 0.5f)
            {
                enemyData.movementType = Enemy.MovementType.UpToBottom;

                SetEnemy(newEnemy, enemyData);

                yield return new WaitForSeconds(3f);
            }

            if (rand > 0.5f)
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

    IEnumerator ResetEnemySpawn()
    {
        Debug.Log(_currentWave);

        _currentWave++;

        _uIManager.UpdateWaveText(_currentWave+1);

        yield return new WaitForSeconds(1f);

        _enemyNumber = 0;

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

    IEnumerator SpawnPowerupsRoutine()
    {
        while (_player != null)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            Instantiate(_powerups[Random.Range(0, 3)], spawningPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 7f));

            

        }
    }

    IEnumerator SpawnExtraPowerupsRoutine()
    {
        while (_player != null)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            float rand = Random.value;

            if (rand > 0.5f)
            {
                yield return new WaitForSeconds(Random.Range(3f, 7f));

            }
            if (rand < 0.5f)
            {
                Instantiate(_powerups[Random.Range(3, 5)], spawningPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }
        }

    }
}
