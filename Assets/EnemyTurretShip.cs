using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyTurretShip : Enemy
{
    [SerializeField]
    float _fireRate = 0.5f;
    float _canFire = 0f;

    [SerializeField]
    GameObject _turret;

    [SerializeField]
    GameObject _enemyLaser;

    GameObject _player;

    void Start()
    {

        SetEnemy();

        _player = GameObject.Find("Player");

        enemyHealth = GetComponent<EnemyHealth>();

    }

    void Update()
    {
        MoveEnemy();

        if(_player != null)
        {
            TurretFaceTarget();
        }
        

        if (Time.time >= _canFire)
        {
            FireLaser();
        }
    }

    void TurretFaceTarget()
    {
        Vector3 relativePos = _player.transform.position - transform.position;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * -relativePos;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, targetRotation, 1000f * Time.deltaTime);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        GameObject newLaser = Instantiate(_enemyLaser, transform.position + new Vector3(0, 0, 0), _turret.transform.rotation);

        Laser laser = newLaser.GetComponent<Laser>();

        laser.GetComponent<Laser>().isEnemyLaser = true;
    }

}
