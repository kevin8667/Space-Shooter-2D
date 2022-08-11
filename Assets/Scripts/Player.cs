using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 3.5f;

    [SerializeField]
    GameObject _laser;

    [SerializeField]
    GameObject _tripleShot;

    [SerializeField]
    float _fireRate = 0.3f;
    float _canFire = 0f;

    [SerializeField]
    bool _isTripleShot;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire)
        {
            FireLaser();
        }

    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShot == true)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
    }

    private void Movement()
    {
        // Get input from the user
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //Move the Player
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _moveSpeed * Time.deltaTime);

        //if x bigger than a value
        if (transform.position.x > 10f)
        {
            //stop the player
            transform.position = new Vector3(-10f, transform.position.y, 0);
        }
        // if the condition doesn't match the first one, check this one
        else if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }

        if (transform.position.y > 6f)
        {
            transform.position = new Vector3(transform.position.x, 6f, 0);
        }
        else if (transform.position.y < -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }
    }

    public void ActivateTripleShot()
    {
        StartCoroutine(TripleShotPoweup());
    }

    IEnumerator TripleShotPoweup()
    {
        _isTripleShot = true;
        yield return new WaitForSeconds(5f);
        _isTripleShot = false;

    }
}
