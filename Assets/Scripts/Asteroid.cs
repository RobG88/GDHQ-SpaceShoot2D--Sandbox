using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidBehaviorType { Giant, Sin, Cos, Tan, Sinh, Cosh, Tanh };
    public AsteroidBehaviorType AsteroidBehavior;

    [SerializeField]
    float _rotateSpeed = 3.0f;
    [SerializeField]
    float _speed;

    Animator _anim;
    GameManager _gameManager;
    AudioSource _audioSource;
    //AudioManager _audioManager;
    Player _player;

    [SerializeField] bool _initialAsteroid = false;

    void Start()
    {
        if (AsteroidBehavior == AsteroidBehaviorType.Giant)
        {
            //SetAsteroidAttributes();
            transform.localScale = new Vector3(Random.Range(2.0f, 3f), Random.Range(1.75f, 2.65f));
        }
        else
        {
            _rotateSpeed = 0;// Random.Range(-4, 4);
            _speed = 0.10f;
        }
        /*
        if (!_initialAsteroid) // Common asteroids have random attributes
        {
            SetAsteroidAttributes();
        }
        else
        {
            _rotateSpeed = 0;// Random.Range(-4, 4);
            _speed = 0.10f;
        }
        */
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        //_audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        _player = Player.Instance;
        //_player = FindObjectOfType<Player>().GetComponent<Player>();

        if (_anim == null)
        {
            Debug.Log("ASTEROID::Start *** ERROR: Animator is Null!");
        }
        /*
        if (_audioManager == null)
        {
            Debug.Log("ASTEROID::Start *** ERROR: AudioSource is Null!");
        }
        */
        if (_gameManager == null)
        {
            Debug.Log("ASTEROID::Start *** ERROR: GameManager is Null");
        }
        /*
        if (_player != null)
        {
            Debug.Log("ASTEROID::Start *** ERROR: Player is Null");
        }
        */
    }

    void Update()
    {
        RotateAsteroid();
        EuclideanTorus();
    }

    void SetAsteroidAttributes()
    {
        _rotateSpeed = Random.Range(-4, 4);
        _speed = Random.Range(-1.25f, 1.8f);//1,3);

    }

    void RotateAsteroid()
    {
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        int x = 0;
        int y = 0;
        int z = 1;
        transform.Rotate(x, y, z);

        Vector2 pos = transform.position;
        float _anotherspeed = 1.0f;
        //pos.y = pos.y - Time.deltaTime * _anotherspeed;
        pos.y = pos.y - Time.deltaTime * _speed;

        switch (AsteroidBehavior)
        {
            case AsteroidBehaviorType.Sin:
                pos.x = Mathf.Sin(pos.y) * 4;
                break;
            case AsteroidBehaviorType.Tan:
                pos.x = Mathf.Tan(pos.y) * 5;
                break;
            case AsteroidBehaviorType.Cos:
                pos.x = Mathf.Cos(pos.y) * 3;
                break;
            case AsteroidBehaviorType.Sinh:
                pos.x = Mathf.Sin(pos.y) * 2;
                break;
            case AsteroidBehaviorType.Tanh:
                pos.x = Mathf.Tan(pos.y) * 1;
                break;
            case AsteroidBehaviorType.Cosh:
                pos.x = Mathf.Cos(pos.y) * 7;
                break;
            case AsteroidBehaviorType.Giant:
                pos.x = 0;
                break;
        }

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Asteroid collided with: " + other.tag);

        if (other.tag == "Laser" || other.tag == "Player")
        {
            this.GetComponent<CircleCollider2D>().enabled = false;
            Destroyed();
            _audioSource.Play();
        }
    }

    void Destroyed()
    {
        ///
        /// CAMERA SHAKE via Cinemachine
        /// 
        CinemachineShake.Instance.ShakeCamera(12.0f, 3.0f);

        // Animate Explosion
        _anim.SetTrigger("AsteroidDestroyed");

        // Play Audio Explosion SFX
        //_audioManager.PlayExplosion();

        // Let GameManager know an object was destroyed
        _gameManager.DestroyedAsteroid();
    }
    public void DestoryAsteroid()
    {
        // Destory Asteroid after Animation via animation event
        Destroy(gameObject);
    }

    void EuclideanTorus()
    {
        // Teleport the astreroid from botton to top and
        // left to right and right to left

        if (transform.position.x > 10.35f)
        {
            transform.position = new Vector3(-10.35f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.35f)
        {
            transform.position = new Vector3(10.35f, transform.position.y, 0);
        }
        else if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(transform.position.x, 7.0f, 0);
        }
    }
}
