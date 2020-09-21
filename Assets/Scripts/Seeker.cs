using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Seeker : MonoBehaviour
{
    [SerializeField] float _speed = 2.0f;
    [SerializeField] float _rotateSpeed = 200.0f;
    [SerializeField] int invertMissle = 1;
    [SerializeField] Transform target;
    [SerializeField] List<GameObject> enemyList;
    [SerializeField] GameObject _freezeExplosion;
    [SerializeField] AudioClip _torpedoExplosionSFX;
    private Rigidbody2D rb;
    private AudioSource _sound;
    private SpriteRenderer sr;

    void Start()
    {
        target = FindClosestEnemy();
        rb = GetComponent<Rigidbody2D>();
        _sound = GetComponent<AudioSource>();
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        _sound.PlayOneShot(_sound.clip);
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * _rotateSpeed;
            rb.velocity = transform.up * _speed;
        }
        else
        {
            target = FindClosestEnemy();
        }
    }

    private Transform FindClosestEnemy()
    {
        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        float clostestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject enemy in enemyList)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < clostestDistance)
            {
                clostestDistance = currentDistance;
                trans = enemy.transform;
            }
        }
        return trans;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _sound.clip = _torpedoExplosionSFX;
            _sound.PlayOneShot(_sound.clip);
            Instantiate(_freezeExplosion, transform.position, transform.rotation);
            sr.enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            FreezeFrame();
            Destroy(this.gameObject, 2f);
        }
    }

    private void FreezeFrame()
    {
        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<Freeze>().Frozen();
        }
    }
}
