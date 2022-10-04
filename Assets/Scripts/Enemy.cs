using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        Gunship,
        Agressive,
        Snatcher
    };

    [Header("Movement Settings")]
    public float speed = 4.5f;

    [SerializeField]
    public EnemyType enemyType;

    public string[] movementTypes;

    [System.Serializable]
    public struct MovementAttributes
    {
        public string movementType;
        public Vector2 startPoint;
        public float randomRangeMin, randomRangeMax;
        public float moveDistance;
        public float rotation;
    }

    [SerializeField]
    protected MovementAttributes[] movementAttr;

    [HideInInspector]
    public string movementType;

    [HideInInspector]
    public bool isShielded;

    public bool isArmed;

    [SerializeField]
    protected GameObject shield;

    [SerializeField]
    protected GameObject enemyLaser;

    public IDictionary<string, MovementAttributes> movementAttrDic;

    [HideInInspector]
    public bool isDestroyed;

    bool _isDodged;

    protected EnemyHealth enemyHealth;

     void Awake()
    {

        movementAttrDic = new Dictionary<string, MovementAttributes>();

        RandomizeType();

        IniitalizeDictionary();
    }

    // Start is called before the first frame update
    void Start()
    {

        SetEnemy();

        if (isShielded)
        {
            shield.SetActive(true);
        }

        enemyHealth = GetComponent<EnemyHealth>();

    }

    // Update is called once per frame
    void Update()
    {

        MoveEnemy();

        if (!_isDodged)
        {        
            DetectLaser();    
        }

    }

    protected virtual void RandomizeType()
    {
        float rand = Random.value;

        if (rand <= 0.5f)
        {
            movementType = movementTypes[0];
        }

        if (rand > 0.5f)
        {
            float rand2 = Random.Range(rand, 1f);

            if (rand2 > 0.75f)
            {
                movementType = movementTypes[1];
            }
            
            if (rand2 < 0.75f && rand2 > 0.5f)
            {
                movementType = movementTypes[2];
            }
        }
    }

    protected virtual void IniitalizeDictionary()
    {
        for (int i = 0; i < movementAttr.Length; i++)
        {
            switch (i)
            {
                case 0:
                    movementAttr[i].startPoint = new Vector2(Random.Range(movementAttr[i].randomRangeMin, movementAttr[i].randomRangeMax), movementAttr[i].startPoint.y);
                    movementAttrDic.Add(movementAttr[i].movementType, movementAttr[i]);
                    break;
                case 1:
                    movementAttr[i].startPoint = new Vector2(movementAttr[i].startPoint.x, Random.Range(movementAttr[i].randomRangeMin, movementAttr[i].randomRangeMax));
                    movementAttrDic.Add(movementAttr[i].movementType, movementAttr[i]);
                    break;
                case 2:
                    movementAttr[i].startPoint = new Vector2(movementAttr[i].startPoint.x, Random.Range(movementAttr[i].randomRangeMin, movementAttr[i].randomRangeMax));
                    movementAttrDic.Add(movementAttr[i].movementType, movementAttr[i]);
                    break;
            }
        }
    }


    protected virtual void SetEnemy()
    {
        transform.rotation *= Quaternion.Euler(0, 0, movementAttrDic[movementType].rotation);

        transform.position = movementAttrDic[movementType].startPoint;

        float rand = Random.value;

        if (rand <= 0.3f)
        {
            if (transform.Find("Shield") != null)
            {
                isShielded = true;
            }

        }
    }

    protected virtual void MoveEnemy()
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

    protected virtual void ResetPosition()
    {
        transform.position = RandomizeStartPoint();
    }

    protected virtual Vector2 RandomizeStartPoint()
    {
        Vector2 point = new Vector2();
        switch (movementType)
        {
            case "TopToBottom":
                point = new Vector2(Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax), movementAttrDic[movementType].startPoint.y);
                break;
            case "LeftToRight": case "RightToLeft":
                point = new Vector2(movementAttrDic[movementType].startPoint.x, Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax));
                break;
        }
        return point;

    }
    
    public void ToggleShield()
    {
        shield.SetActive(isShielded);
    }

    void DetectLaser()
    {
        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
        List<Laser> playerLasers = new List<Laser>();

        if(lasers != null)
        {
            foreach(GameObject laser in lasers)
            {
                if (!laser.GetComponent<Laser>().isEnemyLaser && !laser.GetComponent<Laser>().isHomingLaser)
                {
                    playerLasers.Add(laser.GetComponent<Laser>());
                }
            }
        }

        foreach(Laser playerLaser in playerLasers)
        {
            float distance;
            float angle;

            if (movementType == movementTypes[0])
            {
                angle = Vector2.SignedAngle(Vector2.down, playerLaser.transform.position - transform.position);

                if (Mathf.Abs(angle) < 20)
                {
                    distance = Vector2.Distance(playerLaser.transform.position, transform.position);

                    if (distance <= 2.2f)
                    {
                        StartCoroutine(AvoidLaser(angle));
                    }
                }
            }

            if (movementType == movementTypes[1])
            {
                angle = Vector2.SignedAngle(Vector2.right, playerLaser.transform.position - transform.position);

                if (Mathf.Abs(angle) < 20)
                {
                    distance = Vector2.Distance(playerLaser.transform.position, transform.position);

                    if (distance <= 2.2f)
                    {
                        StartCoroutine(AvoidLaser(angle));
                    }
                }
            }

            if (movementType == movementTypes[2])
            {
                angle = Vector2.SignedAngle(Vector2.left, playerLaser.transform.position - transform.position);

                if (Mathf.Abs(angle) < 20)
                {
                    distance = Vector2.Distance(playerLaser.transform.position, transform.position);

                    if (distance <= 2.2f)
                    {                      
                        StartCoroutine(AvoidLaser(angle));
                    }
                    
                }
            }

        }
    }

    IEnumerator AvoidLaser(float angle)
    {
        float elapedTime = 0;
        float duration = 0.1f;

        if (movementType == movementTypes[0])
        {
            
            if (angle <= 0)
            {
                _isDodged = true;

                while (elapedTime < duration)
                {
                    elapedTime += Time.deltaTime;

                    transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + 0.7f, transform.position.y), elapedTime);

                    if (transform.position.x > 10)
                    {
                        transform.position = new Vector2(10, transform.position.y);
                    }
                    else if (transform.position.x < -10)
                    {
                        transform.position = new Vector2(-10, transform.position.y);
                    }

                    yield return null;
                }
                
            }

            if (angle > 0)
            {
                _isDodged = true;

                while (elapedTime < duration)
                {
                    elapedTime += Time.deltaTime;

                    transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x - 0.7f, transform.position.y), elapedTime);

                    if (transform.position.x > 10)
                    {
                        transform.position = new Vector2(10, transform.position.y);
                    }
                    else if (transform.position.x < -10)
                    {
                        transform.position = new Vector2(-10, transform.position.y);
                    }

                    yield return null;
                }

            }
        }

        if(movementType == movementTypes[1])
        {
            _isDodged = true;

            while (elapedTime < duration)
            {
                elapedTime += Time.deltaTime;

                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x - 1f, transform.position.y), elapedTime);

                if (transform.position.x < -10)
                {
                    transform.position = new Vector2(-10, transform.position.y);
                }

                yield return null;
            }
        }

        if (movementType == movementTypes[2])
        {
            _isDodged = true;

            while (elapedTime < duration)
            {
                elapedTime += Time.deltaTime;

                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + 1f, transform.position.y), elapedTime);

                if (transform.position.x > 10)
                {
                    transform.position = new Vector2(10, transform.position.y);
                }

                yield return null;
            }

        }

        
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
    }

}
