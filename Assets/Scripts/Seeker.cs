using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Seeker : MonoBehaviour
{
    [SerializeField] float _speed = 3.0f;
    [SerializeField] float _rotateSpeed = 200.0f;
    [SerializeField] int invertMissle = 1;
    [SerializeField] Transform target;
    [SerializeField] List<GameObject> enemyList;
    [SerializeField] GameObject _freezeExplosion;
    private Rigidbody2D rb;

    void Start()
    {
        target = FindClosestEnemy();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * _rotateSpeed;
        rb.velocity = transform.up * _speed;
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
            //Instantiate(_freezeExplosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
