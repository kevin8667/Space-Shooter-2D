using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boss : MonoBehaviour
{

    [HideInInspector]
    bool _isShielded;

    bool _isPositioned;

    bool _isSeraching;

    [HideInInspector]
    public bool isDestroyed;

    [Header("Boss Health")]
    [SerializeField]
    BossHealth _bossHealth;

    [Header("Weapon Settings")]
    [SerializeField]
    GameObject _searchingCursor;

    GameObject _newSearchingCursor;

    [SerializeField]
    GameObject _turret;

    [SerializeField]
    GameObject _bossLaser;

    float _fireRate = 0.5f;

    float _canFire = 0f;

    [Header("Shield Settings")]
    [SerializeField]
    GameObject _shield;

    [SerializeField]
    GameObject _oritalPointer;

    [SerializeField]
    GameObject _shieldBit;

    List<Vector3> _orbitalPositions;

    List<GameObject> _orbitalPointers;

    [SerializeField]
    float _orbitingSpeed;

    public float OrbitingSpeed => _orbitingSpeed;

    public int shieldBitNumber = 0;

    void Start()
    {
        StartCoroutine(Entrance());

        _orbitalPointers = new List<GameObject>();

        _orbitalPositions = new List<Vector3>();

    }

    void Update()
    {
        if(transform.position.y == 2 && !_isPositioned)
        {
            _isPositioned = true;

            GetComponent<Collider2D>().enabled = true;

            SpawnShieldBits();

        }

        if(_isPositioned && !_isSeraching)
        {
            StartCoroutine(TargetingAttack());
        }


        if(shieldBitNumber == 0 || !_isPositioned)
        {
            ToggleShield(false);
        }
        else
        {
            ToggleShield(true);

        }
    }

    public void ToggleShield(bool shieldState)
    {
        _shield.SetActive(shieldState);

        _isShielded = shieldState;
    }


    IEnumerator Entrance()
    {
        while (!_isPositioned)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 2), 2.5f * Time.deltaTime);

            GetComponent<Collider2D>().enabled = false;

            yield return null;
        }
    }

    void StartTargeting()
    {
        _newSearchingCursor =Instantiate(_searchingCursor, transform.position - new Vector3(0, 1.75f, 0), Quaternion.identity);       
    }

    public void TurretFaceTarget()
    {
        Vector3 relativePos = _newSearchingCursor.transform.position - _turret.transform.position;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * -relativePos;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, targetRotation, 1000f * Time.deltaTime);
    }

    public void StartShootingLaser()
    {
        StartCoroutine(ShotingRoutine());
    } 

    IEnumerator ShotingRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;

            if (Time.time >= _canFire)
            {
                _canFire = Time.time + _fireRate;

                Instantiate(_bossLaser, _turret.transform.position, _turret.transform.rotation);

                GameObject LeftLaser = Instantiate(_bossLaser, _turret.transform.position, _turret.transform.rotation);

                LeftLaser.GetComponent<BossLaser>().sinAmplitude *= -1;
            }

            yield return null;
        }

    }

    IEnumerator TargetingAttack()
    {
        _isSeraching = true;

        StartTargeting();

        yield return new WaitForSeconds(8f);

        _isSeraching = false;

    }

    public void CreateOrbitalPoints(int num, Vector3 point, float radius)
    {
        
        for (int i = 0; i < num; i++)
        {

            float radians = 2 * Mathf.PI / num * i;

            float vertical = Mathf.Sin(radians);

            float horizontal = Mathf.Cos(radians);

            Vector3 spawnDirection = new Vector3(horizontal, vertical);

            _orbitalPositions.Add( point + spawnDirection * radius);

            _orbitalPointers.Add(Instantiate(_oritalPointer, transform.position, Quaternion.identity));

            _orbitalPointers[i].transform.parent = transform;

            _orbitalPointers[i].transform.position = _orbitalPositions[i];

        }
    }

    void SpawnShieldBits()
    {
        CreateOrbitalPoints(shieldBitNumber, transform.position, 3);

        for (int i = 0; i < shieldBitNumber; i++)
        {
            GameObject shieldBit = Instantiate(_shieldBit, transform.position, Quaternion.identity);

            shieldBit.transform.parent = transform;

            shieldBit.GetComponent<ShieldBit>().orbitalPointer = _orbitalPointers[i];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BossLaser>())
        {
            return;
        }


        if (other.tag == "Player")
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Damage();
            }

        }

        if (other.tag == "Laser" && !other.GetComponent<Laser>().isEnemyLaser && !isDestroyed)
        {

            if (_isShielded)
            {
                Destroy(other.gameObject);

                return;
            }

            Destroy(other.gameObject);

            _bossHealth.BossDamage(other.GetComponent<Laser>().power);

        }
    }
}
