using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunship : Enemy
{
    [Header("Weapon Settings")]
    [SerializeField]
    float _fireRate = 0.5f;
    float _canFire = 0f;

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();

        if (isArmed && Time.time >= _canFire)
        {
            FireLaser();
        }

    }

    protected override void MoveEnemy()
    {
        switch (movementType)
        {
            case "TopToBottom":
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
            case "LeftToRight":
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                break;
            case "RightToLeft":
                transform.Translate(-Vector3.right * speed * Time.deltaTime);
                break;
        }

        float distance = Vector2.Distance(transform.position, startPos);

        if (distance >= movementAttrDic[movementType].moveDistance && !isDestroyed)
        {
            ResetPosition();
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        GameObject newLaser = Instantiate(enemyLaser, transform.position + new Vector3(0, 0, 0), Quaternion.identity);

        Laser[] lasers = newLaser.GetComponentsInChildren<Laser>();

        foreach (Laser laser in lasers)
        {
            laser.GetComponent<Laser>().isEnemyLaser = true;
        }
    }
}
