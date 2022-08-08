using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5f)
        {
            transform.position = new Vector3(Random.Range(-8f, 8f), 7f , 0);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {

            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if(playerHealth != null)
            {
                playerHealth.Damage();
            }


            Destroy(gameObject);
        }
        else if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
            Destroy(gameObject);

        }

    }

}
