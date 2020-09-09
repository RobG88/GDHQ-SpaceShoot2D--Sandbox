using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float moveSpeed = .1f;
    [SerializeField] private Transform cam;

    private void Update()
    {
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //transform.Rotate(0f, moveSpeed * Time.deltaTime, 0f);
        transform.Rotate(0f, 0f, moveSpeed * Time.deltaTime);
    }
    /*
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
    */
}
