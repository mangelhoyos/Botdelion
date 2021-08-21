using System;
using UnityEngine;

public class OptimizeCameras : MonoBehaviour
{
    private Camera camaraActual;

    private Transform playerTransform;

    private void Awake()
    {
        camaraActual = GetComponent<Camera>();
    }

    private void Start()
    {
        playerTransform = PlayerMovement.INSTANCE.transform;
        PlayerMovement.OnPlayerChange += ChangeTransform;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) >= 13)
        {
            camaraActual.enabled = false;
        }
        else
        {
            camaraActual.enabled = true;
        }
    }

    public void ChangeTransform()
    {
        playerTransform = PlayerMovement.INSTANCE.transform;
    }

    
}
