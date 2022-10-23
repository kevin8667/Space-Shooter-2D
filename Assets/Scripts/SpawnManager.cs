using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Related Settings")]
    [SerializeField]
    int _waveNumber = 3;

    int _currentWave = 0;

    int _enemyNumber = 0;

    [SerializeField]
    float _spawningInterval = 3f;

    [HideInInspector]
    public int destroyedEnemyNumber = 0;

    [SerializeField]
    int[] _enemiesInWaves;

    [SerializeField]
    GameObject[] _enemyPrefab;

    [SerializeField]
    GameObject _boss;

    [Header("Powerup Related Settings")]
    [SerializeField]
    GameObject[] _powerups;

    [SerializeField]
    float _powerupProbabilities;

    [SerializeField]
    GameObject _laserDiffuser;

    [SerializeField]
    float _diffuserSpawnProbability;

    GameObject _player;

    [SerializeField]
    UIManager _uIManager;

    bool _isAllWavesCompleted;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

        if(_enemiesInWaves.Length > _waveNumber || _enemiesInWaves.Length < _waveNumber)
        {
            Debug.LogError("The list length of Enemies In Waves should be the same as the wave number!");
        }

        if(_player != null)
        {
            _uIManager.UpdateWaveText(_currentWave+1);

            StartCoroutine(SpawnRoutine());

            StartCoroutine(SpawnPowerupsRoutine());

            StartCoroutine(SpawnExtraPowerupsRoutine());

            StartCoroutine(SpawnDiffuserRoutine());
        }
        
    }

    void Update()
    {
        if (destroyedEnemyNumber == _enemiesInWaves[_currentWave] && _currentWave+1 < _waveNumber)
        {
            StartCoroutine(ResetEnemySpawn());
        }

        
        if(destroyedEnemyNumber == _enemiesInWaves[_currentWave] && _currentWave+1 == _waveNumber && !_isAllWavesCompleted)
        {
            FindObjectOfType<GameManager>().WarningSequence();

            Instantiate(_boss, new Vector2(0, 9), Quaternion.identity);

            _isAllWavesCompleted = true;

        }

    }


    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return null;

            while (_player != null && _enemyNumber < _enemiesInWaves[_currentWave] && destroyedEnemyNumber != _enemiesInWaves[_currentWave])
            {

                int randEnemy = Random.Range(0, _enemyPrefab.Length);

                Instantiate(_enemyPrefab[randEnemy], gameObject.transform.position, Quaternion.identity);

                _enemyNumber++;

                yield return new WaitForSeconds(_spawningInterval);

            }
        }
    }

    IEnumerator ResetEnemySpawn()
    {
        destroyedEnemyNumber = 0;

        yield return new WaitForSeconds(1f);

        _currentWave++;

        _uIManager.UpdateWaveText(_currentWave+1);

        _enemyNumber = 0;

        yield return new WaitForSeconds(1f);

    }


    IEnumerator SpawnPowerupsRoutine()
    {
        while (_player != null)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            Instantiate(_powerups[Random.Range(0, 3)], spawningPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    IEnumerator SpawnExtraPowerupsRoutine()
    {
        while (_player != null)
        {
            Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            float rand = Random.value;

            
            if (rand <= 0.3f)
            {
                float rand2 = Random.value;

                if(rand2 >= _powerupProbabilities)
                {
                    Instantiate(_powerups[3], spawningPos, Quaternion.identity);
                }
                else
                {
                    Instantiate(_powerups[4], spawningPos, Quaternion.identity);
                }

            }

            yield return new WaitForSeconds(Random.Range(7f, 15f));
        }
    }

    IEnumerator SpawnDiffuserRoutine()
    {
        while (_player != null)
        {
            float rand = Random.value;

            if(rand <= _diffuserSpawnProbability)
            {
                Vector3 spawningPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

                Instantiate(_laserDiffuser, spawningPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(7f, 15f));
        }
    }
}
