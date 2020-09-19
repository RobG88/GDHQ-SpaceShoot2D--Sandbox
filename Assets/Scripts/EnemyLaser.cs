using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] float _speed = 8;
    float _yThreshold = -7.0f;
    private AudioSource _sound;

    private void Start()
    {
        _sound = GetComponent<AudioSource>();
        _sound.PlayOneShot(_sound.clip);
    }
    void Update()
    {
        MoveDown();
        if (transform.position.y < _yThreshold)
            Destroy(transform.gameObject);
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}