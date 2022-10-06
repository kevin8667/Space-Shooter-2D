using System.Collections;
using UnityEngine;


public class PowerupSnatcher : Enemy
{
    [Header("Snatcher Setting")]
    [SerializeField]
    float _snatchRange = 10f;

    void Update()
    {
        MoveEnemy();

        DetectPowerup();
    }


    protected override void MoveEnemy()
    {
        switch (movementType)
        {
            case "TopToBottom":
                if (transform.position.x < 0)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                if (transform.position.x > 0)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
                break;
            case "LeftToRight":
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                break;
            case "RightToLeft":
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                break;
        }

        if (!isDestroyed)
        {
            if (transform.position.x > 11 || transform.position.x < -11 || transform.position.y > 8 || transform.position.y < -8)
            {
                ResetPosition();
            }
        }
    }

    void DetectPowerup()
    {
        Powerup[] powerups = FindObjectsOfType<Powerup>();

        foreach (Powerup powerup in powerups)
        {
            float distance;
            float angle = 0;

            if (movementType == movementTypes[0])
            {
                if (transform.position.x < 0)
                {
                    angle = Vector2.Angle(Vector2.right, powerup.transform.position - transform.position);
                }
                if (transform.position.x > 0)
                {
                    angle = Vector2.Angle(Vector2.left, powerup.transform.position - transform.position);
                }
            }
            else
            {
                angle = Vector2.Angle(Vector2.down, powerup.transform.position - transform.position);
            }
            if (Mathf.Abs(angle) < 30)
            {
                distance = Vector2.Distance(powerup.transform.position, transform.position);

                if (distance <= _snatchRange)
                {
                    StartCoroutine(SnatchPowerup(powerup.gameObject));
                }
            }

        }

    }

    IEnumerator SnatchPowerup(GameObject powerup)
    {
        while (powerup!=null)
        {
            powerup.transform.position = Vector2.MoveTowards(powerup.transform.position, transform.position, 10f * Time.deltaTime);

            yield return null;
        }
    }

    protected override void SetEnemy()
    {
        transform.position = movementAttrDic[movementType].startPoint;

        if(movementType == movementTypes[0])
        {
            while (transform.position.x == 0)
            {
                transform.position = new Vector2(Random.Range(-1f, 1f), transform.position.y);
            }
            if (transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            if (transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }
        }
    }

    protected override void ResetPosition()
    {
        transform.position = RandomizeStartPoint();

        if (movementType == movementTypes[0])
        {
            if (transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            if (transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }
        }
    }

    protected override Vector2 RandomizeStartPoint()
    {
        Vector2 point = new Vector2();
        switch (movementType)
        {
            case "TopToBottom":
                do
                {
                    point = new Vector2(Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax), movementAttrDic[movementType].startPoint.y);
                } while (transform.position.x == 0);
                break;
            case "LeftToRight":
            case "RightToLeft":
                point = new Vector2(movementAttrDic[movementType].startPoint.x, Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax));
                break;
        }
        return point;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.tag == "Player")
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Damage();
            }

            if (isShielded)
            {
                isShielded = false;
                enemyHealth.PlayShieldBreakingSFX();
                ToggleShield();
                return;
            }

            enemyHealth.Damage();

        }
        
        if (other.tag == "Laser" && !other.GetComponent<Laser>().isEnemyLaser && !isDestroyed)
        {

            if (isShielded)
            {
                isShielded = false;
                enemyHealth.PlayShieldBreakingSFX();
                ToggleShield();
                Destroy(other.gameObject);
                return;
            }

            Destroy(other.gameObject);

            enemyHealth.Damage();

        }

        if(other.GetComponent<Powerup>() != null && !isDestroyed)
        {
            Destroy(other.gameObject);
        }
    }

}
