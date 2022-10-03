using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDiffuser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    float _duration = 10f;

    [SerializeField]
    AudioClip _diffusionSFX;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {

                AudioSource.PlayClipAtPoint(_diffusionSFX, GameObject.Find("Main Camera").transform.position, 0.3f);

                player.LaserDiffusion(_duration);

            }

            Destroy(gameObject);
        }

    }
}
