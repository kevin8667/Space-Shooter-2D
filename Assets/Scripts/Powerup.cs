using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    enum PoweupType 
    {
        TripleShot,
        Speedup,
        Shield
    };

    [SerializeField]
    PoweupType _poweupType;

    [SerializeField]
    private float _speed = 3f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();

            if(player != null)
            {

                switch (_poweupType)
                {
                    case PoweupType.TripleShot:
                        player.ActivateTripleShot();
                        break;
                    case PoweupType.Speedup:
                        player.ActivateSpeedup();
                        break;
                    case PoweupType.Shield:
                        player.ActivateShield();
                        break;
                }

                
            }
            
            Destroy(gameObject);
        }
        
    }
}
