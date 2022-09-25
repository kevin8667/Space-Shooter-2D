using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggresiveEnemy : Enemy
{
    void Awake()
    {
        movementAttrDic = new Dictionary<string, MovementAttributes>();

        IniitalizeDictionary();
    }

    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;

        enemyHealth = GetComponent<EnemyHealth>();


    }

    // Update is called once per frame
    void Update()
    {

        MoveEnemy();

    }

    protected override void IniitalizeDictionary()
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

    protected override void MoveEnemy()
    {
        switch (movementType)
        {
            case "UpToBottom":
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

    protected override void ResetPosition()
    {
        transform.position = RandomizeStartPoint();
        startPos = transform.position;
    }

    protected override Vector2 RandomizeStartPoint()
    {
        Vector2 point = new Vector2(0, 0);
        switch (movementType)
        {
            case "UpToBottom":
                point = new Vector2(Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax), movementAttrDic[movementType].startPoint.y);
                break;
            case "LeftToRight":
            case "RightToLeft":
                point = new Vector2(movementAttrDic[movementType].startPoint.x, Random.Range(movementAttrDic[movementType].randomRangeMin, movementAttrDic[movementType].randomRangeMax));
                break;
        }
        return point;

    }
}
