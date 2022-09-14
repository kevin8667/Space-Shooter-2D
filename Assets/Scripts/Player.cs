using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    float _speedMultiplier = 2f;

    [SerializeField]
    float _fireRate = 0.3f;
    
    float _canFire = 0f;

    [SerializeField]
    float _reloadTime = 3f;

    float _holdTimer;

    [SerializeField]
    float _fuelTime = 10f;

    float _thrusterTimer;

    public int _ammoCount = 15;

    bool _isTripleShot;

    bool _isHomingLaser;

    bool _isSpeedUp;

    bool _isBoosterOn;

    bool _isReloading, _isOutOfFuel;

    public bool isShielded;

    public GameObject shield;

    [SerializeField]
    GameObject _laser, _homingLaser;

    [SerializeField]
    GameObject _tripleShot;

    [SerializeField]
    GameObject _thruster;

    Image _reloadImage, _fuelImage;

    RectTransform _canvasRect;

    [SerializeField]
    AudioClip _laserSFX, _reloadSFX;

    AudioSource _audioSource;

    PlayerHealth _playHealth;

    UIManager _uImanager;

    Vector2 _screenPoint;

    Vector2 _canvasPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _moveSpeed = 5f;

        _holdTimer = _reloadTime;

        _thrusterTimer = _fuelTime;

        _audioSource = gameObject.GetComponent<AudioSource>();

        _playHealth = gameObject.GetComponent<PlayerHealth>();

        _uImanager = GameObject.Find("UIManager").GetComponent<UIManager>();

        _reloadImage = _uImanager.ReloadImage;

        _fuelImage = _uImanager.FuelImage;

        _canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();

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

        if (_uImanager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }
        else
        {
            _uImanager.UpdateAmmo(_ammoCount);
        }

        if (_reloadImage == null)
        {
            Debug.LogError("The Reload Image is NULL!");
        }
        else
        {
            _reloadImage.fillAmount = 0;
            _reloadImage.gameObject.SetActive(false);
        }

        if(_fuelImage == null)
        {
            Debug.LogError("The Fuel Image is NULL!");

        }
        else
        {
            _fuelImage.fillAmount = 1;
        }

        if (_canvasRect == null)
        {
            Debug.LogError("The Canvas is NULL!");
        } 

    }

    // Update is called once per frame
    void Update()
    {
        Ignition();

        Movement();

        Reload();

        ChangeFuelAmount(_isBoosterOn);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire)
        {
            FireLaser();

        }

        if (Input.GetKeyDown(KeyCode.H) && Time.time >= _canFire)
        {
            Instantiate(_homingLaser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);

        }

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_ammoCount > 0)
        {
            if (_isTripleShot)
            {
                _uImanager.UpdateAmmo(--_ammoCount);

                _audioSource.Play();

                Instantiate(_tripleShot, transform.position, Quaternion.identity);

            }
            else if (_isHomingLaser)
            {
                _audioSource.Play();
                
                Instantiate(_homingLaser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            }
            else
            {
                _uImanager.UpdateAmmo(--_ammoCount);

                _audioSource.Play();

                Instantiate(_laser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            }
        }
    }

    void CalculateReloadImage()
    {
        //get the position of the Player in screen space
        _screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        //transfrom the Player's position in screen space to the position in the local space of a RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, _screenPoint, null, out _canvasPos);

        //move the reaload image to the position of the corresponding Player's position on the canvas
        _reloadImage.transform.localPosition = _canvasPos; 
    }

    void Reload()
    {

        if (Input.GetKey(KeyCode.R))
        {
            if(_ammoCount < 15)
            {
                if (!_isReloading)
                {
                    _moveSpeed *= 0.5f;
                    _isReloading = true;
                }

                _reloadImage.gameObject.SetActive(true);

                CalculateReloadImage();

                _holdTimer -= Time.deltaTime;

                _uImanager.UpdateImageFillAmount(_reloadImage, 1f - _holdTimer / _reloadTime);

                //_reloadImage.fillAmount = 1f - _holdTimer / _reloadTime;
            }

            if (_holdTimer <= 0 && _ammoCount < 15)
            {  
                FinishingReload();
            }
        }
        else if(Input.GetKeyUp(KeyCode.R))
        {
            if (_isReloading)
            {
                _moveSpeed /= 0.5f;
                _isReloading = false;
            }

            _reloadImage.gameObject.SetActive(false);

            _uImanager.UpdateImageFillAmount(_reloadImage, 0);

            _holdTimer = _reloadTime;
        }

        
    }

    void FinishingReload()
    {
        _ammoCount += 5;

        AudioSource.PlayClipAtPoint(_reloadSFX, GameObject.Find("Main Camera").transform.position, 0.4f);

        if (_ammoCount > 15)
        {
            _ammoCount = 15;
        }

        _moveSpeed /= 0.5f;

        _isReloading = false;

        _holdTimer = _reloadTime;

        _uImanager.UpdateImageFillAmount(_reloadImage, 0);

        _uImanager.UpdateAmmo(_ammoCount);

        
    }

    void Movement()
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
        if (!_isHomingLaser)
        {
            StartCoroutine(TripleShotPoweup());
        }
        
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

    public void ActivateHomingLaser()
    {
        if (!_isTripleShot)
        {
            StartCoroutine(HomingLaserPoweup());
        }
        
    }

    IEnumerator HomingLaserPoweup()
    {
        _isHomingLaser = true;

        yield return new WaitForSeconds(5f);

        _isHomingLaser = false;
    }

    void Ignition()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isOutOfFuel)
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
        else if (Input.GetKeyUp(KeyCode.LeftShift) || _thrusterTimer <=0)
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
            _thruster.transform.localPosition = new Vector3(_thruster.transform.localPosition.x, -4.25f, _thruster.transform.localPosition.z);
            _thruster.transform.localScale = new Vector3(_thruster.transform.localScale.x, 1.5f, _thruster.transform.localScale.z);

        }
        else
        {
            _thruster.transform.localPosition = new Vector3(_thruster.transform.localPosition.x, -3.5f, _thruster.transform.localPosition.z);
            _thruster.transform.localScale = new Vector3(_thruster.transform.localScale.x, 1f, _thruster.transform.localScale.z);
        }   
    }

    void ChangeFuelAmount(bool isBoosting)
    {
        
        if (isBoosting && _thrusterTimer > 0)
        {
            _thrusterTimer -= Time.deltaTime;

            _uImanager.UpdateImageFillAmount(_fuelImage, _thrusterTimer / _fuelTime);
           
        }
        else if(!isBoosting && _thrusterTimer <= _fuelTime)
        {
            _thrusterTimer += Time.deltaTime;

            _uImanager.UpdateImageFillAmount(_fuelImage, _thrusterTimer / _fuelTime);
        }

        if (_thrusterTimer <= 0)
        {
            _isOutOfFuel = true;

            _uImanager.StartImageFlicker(_fuelImage, 0.2f, _isOutOfFuel);

        }else if (_thrusterTimer >= _fuelTime && _isOutOfFuel)
        {
            _thrusterTimer = _fuelTime;

            _isOutOfFuel = false;

            _uImanager.StoprImageFlicker();

            _fuelImage.enabled = true;

        }

    }

}
