using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTest : MonoBehaviour
{
    [SerializeField]
    GameObject _laser;

    GameObject _player;

    [SerializeField]
    float _fireRate = 0.5f;
    float _canFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget();

        if (Time.time >= _canFire)
        {
            FireLaser();
        }
    }


    void FaceTarget()
    {
        Vector3 relativePos = GameObject.Find("Player").transform.position - transform.position;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * -relativePos;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        GameObject newLaser = Instantiate(_laser, transform.position + new Vector3(0, 0, 0), transform.rotation);

        Laser laser = newLaser.GetComponent<Laser>();

        laser.GetComponent<Laser>().isEnemyLaser = true;
    }
}
