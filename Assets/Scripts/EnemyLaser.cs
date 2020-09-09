using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] float _speed = 8;
    float _yThreshold = -6.0f;

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
    /*
    void OnBecameInvisible()
    {
        if (transform.parent != null) // if object has a parent
        {
            Destroy(transform.parent.gameObject, 0.1f);
        }
        else
        {
            Destroy(transform.gameObject, 0.1f);
        }
    }
    */
}