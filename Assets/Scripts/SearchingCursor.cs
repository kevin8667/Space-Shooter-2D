using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingCursor : MonoBehaviour
{
    [SerializeField]
    float _searchingDuration = 3f;


    GameObject _player;

    Boss _boss;

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.Find("Player");

        _boss = FindObjectOfType<Boss>();

        StartCoroutine(SearchingTarget());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SearchingTarget()
    {
        float elapsedTime = 0;

        while(elapsedTime < _searchingDuration && _player!= null)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, 8f * Time.deltaTime);

            _boss.TurretFaceTarget();

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        _boss.StartShootingLaser();

        Destroy(gameObject, 2f);


    }

    
}
