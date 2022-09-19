using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Drawing;

public class Enemy : MonoBehaviour
{
    public enum MovementType
    {
        UpToBottom,
        LeftToRight,
        RightToLeft

    };

    [HideInInspector]
    public bool isShielded;

    [SerializeField]
    GameObject _shield;

    public float speed = 4.5f;

    [System.Serializable]
    public struct MovementAttributes
    {
        public MovementType movementType;
        public Vector2 startPoint;
        public float randomRangeMin, randomRangeMax;
        public float moveDistance;
        public float rotation;
    }

    [SerializeField]
    MovementAttributes[] _movementAttr;

    [HideInInspector]
    public MovementType movementType;

    Vector2 startPos;

    public IDictionary<MovementType, MovementAttributes> movementAttrDic;

    [HideInInspector]
    public bool isDestroyed;

    EnemyHealth _enemyHealth;

     void Awake()
    {
        movementAttrDic = new Dictionary<MovementType, MovementAttributes>();

        for (int i = 0; i < _movementAttr.Length; i++)
        {
            switch (i)
            {
                case 0:
                    _movementAttr[i].startPoint = new Vector2(Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax), _movementAttr[i].startPoint.y);
                    movementAttrDic.Add(_movementAttr[i].movementType, _movementAttr[i]);
                    break;
                case 1:
                    _movementAttr[i].startPoint = new Vector2(_movementAttr[i].startPoint.x, Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax));
                    movementAttrDic.Add(_movementAttr[i].movementType, _movementAttr[i]);
                    break;
                case 2:
                    _movementAttr[i].startPoint = new Vector2(_movementAttr[i].startPoint.x, Random.Range(_movementAttr[i].randomRangeMin, _movementAttr[i].randomRangeMax));
                    movementAttrDic.Add(_movementAttr[i].movementType, _movementAttr[i]);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;

        if (isShielded)
        {
            _shield.SetActive(true);
        }
        _enemyHealth = GetComponent<EnemyHealth>();


    }

    // Update is called once per frame
    void Update()
    {

        MoveEnemy();

    }

    private void MoveEnemy()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, startPos);

        if (movementType == MovementType.UpToBottom && distance >= movementAttrDic[movementType].moveDistance && !isDestroyed)
        {
            ResetPosition();
        }
        else if (movementType == MovementType.LeftToRight && distance >= movementAttrDic[movementType].moveDistance && !isDestroyed)
        {
            ResetPosition();
        }
        else if (movementType == MovementType.RightToLeft && distance >= movementAttrDic[movementType].moveDistance && !isDestroyed)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        transform.position = RandomizeStartPoint();
        startPos = transform.position;
    }

    Vector2  RandomizeStartPoint()
    {
        Vector2 point = new Vector2(0, 0);
        switch (movementType)
        {
            case MovementType.UpToBottom:
                point = new Vector2(Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax), movementAttrDic[movementType].startPoint.y);
                break;
            case MovementType.LeftToRight: case MovementType.RightToLeft:
                point = new Vector2(movementAttrDic[movementType].startPoint.x, Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax));
                break;
        }
        return point;

    }
    

    public void ToggleShield()
    {
        _shield.SetActive(isShielded);
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
                _enemyHealth.PlayShieldBreakingSFX();
                ToggleShield();
                return;
            }

            _enemyHealth.Damage();

        }
        else if (other.tag == "Laser")
        {

            if (isShielded)
            {
                isShielded = false;
                _enemyHealth.PlayShieldBreakingSFX();
                ToggleShield();
                Destroy(other.gameObject);
                return;
            }

            Destroy(other.gameObject);

            _enemyHealth.Damage();

        }
    }

    
}
