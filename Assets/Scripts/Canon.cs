using Unity.Mathematics;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public Rigidbody2D canonBall;
    public Transform target;
    public Transform shootPoint;
    public GameObject particles;

    private AudioSource _audioSource;

    private bool used = false;
    
    public static Canon INSTANCE { get; private set; }

    private void Awake()
    {
        INSTANCE = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (!used)
        {
            used = true;
            _audioSource.Play();
            Rigidbody2D proyectile = Instantiate(canonBall, shootPoint.position, Quaternion.identity);
            float AngleRad = Mathf.Atan2(target.position.y - proyectile.transform.position.y,
                target.position.x - proyectile.transform.position.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            proyectile.transform.rotation = Quaternion.Euler(0, 0, AngleDeg + 90);
            Vector2 speedDir = target.position - proyectile.transform.position;
            proyectile.velocity = speedDir.normalized * 20f;
            GameObject ps = Instantiate(particles, shootPoint.position, quaternion.identity);
            Destroy(proyectile.gameObject, 4f);
        }
    }
}
