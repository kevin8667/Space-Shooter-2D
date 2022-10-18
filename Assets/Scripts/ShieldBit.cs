using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBit : MonoBehaviour
{

    [SerializeField]
    float _Hp;

    [SerializeField]
    float _respawnTime = 5f;

    Vector3[] _linePositions = new Vector3[2];

    public bool isDestroyed;

    public GameObject orbitalPointer;

    Boss _boss;

    [SerializeField]
    LineRenderer _lineRenderer;

    [SerializeField]
    GameObject _explosion;


    void Start()
    {
        _boss = FindObjectOfType<Boss>();
    }

    void Update()
    {
        DrawEnergyLine();

        if (!isDestroyed)
        {
            transform.position = Vector2.MoveTowards(transform.position, orbitalPointer.transform.position, 10f * Time.deltaTime);
        }

    }

    void DrawEnergyLine()
    {
        _linePositions[0] = transform.position;

        if (_boss != null)
        {
            _linePositions[1] = _boss.transform.position;
        }

        if (_boss != null)
        {
            _lineRenderer.SetPositions(_linePositions);
        }
    }

    IEnumerator Respawn()
    {
        float elapsedTime = 0;

        while (isDestroyed && elapsedTime < _respawnTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        
        if (elapsedTime >= _respawnTime && !_boss.isDestroyed)
        {
            GetComponent<SpriteRenderer>().enabled = true;

            GetComponent<Collider2D>().enabled = true;

            _lineRenderer.enabled = true;

            _boss.shieldBitNumber++;

            isDestroyed = false;
        }
    }
    

    void Damage(float damageAmount)
    {
        _Hp -= damageAmount;

        if(_Hp <= 0)
        {
            isDestroyed = true;

            _boss.shieldBitNumber--;

            Instantiate(_explosion, transform.position, Quaternion.identity);

            _lineRenderer.enabled = false;

            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            gameObject.GetComponent<Collider2D>().enabled = false;

            transform.position = _boss.transform.position;

            StartCoroutine(Respawn());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BossLaser>())
        {
            return;
        }


        if (other.tag == "Laser" && !other.GetComponent<Laser>().isEnemyLaser && !isDestroyed)
        {

            Damage(other.GetComponent<Laser>().power);
 
            Destroy(other.gameObject);

        }
    }
}
