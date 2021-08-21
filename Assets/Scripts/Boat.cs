using System;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D _collider;
    private bool isReady = false;
    public float speed;
    



    private void Update()
    {
        if (isReady)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position,target.position) <= 0.5f)
            {
                _collider.enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isReady = true;
            other.transform.SetParent(transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
