using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        Gunship,
        Agressive
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

    protected Vector2 startPos;

    public IDictionary<string, MovementAttributes> movementAttrDic;

    [HideInInspector]
    public bool isDestroyed;

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

        startPos = transform.position;

        if (isShielded)
        {
            shield.SetActive(true);
        }
        enemyHealth = GetComponent<EnemyHealth>();

        Debug.Log(movementType);

    }

    // Update is called once per frame
    void Update()
    {

        MoveEnemy();

    }

    protected void RandomizeType()
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
            else if (rand2 < 0.75f)
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
        startPos = transform.position;
    }

    protected virtual Vector2 RandomizeStartPoint()
    {
        Vector2 point = new Vector2(0, 0);
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
        else if (other.tag == "Laser" && !other.GetComponent<Laser>().isEnemyLaser && !isDestroyed)
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
