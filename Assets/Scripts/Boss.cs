using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [HideInInspector]
    bool isShielded;

    [SerializeField]
    GameObject shield;

    [HideInInspector]
    public bool  isDestroyed;

    [SerializeField]
    BossHealth bossHealth;

    void Start()
    {
        StartCoroutine(Entrance());
       
    }

    void Update()
    {


    }

    public void ToggleShield()
    {
        shield.SetActive(isShielded);
    }


    IEnumerator Entrance()
    {
        while (transform.position.y > 2)
        {
            transform.Translate(Vector3.down * 2.5f * Time.deltaTime);

            yield return null;
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

        }
        if (other.tag == "Laser" && !other.GetComponent<Laser>().isEnemyLaser && !isDestroyed)
        {

            if (isShielded)
            {
                return;
            }

            Destroy(other.gameObject);

            bossHealth.BossDamage(other.GetComponent<Laser>().power);

        }
    }
}
