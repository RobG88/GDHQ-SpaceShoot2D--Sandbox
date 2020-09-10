using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinShield : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        transform.Rotate(0f, moveSpeed * Time.deltaTime, 0f);
    }
}
