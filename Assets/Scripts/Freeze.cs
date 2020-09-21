using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    // Freeze 
    //
    // Freeze enemy with 'F'reeze
    // Un-freeze enemy with 'R'estore
    //
    [SerializeField] Material _originalMat;
    [SerializeField] Material _frozenMat;
    [SerializeField] Material _cloakingMat;
    Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        _originalMat = gameObject.GetComponent<SpriteRenderer>().material;
        _cloakingMat = _originalMat;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            EncasedInIce();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Thawed();
        }
    }

    private void Thawed()
    {
        gameObject.GetComponent<SpriteRenderer>().material = _originalMat;
    }

    private void EncasedInIce()
    {
        gameObject.GetComponent<SpriteRenderer>().material = _frozenMat;
    }

    public void Frozen()
    {
        StartCoroutine(IceIce());
    }

    IEnumerator IceIce()
    {
        EncasedInIce();
        enemy.FreezeEnemyShip(.25f);

        yield return new WaitForSeconds(6f);

        Thawed();
        enemy.ThawedEnemyShip();
    }
}
