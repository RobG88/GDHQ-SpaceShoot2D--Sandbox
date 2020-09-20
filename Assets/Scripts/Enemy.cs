using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _scoreValue = 0;
    [SerializeField] float _enemySpeed = 4.0f;
    float _tempSpeed;
    [SerializeField] float _enemyLaser = 6.0f;  // speed of enemy's laser
    [SerializeField] GameObject _enemyLaserPrefab;
    float _fireRate = 3.0f;
    float _canFire = -1.0f;
    bool _isDestroyed = false; // if enemy is hit by player/ship Laser then isDestroyed = true, enemy is put back into pool

    float _enemyReSpawnThreshold = -6.0f; // Game Screen threshold, once enemy is beyond this point and has been destroyed it will respawn 
    Vector3 _enemyPos = new Vector3(0, 0, 0); // Random position of enemy once re-spawned X(-8,8) Y(12,9)

    float _respawnXmin = -9.0f;
    float _respawnXmax = 9.0f;

    Player _player;
    Animator _anim;
    BoxCollider2D _boxCollider2D;
    AudioSource _sound;

    ///
    /// FREEZE/EMP TORPEDO Secondary Fire Variable
    ///
    bool isFrozen = false;
    ///
    /// FREEZE/EMP TORPEDO Secondary Fire Variable - END
    ///

    void Start()
    {
        _anim = GetComponent<Animator>();
        _sound = GetComponent<AudioSource>();
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

        _player = Player.Instance;

        if (_anim == null)
        {
            Debug.Log("ENEMY::Start *** Animator is NULL");
        }

        if (_player == null)
        {
            Debug.Log("ENEMY::Start *** Player is NULL");
        }

        if (_boxCollider2D == null)
        {
            Debug.Log("ENEMY::Start *** BoxCollider2D is NULL");
        }

        Spawn();
    }

    void Update()
    {
        ///
        /// FREEZE/EMP TORPEDO - If enemy is frozen, can shoot, _CanFire is reset upon Thaw
        /// 
        if (Time.time > _canFire && transform.position.y < 6.5f && !isFrozen)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject EnemyLaserShot = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            EnemyLaserShot.transform.parent = this.transform;
        }
    }

    private void FixedUpdate()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.y < _enemyReSpawnThreshold && !_isDestroyed)
        {
            transform.position = SpawnEnmeyAtRandomLocation();
        }
    }

    Vector3 SpawnEnmeyAtRandomLocation()
    {
        return (_enemyPos = new Vector3(Random.Range(_respawnXmin, _respawnXmax), Random.Range(7.0f, 12.0f), 0));
    }

    bool CheckSpawnPosition(Vector3 pos)
    {
        return (Physics.CheckSphere(pos, 0.8f));
    }

    void Spawn()
    {
        float respawnX = Random.Range(_respawnXmin, _respawnXmax);
        _enemyPos.x = respawnX;
        _enemyPos.y = Random.Range(8, 12);
        transform.position = _enemyPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Laser":
                // notify score system
                _player.AddScore(_scoreValue);
                // disable Enemy BoxCollider so explosion is not able to collide
                _boxCollider2D.enabled = false;
                //_audioSource.Play();
                _sound.PlayOneShot(_sound.clip);
                ///
                /// CAMERA SHAKE
                /// 
                CinemachineShake.Instance.ShakeCamera(5f, 1f);
                ///
                /// CAMERA SHAKE
                /// 
                // trigger explosion
                _anim.SetTrigger("OnEnemyDeath");
                // destory self (enemy)
                // animation event destroys enemy object
                break;

            case "Player":
                // damage player/ship
                //_player.Damage(); // Damage handled by Player
                // notify score system
                _player.AddScore(_scoreValue);
                // disable Enemy BoxCollider so explosion is not able to collide
                _boxCollider2D.enabled = false;
                //_audioSource.Play();
                _sound.PlayOneShot(_sound.clip);
                ///
                /// CAMERA SHAKE
                /// 
                CinemachineShake.Instance.ShakeCamera(5f, 1f);
                ///
                /// CAMERA SHAKE
                /// 
                // Trigger explosion
                _anim.SetTrigger("OnEnemyDeath");
                // destory self (enemy)
                // animation event destroys enemy object
                break;

            default:
                break;
        }
    }

    void DestroyEnemyShip() // Called by Animation Event
    {
        Destroy(gameObject);
    }

    ///
    /// FREEZE/EMP TORPEDO Secondary Fire Functions
    ///
    public void FreezeEnemyShip(float speed)
    {
        _tempSpeed = _enemySpeed;
        _enemySpeed = speed;
        isFrozen = true;
    }

    public void ThawedEnemyShip()
    {
        _enemySpeed = _tempSpeed;
        isFrozen = false;
        _canFire = Time.time + _fireRate; // reset CanFire time after thaw
    }

    public bool Frozen() { return isFrozen; }
    ///
    /// FREEZE/EMP TORPEDO Secondary Fire Functions - END
    ///
}
