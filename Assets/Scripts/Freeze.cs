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
    private void Awake()
    {
        _originalMat = gameObject.GetComponent<SpriteRenderer>().material;
        _cloakingMat = _originalMat;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            gameObject.GetComponent<SpriteRenderer>().material = _frozenMat;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.GetComponent<SpriteRenderer>().material = _originalMat;
        }
    }
}
