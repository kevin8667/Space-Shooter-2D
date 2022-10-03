using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    enum PoweupType 
    {
        TripleShot,
        Speedup,
        Shield,
        ExtraHealth,
        HomingLaser
    };


    [SerializeField]
    PoweupType _poweupType;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    AudioClip _powerupSFX;

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

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            UIManager uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

            if (player != null)
            {

                AudioSource.PlayClipAtPoint(_powerupSFX, GameObject.Find("Main Camera").transform.position, 0.6f);

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
                    case PoweupType.ExtraHealth:
                        if(playerHealth.health < 3)
                        {
                            uIManager.UpadateLives(++playerHealth.health);
                        }
                        break;
                    case PoweupType.HomingLaser:
                        player.ActivateHomingLaser();
                        break;
                }

            }

            Destroy(gameObject);
        }
        
    }
}
