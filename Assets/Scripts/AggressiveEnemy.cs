using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : Enemy
{
    [SerializeField]
    float _aimingRange = 5f;

    [SerializeField]
    float _rammingSpeed = 8f;

    bool _isAiming, _isLockedOn;

    GameObject _player;

    void Start()
    {

        SetEnemy();

        _player = GameObject.Find("Player");

        enemyHealth = GetComponent<EnemyHealth>();

    }

    // Update is called once per frame
    void Update()
    {

        MoveEnemy();

        float distanceToTarget = 0f;

        if(_player != null)
        {
            distanceToTarget = Vector3.Distance(GameObject.Find("Player").transform.position, transform.position);
        }


        if (distanceToTarget <= _aimingRange && !_isAiming)
        {
            if (_player != null)
            {
                _isAiming = true;

                StartCoroutine(AimingRoutine(_player));
            }
            
        }

        if (_isLockedOn)
        {
            transform.Translate(Vector3.down * _rammingSpeed * Time.deltaTime);
        }

    }

    protected override void SetEnemy()
    {
        transform.rotation *= Quaternion.Euler(0, 0, movementAttrDic[movementType].rotation);

        transform.position = movementAttrDic[movementType].startPoint;
    }

    protected override void MoveEnemy()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (!isDestroyed)
        {
            if (transform.position.x > 11 || transform.position.x < -11 || transform.position.y > 8 || transform.position.y < -8)
            {
                ResetPosition();
            }

        }
    }

    void FaceTarget(GameObject target)
    {
        Vector3 relativePos = target.transform.position - transform.position;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * -relativePos;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);
    }


    IEnumerator AimingRoutine(GameObject target)
    {
        float elapsedTime = 0;
        float duration = 1.5f;
        float tempSpeed = speed;
        speed = 0;

        while (elapsedTime < duration)
        {
            FaceTarget(target);

            elapsedTime += Time.deltaTime;

            yield return null;

        }

        yield return new WaitForSeconds(0.5f);

        _isLockedOn = true;

        speed = tempSpeed;
    }

    protected override void ResetPosition()
    {
        transform.position = RandomizeStartPoint();

        transform.rotation = Quaternion.identity;

        transform.rotation *= Quaternion.Euler(0, 0, movementAttrDic[movementType].rotation);

        _isAiming = false;

        _isLockedOn = false;
    }
}
