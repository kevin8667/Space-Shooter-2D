using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    float _speedMultiplier = 2f;

    [SerializeField]
    GameObject _laser;

    [SerializeField]
    GameObject _tripleShot;

    [SerializeField]
    float _fireRate = 0.3f;
    float _canFire = 0f;

    [SerializeField]
    bool _isTripleShot;

    [SerializeField]
    bool _isSpeedUp;

    bool _isBoosterOn;

    public bool isShielded;

    public GameObject shield;

    [SerializeField]
    GameObject _thruster;

    [SerializeField]
    AudioClip _laserSFX;

    AudioSource _audioSource;

    PlayerHealth _playHealth;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _moveSpeed = 5f;

        _audioSource = gameObject.GetComponent<AudioSource>();

        _playHealth = gameObject.GetComponent<PlayerHealth>();

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL!");
        }
        else
        {
            _audioSource.clip = _laserSFX;
        }

        if (_playHealth == null)
        {
            Debug.LogError("The Player Health is NULL!");
        }

    }

    // Update is called once per frame
    void Update()
    {

        Ignition();

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

            _audioSource.Play();

            Instantiate(_tripleShot, transform.position, Quaternion.identity);

        }
        else
        {
            _audioSource.Play();
            Instantiate(_laser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
    }

    private void Movement()
    {
        // Get input from the user
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Move the Player
        if (_isBoosterOn)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_moveSpeed * 1.5f) * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _moveSpeed * Time.deltaTime);
        }
        

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


    public void ActivateSpeedup()
    {
        if (!_isSpeedUp)
        {
            StartCoroutine(SpeedupPoweup());
        }
        
    }

    IEnumerator SpeedupPoweup()
    {

        _moveSpeed *= _speedMultiplier;
        _isSpeedUp = true;

        if (_isBoosterOn)
        {
            ToggleBoost(true);
        }
        else
        {
            _thruster.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

        _isSpeedUp = false;
        _moveSpeed /= _speedMultiplier;

        if (_isBoosterOn)
        {
            ToggleBoost(false);
        }
        else
        {
            _thruster.SetActive(false);
        }
        

    }

    public void ActivateShield()
    {
        switch (_playHealth._shieldHealth)
        {
            case 2:
                _playHealth.ShieldRecover();
                break;
            case 1:
                _playHealth.ShieldRecover();
                break;
            case 0:
                _playHealth._shieldHealth = 3;
                _playHealth.SetShield();
                isShielded = true;
                shield.SetActive(true);
                break;
        }    
    }

    public void StopMoving()
    {
        _moveSpeed = 0;
    }

    void Ignition()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isBoosterOn = true;

            if (_isSpeedUp)
            {
                ToggleBoost(true);
            }
            else
            {
                _thruster.SetActive(true);
            }

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isBoosterOn = false;

            if (_isSpeedUp)
            {
                ToggleBoost(false);
            }
            else
            {
                _thruster.SetActive(false);
            }

        }
    }

    void ToggleBoost(bool isBoost)
    {
        if (isBoost)
        {
            _thruster.transform.position = new Vector3(_thruster.transform.position.x, _thruster.transform.position.y - .75f, _thruster.transform.position.z);
            _thruster.transform.localScale = new Vector3(_thruster.transform.localScale.x, _thruster.transform.localScale.y + .5f, _thruster.transform.localScale.z);
        }
        else
        {
            _thruster.transform.position = new Vector3(_thruster.transform.position.x, _thruster.transform.position.y + .75f, _thruster.transform.position.z);
            _thruster.transform.localScale = new Vector3(_thruster.transform.localScale.x, _thruster.transform.localScale.y - .5f, _thruster.transform.localScale.z);
        }   
    }

}
