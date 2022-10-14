using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    #region"Numbers"

    [Header("Movement Settings")]
    [SerializeField]
    float _moveSpeed;
    [SerializeField]
    float _speedMultiplier = 2f;


    [Header("Weapon Settings")]
    [SerializeField]
    float _fireRate = 0.3f;

    float _canFire = 0f;

    public int maxAmmo = 15;

    public int ammoCount;

    [SerializeField]
    float _reloadTime = 3f;

    float _holdTimer;

    [SerializeField]
    int _bombCount = 3;

    [SerializeField]
    float _bombCoolDown = 5f;

    float _canUseBomb = 0f;

    [Header("Thruster Settings")]
    [SerializeField]
    float _fuelTime = 10f;
    float _thrusterTimer;

    #endregion

    #region"Triggers and Switches"

    bool _isTripleShot;

    bool _isHomingLaser;

    bool _isSpeedUp;

    bool _isBoosterOn;

    bool _isReloading, _isOutOfFuel;

    //bool _isBombing;

    bool _isCollecting;

    bool _isDiffused;

    [HideInInspector]
    public bool isShielded;

    #endregion

    #region"Object References"
    [Header("Shield Prefab")]
    public GameObject shield;

    [Header("Weapon Prefabs")]
    [SerializeField]
    GameObject _laser;
    [SerializeField]
    GameObject _homingLaser;
    [SerializeField]
    GameObject _tripleShot;
    [SerializeField]
    GameObject _bomb;

    [Header("Thruster Prefab")]
    [SerializeField]
    GameObject _thruster;

    Image _reloadImage, _fuelImage;
    RectTransform _canvasRect;

    [Header("Sounds")]
    [SerializeField]
    AudioClip _laserSFX, _reloadSFX;
    AudioSource _audioSource;

    PlayerHealth _playHealth;

    UIManager _uImanager;

    List<Powerup> powerups;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _moveSpeed = 5f;

        ammoCount = maxAmmo;

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
            _uImanager.UpdateAmmo(ammoCount);
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

        if (Input.GetKeyDown(KeyCode.C))
        {          
            _isCollecting = true;            
        }

        if (_isCollecting)
        {
            CollectPowerups();
        }
        
        ChangeFuelAmount(_isBoosterOn);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire)
        {
            FireLaser();

        }

        if (Input.GetKeyDown(KeyCode.B) && Time.time >= _canUseBomb)
        {
            DeployBomb();
        }
  

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isHomingLaser)
        {
            _audioSource.Play();

            Instantiate(_homingLaser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);

            return;
        }

        if (ammoCount > 0)
        {
            if (_isTripleShot)
            {
                _uImanager.UpdateAmmo(--ammoCount);

                _audioSource.Play();

                GameObject newTripleShot =  Instantiate(_tripleShot, transform.position, Quaternion.identity);

                for(int i = 0; i < newTripleShot.transform.childCount; i++)
                {

                    Laser childLaser = newTripleShot.transform.GetChild(i).GetComponent<Laser>();

                    float laserRange = childLaser.range;

                    if (_isDiffused)
                    {
                        childLaser.range = laserRange / 2;
                    }
                    else
                    {
                        childLaser.range = laserRange;
                    }

                }

            }
            else
            {
                _uImanager.UpdateAmmo(--ammoCount);

                _audioSource.Play();

                GameObject newLaser  = Instantiate(_laser, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);

                Laser laser = newLaser.GetComponent<Laser>();

                float laserRange = laser.range;

                if (_isDiffused)
                {
                    laser.range = laserRange / 2;
                }
                else
                {
                    laser.range = laserRange;
                }


            }
        }
    }


    void DeployBomb()
    {
        _canUseBomb = Time.time + _bombCoolDown;

        if(_bombCount > 0 )
        {           
            Instantiate(_bomb, new Vector2(0, 0), Quaternion.identity);

            _uImanager.UpdateBombs(--_bombCount);

        }  
    }

    void Reload()
    {

        if (Input.GetKey(KeyCode.R))
        {
            if(ammoCount < maxAmmo)
            {
                if (!_isReloading)
                {
                    _moveSpeed *= 0.5f;
                    _isReloading = true;
                }

                _reloadImage.gameObject.SetActive(true);

                _uImanager.CalculateReloadImage(transform.position);

                _holdTimer -= Time.deltaTime;

                _uImanager.UpdateImageFillAmount(_reloadImage, 1f - _holdTimer / _reloadTime);
            }

            if (_holdTimer <= 0 && ammoCount < 15)
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
        ammoCount += 5;

        AudioSource.PlayClipAtPoint(_reloadSFX, GameObject.Find("Main Camera").transform.position, 0.4f);

        if (ammoCount > maxAmmo)
        {
            ammoCount = maxAmmo;
        }

        _moveSpeed /= 0.5f;

        _isReloading = false;

        _holdTimer = _reloadTime;

        _uImanager.UpdateImageFillAmount(_reloadImage, 0);

        _uImanager.UpdateAmmo(ammoCount);

        
    }

    void CollectPowerups()
    {
        powerups = new List<Powerup>();

        foreach (Powerup powerup in FindObjectsOfType<Powerup>())
        {
            powerups.Add(powerup);
        }

        foreach (Powerup powerup in powerups)
        {
            powerup.transform.position = Vector2.MoveTowards(powerup.transform.position, transform.position, 10f * Time.deltaTime);
        }

        if (powerups.Count == 0)
        {
            _isCollecting = false;
        }

    }

    #region"Movement Related Methods"
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
        else if (Input.GetKeyUp(KeyCode.LeftShift) || _thrusterTimer <= 0)
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
        else if (!isBoosting && _thrusterTimer <= _fuelTime)
        {
            _thrusterTimer += Time.deltaTime;

            _uImanager.UpdateImageFillAmount(_fuelImage, _thrusterTimer / _fuelTime);
        }

        if (_thrusterTimer <= 0)
        {
            _isOutOfFuel = true;

            _uImanager.StartImageFlicker(_fuelImage, 0.2f, _isOutOfFuel);

        }
        else if (_thrusterTimer >= _fuelTime && _isOutOfFuel)
        {
            _thrusterTimer = _fuelTime;

            _isOutOfFuel = false;

            _uImanager.StoprImageFlicker();

            _fuelImage.enabled = true;

        }

    }
    #endregion


    #region"Powerups"
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
        switch (_playHealth.shieldHealth)
        {
            case 2:
                _playHealth.ShieldRecover();
                break;
            case 1:
                _playHealth.ShieldRecover();
                break;
            case 0:
                _playHealth.shieldHealth = 3;
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
    #endregion

    public void LaserDiffusion(float duration)
    {
        StartCoroutine(LaserDiffusionRoutine(duration));
    }

    IEnumerator LaserDiffusionRoutine(float duration)
    {

        _isDiffused = true;

        yield return new WaitForSeconds(duration);

        _isDiffused = false;
    }




}
