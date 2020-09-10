using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Singleton
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("PLAYER::Singleton *** ERROR: Instance is NULL");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    [SerializeField] int _playerLives = 3;

    [SerializeField] int _score = 0;

    [SerializeField] float _speed; // player/ship movement speed
    [SerializeField] float _spaceshipSpeed = 6.0f;  // player/ship CONSTANT speed
                                                    // speed lost for damage

    [SerializeField] float _fireRate = 0.15f;  // delay (in Seconds) how quickly the laser will fire
    float _nextFire = -1.0f;  // game time value, tracking when player/ship can fire next laser

    float horizontalInput, verticalInput;

    [SerializeField] bool _wrapShip = false; // Q = toggle wrap

    [SerializeField] bool _gameOver = false;

    [SerializeField] GameObject _powerUpCountDownBar;
    [SerializeField] Text _timesUpText;

    //float _xScreenClampRight = 10.75f; // Original setting
    //float _xLeftBoundry = -10.75f; // Original setting

    float _xScreenClampRight = 9.5f;
    float _xLeftBoundry = -9.5f;
    float _yScreenClampUpper = 0;
    float _yScreenClampLower = -4.5f;

    [SerializeField] GameObject _thruster_left;
    [SerializeField] GameObject _thruster_right;
    Vector3 _originalThrustersLocalScale;

    [SerializeField] GameObject _shipDamageLeft;
    [SerializeField] GameObject _shipDamageRight;
    bool _damagedLeft = false;
    bool _damagedRight = false;
    Animator _animShipDamageLeft;
    Animator _animShipDamageRight;

    [SerializeField] GameObject _laserPrefab;
    [SerializeField] Vector3 _laserOffset = new Vector3(0, 1.20f, 0); // distance offset when spawning laser Y-direction

    [SerializeField] bool _tripleShotActive = false;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] float _tripleShotCoolDown = 5.0f;

    [SerializeField] float _speedCoolDown = 5.0f;
    [SerializeField] bool _speedActive = false;

    [SerializeField] bool _shieldActive = false;
    [SerializeField] GameObject _shield;
    [SerializeField] int _shieldPower = 2;
    Vector3 _shieldOriginalSize;

    [SerializeField] AudioClip _laserSFX;
    [SerializeField] AudioClip _PowerUpSFX;
    [SerializeField] AudioClip _explosion;
    [SerializeField] GameObject PlayerFinalExplosionPE;

    [SerializeField] private int _maxAmmo, _currentAmmo;
    [SerializeField] private float _maxThrusters, _currentThrusters;
    private float _thrusterBurnRate = 2.5f;
    [SerializeField] private bool _enableMainThrusters;
    [SerializeField] private bool _regeneratingThrusters;
    private float _thrustersInitialRegenDelay = 2.0f; // wait 2.0f seconds before THRUSTERS begin to regenerate
    private float _thrustersRegenTick = 0.1f;
    private WaitForSeconds _thrustersRegenDelay;
    private WaitForSeconds _thrustersRegenTickDelay;
    //private Coroutine _regenThrusters;

    Animator _anim; // Player Death Explosion with Event to clean-up
    AudioSource _audioSource; // Audio source for laser, player damage & powerups

    void Start()
    {
        _playerLives = 3;
        _gameOver = false;
        transform.position = new Vector3(0, -3.5f, 0);

        _anim = GetComponent<Animator>();
        _animShipDamageLeft = _shipDamageLeft.GetComponent<Animator>();
        _animShipDamageRight = _shipDamageRight.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        // Initialize Player/Ship variables & set GUI
        // Ship speed
        // At the start of the game the ship has taken damage to both 
        // port & starboard sides so speed will reduced by Damage(), Damage()
        _speed = _spaceshipSpeed; // initialize Ship/Player speed
        // Laser Cannon Ammo
        _currentAmmo = 5;
        _maxAmmo = 15;
        UIManager.Instance.SetMaxAmmo(_maxAmmo);
        UIManager.Instance.SetAmmo(_currentAmmo);
        // Thrusters
        _maxThrusters = 10.0f;
        _currentThrusters = 10.0f; // TODO: disable for beginning of game
        _thrustersRegenDelay = new WaitForSeconds(_thrustersInitialRegenDelay);
        _thrustersRegenTickDelay = new WaitForSeconds(_thrustersRegenTick);
        UIManager.Instance.SetMaxThrusters(_maxThrusters);
        UIManager.Instance.SetThrusters(_currentThrusters);

        UIManager.Instance.UpdatePlayerLives(_playerLives);
        _shieldOriginalSize = _shield.transform.localScale;
        _originalThrustersLocalScale = _thruster_left.transform.localScale;

        GameObjectNullChecks();
    }

    private void GameObjectNullChecks()
    {
        if (_animShipDamageLeft == null)
        {
            Debug.LogError("PLAYER::Start *** ERROR: Animator ShipDamageLeft is Null!");
        }

        if (_animShipDamageRight == null)
        {
            Debug.LogError("PLAYER::Start *** ERROR: Animator ShipDamageRight is Null!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("PLAYER::Start *** ERROR: AudioSource is Null!");
        }

        if (_anim == null)
        {
            Debug.LogError("PLAYER:Start *** ERROR: Animator is Null!");
        }
    }

    void Update() // Fire Laser Cannons (SPACE) & Thrusters (LeftShift)
    {
        if (!_gameOver)
        {
            if (RegenThrusters() && !_enableMainThrusters)
            {
                StartCoroutine(RegeneratorThrusters());
            }

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && Ammo())
            {
                FireLaser();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Thrusters())
            {
                EnableMainThrusters();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                DisableMainThrusters();
            }

            // Below is for testing purposes only
            // CHEAT KEYS
            if (Input.GetKeyDown(KeyCode.Q)) { _wrapShip = !_wrapShip; }
            if (Input.GetKeyDown(KeyCode.B)) { _tripleShotActive = !_tripleShotActive; }
        }
    }

    private void FixedUpdate() // Calculate Movement
    {
        if (!_gameOver)
            CalculateMovement();
    }

    private void FireLaser() // Fire Laser Cannons & TripleShot
    {
        _nextFire = Time.time + _fireRate; // delay (in Seconds) how quickly the laser will fire
        _currentAmmo--;
        UIManager.Instance.SetAmmo(_currentAmmo);
        if (!_tripleShotActive) // Tripleshoot PowerUp
        {
            Vector3 _laserOrigin = transform.position + _laserOffset;
            Instantiate(_laserPrefab, _laserOrigin, Quaternion.identity);
            _audioSource.pitch = Random.Range(2.5f, 3.0f);
        }

        if (_tripleShotActive)
        {
            Vector3 _tripleOrigin = transform.position;
            GameObject TripleShot = Instantiate(_tripleShotPrefab, _tripleOrigin, Quaternion.identity);
            Destroy(TripleShot, 1.5f);
            _audioSource.pitch = 1.0f;
        }

        _audioSource.PlayOneShot(_laserSFX, 0.50f);
    }

    private void CalculateMovement() // Ship movement, animate thrusters & screen clamps
    {
        ////////////////////////////////
        /// CalculateMovement function
        /// 
        /// Using pre-defined Unity Axis Horizontal/Vertical
        /// Move player/ship in the desired direction 
        /// Clamping vertical between xScreenClampUpper & xScreenClampLower
        /// Wraping horizontal between yScreenClampRight & yScreenClampLeft
        /// 

        _speed = CalculateShipSpeed();

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Animate Ship Tilting/Banking
        _anim.SetFloat("Tilt", horizontalInput); // * 2.0f);

        // Use the verticalInput 'W' or UpArrow * 1.75 as Thruster localScale multiplier
        float thruster_y = verticalInput * 1.75f;

        if (verticalInput > 0.20f)
        {
            // Reset Thrusters/Afterburners to originalLocalScale so when Afterburners 
            // do not over extend graphically under Spaceship
            _thruster_left.transform.localScale = _originalThrustersLocalScale;
            _thruster_right.transform.localScale = _originalThrustersLocalScale;

            Vector3 thrusters = new Vector3(_thruster_left.transform.localScale.x,
                                            thruster_y,
                                            _thruster_left.transform.localScale.z);

            _thruster_left.transform.localScale = thrusters; // Afterburner left
            _thruster_right.transform.localScale = thrusters; // Afterburner right
        }
        else if (verticalInput < 0.20f) // if the verticalInput < .20 then 'flicker' thrusters
        {
            float _thrustersAnimation = Random.Range(1.25f, 1.50f);
            _thruster_left.transform.localScale = _originalThrustersLocalScale * _thrustersAnimation;
            _thruster_right.transform.localScale = _originalThrustersLocalScale * _thrustersAnimation;
        }

        // Move player/ship along X & Y Axis
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        // Player Boundaries 
        // Clamp Ship's Y pos
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yScreenClampLower, _yScreenClampUpper), 0);

        // Clamp xPos
        if (_wrapShip && transform.position.x > _xScreenClampRight) // Wrap ship
        {
            transform.position = new Vector3(_xLeftBoundry, transform.position.y, 0);
        }
        else if (!_wrapShip && transform.position.x > _xScreenClampRight) // Lock pos
        {
            transform.position = new Vector3(_xScreenClampRight, transform.position.y, 0);
        }

        // or Wrap Ship's X pos
        if (_wrapShip && transform.position.x < _xLeftBoundry) // Wrap ship
        {
            transform.position = new Vector3(_xScreenClampRight, transform.position.y, 0);
        }
        else if (!_wrapShip && transform.position.x < _xLeftBoundry) // Lock pos 
        {
            transform.position = new Vector3(_xLeftBoundry, transform.position.y, 0);
        }
    }

    void PlayerDeath() // called via PlayerDeathExplosion animation
    {
        GameManager.Instance.GameOver();
        SpawnManager.Instance.OnPlayerDeath();
        UIManager.Instance.DisplayGameOver();
        Destroy(this.gameObject);
    }

    public void Damage() // ship & shield damage
    {
        if (_shieldActive)
        {
            _audioSource.PlayOneShot(_explosion);
            if (_shieldPower > 0)
            {
                _shieldPower--;
                _shield.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
            }
            if (_shieldPower == 0)
            {
                _shieldActive = false;
                _shieldPower = 2;
                _shield.transform.localScale = _shieldOriginalSize;
                _shield.SetActive(false);
            }
        }
        else
        {
            _playerLives--;
            UIManager.Instance.UpdatePlayerLives(_playerLives);

            if (_playerLives < 1)
            {
                _gameOver = true;
                CinemachineShake.Instance.ShakeCamera(16f, 4f);
                PlayerDeathSequence();
                return;
            }

            CinemachineShake.Instance.ShakeCamera(5f, 1f);
            SpaceshipDamaged();
        }
    }

    public void PlayerDeathSequence() // player death final sequence
    {
        if (_shieldActive) {
            _shieldPower = 0;
            _shieldActive = false;
            _shield.transform.localScale = _shieldOriginalSize;
            _shield.SetActive(false);
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        _audioSource.PlayOneShot(_explosion);
        _thruster_left.SetActive(false);
        _thruster_right.SetActive(false);
        _shipDamageLeft.SetActive(false);
        _shipDamageRight.SetActive(false);
        // Play Explosion animation, final Player Death sequence via animation event
        _anim.SetTrigger("GameOver");
        PlayerFinalExplosionPE.SetActive(true);
    }

    void SpaceshipDamaged() // if player ship is hit, damage port or starboard
    {
        if (!_damagedLeft && !_damagedRight)
        {
            int RND_Damage = Random.Range(0, 2);
            if (RND_Damage == 0)
            {
                SpaceshipDamagedLeft();
            }
            else if (RND_Damage == 1)
            {
                SpaceshipDamagedRight();
            }
        }
        else if (_damagedLeft && !_damagedRight)
        {
            SpaceshipDamagedRight();
        }
        else if (!_damagedLeft && _damagedRight)
        {
            SpaceshipDamagedLeft();
        }
    }

    private void SpaceshipDamagedLeft() // ship port side damage
    {
        _damagedLeft = true;
        _shipDamageLeft.SetActive(true);
        _animShipDamageLeft.SetTrigger("PlayerDamageLeft");
        _audioSource.PlayOneShot(_explosion);
    }

    private void SpaceshipDamagedRight() // ship starboard side damage
    {
        _damagedRight = true;
        _shipDamageRight.SetActive(true);
        _animShipDamageRight.SetTrigger("PlayerDamageRight");
        _audioSource.PlayOneShot(_explosion);
    }

    private void OnTriggerEnter2D(Collider2D other) // collisions
    {
        if (other.tag == "Enemy" || other.tag == "EnemyLaser")
        {
            Damage();
        }

        if (other.tag == "PowerUp")
        {
            string PowerUpToActivate;
            PowerUpToActivate = other.transform.GetComponent<PowerUp>().PowerType().ToString();
            // TODO: Need to Handle Multiple PowerUps?
            ActivatePowerUp(PowerUpToActivate);
        }

        if (other.tag == "Asteroid") // Kill player if hit by Giant Asteroid
        {
            _gameOver = true;
            PlayerDeathSequence();
        }
    }

    void ActivatePowerUp(string _powerUpType) // PowerUp activations
    {
        _timesUpText.text = _powerUpType;
        switch (_powerUpType)
        {
            case "TripleShot":
                _tripleShotActive = true;
                //_timesUpText.text = _powerUpType;
                // Enable PowerUpCountDownBar
                _powerUpCountDownBar.SetActive(true);
                StartCoroutine(PowerUpCoolDownRoutine(_tripleShotCoolDown));
                break;
            case "Speed":
                _speedActive = true;
                // Enable PowerUpCountDownBar
                _powerUpCountDownBar.SetActive(true);
                StartCoroutine(PowerUpCoolDownRoutine(_speedCoolDown));
                break;
            case "Shield":
                _shieldPower = 2; // # of hits before shield is destroyed
                _shield.transform.localScale = _shieldOriginalSize; // reset shield graphic to initial size
                _shieldActive = true;
                _shield.SetActive(true); // enable the Shield gameObject
                break;
            case "EnergyCell":
                _currentAmmo = _currentAmmo + 25; // # of hits before shield is destroyed
                if (_currentAmmo > 15)
                    _currentAmmo = 15;
                UIManager.Instance.SetAmmo(_currentAmmo);
                break;
        }
        _audioSource.pitch = 1.0f;
        _audioSource.PlayOneShot(_PowerUpSFX);
    }

    IEnumerator PowerUpCoolDownRoutine(float coolDown) // PowerUp Cooldown
    {
        yield return new WaitForSeconds(coolDown);
        _tripleShotActive = false;
        _speedActive = false;
    }

    float CalculateShipSpeed() // Ship's speed = _spaceshipSpeed, calc PowerUp, damage 
    {
        var _newSpeed = _spaceshipSpeed;

        if (_playerLives == 2)
        {
            _newSpeed = _spaceshipSpeed - 1;
        }

        if (_playerLives == 1)
        {
            _newSpeed = _spaceshipSpeed - 2;
        }

        if (_speedActive) // PowerUp = speed * 175%
        {
            _newSpeed = _spaceshipSpeed * 1.75f;
        }

        if (_enableMainThrusters) // Thrusters = speed * 250%
        {
            _newSpeed = _spaceshipSpeed * 2.50f;
        }

        return _newSpeed;
    }

    public void AddScore(int scoreAmount) // Update score, send to UI
    {
        _score += scoreAmount;
        UIManager.Instance.UpdateScore(_score);
    }

    private bool Ammo() // True, if ship has Laser Cannon Energy
    {
        return (_currentAmmo > 0);
    }
    private bool Thrusters() // True, if ship has Main Thruster power
    {
        return (_currentThrusters > 0);
    }
    private bool RegenThrusters()
    {
        return (_currentThrusters < _maxThrusters);
    }
    private void EnableMainThrusters()
    {
        _regeneratingThrusters = false;
        _enableMainThrusters = true;

        if (_currentThrusters > 0)
        {
            _currentThrusters -= _thrusterBurnRate * Time.deltaTime;

            if (_currentThrusters <= 0)
            {
                _currentThrusters = 0;
                _enableMainThrusters = false;

                _speed = CalculateShipSpeed();
            }

            UIManager.Instance.SetThrusters(_currentThrusters);
        }
    }

    private void DisableMainThrusters()
    {
        _enableMainThrusters = false;
    }
    private IEnumerator RegeneratorThrusters()
    {
        _regeneratingThrusters = true;

        yield return _thrustersRegenDelay; // cahced WaitForSeconds(_thrustersInitialRegenDelay)

        while (_currentThrusters < _maxThrusters && _regeneratingThrusters)
        {
            _currentThrusters += _maxThrusters / 100000;
            UIManager.Instance.SetThrusters(_currentThrusters);
            yield return _thrustersRegenTickDelay;
        }

        _regeneratingThrusters = false;
    }
}