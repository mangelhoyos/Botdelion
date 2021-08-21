using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IPooledObject
{
    #region CashVariables
    private BoxCollider2D sBoxCol;
    private CircleCollider2D sCircleCol;
    private CinemachineVirtualCamera sCinemaChine;
    private Battery sBattery;
    #endregion
    
    public static PlayerMovement INSTANCE { get; private set; } //Singleton
    public static event Action OnPlayerChange; //Observer

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audio;
    private bool isGrounded;

    [Header("Config")] 
    public Transform raycastPos;
    public Transform raycastPos2;
    public LayerMask mask;
    public float distance;
    
    [Header("Movimiento")] 
    public float velocidad;
    public float velocidadSalto;

    private void Awake()
    {
        #region CashVariables
        sBoxCol = GetComponent<BoxCollider2D>();
        sCinemaChine = GameObject.Find("CineMachine").GetComponent<CinemachineVirtualCamera>();
        sBattery = GetComponentInChildren<Battery>();
        sCircleCol = GetComponent<CircleCollider2D>();
        #endregion

        audio = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        INSTANCE = this;
        OnPlayerChange?.Invoke();
        sCinemaChine.Follow = transform;
        anim.SetBool("Dead",false);
    }

    private void OnDisable()
    {
        rb.velocity = new Vector2(0,rb.velocity.y);
        anim.SetBool("Dead",true);
        anim.SetTrigger("Die");
        audio.enabled = false;
    }
    
    private void Update()
    {
        SpeedState();
        CheckSound();
        
        anim.SetFloat("Velocity",Mathf.Abs(rb.velocity.x));
        anim.SetBool("Grounded",isGrounded);

    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(raycastPos.position, Vector2.down, distance, mask) ||
                     Physics2D.Raycast(raycastPos2.position, Vector2.down, distance, mask);
        float h = Input.GetAxisRaw("Horizontal");

        HorizontalMovement(h);
        TryJump();
        CheckBox(h);
        TryWallJump();
    }
    
    private void TryJump()
    {
        if (Input.GetAxisRaw("Vertical") > 0 && isGrounded)
        {
            rb.AddForce(Vector2.up * velocidadSalto , ForceMode2D.Impulse);
        }
    }

    private void HorizontalMovement(float h)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(h * velocidad, rb.velocity.y);
        }else
        {
            rb.AddForce(Vector2.right * (h * velocidadSalto * 8f));
        }
    }

    private void TryWallJump()
    {
        if (!isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, 1f,mask) && transform.localScale.x < 0)
            {
                rb.velocity = Vector2.zero;
                Vector2 diagonal = new Vector2(-3.4f * velocidadSalto, 2.5f * velocidadSalto);
                rb.velocity = diagonal * 1.5f;
            }else if (Physics2D.Raycast(transform.position, Vector2.left, 1f,mask) && transform.localScale.x > 0)
            {
                rb.velocity = Vector2.zero;
                Vector2 diagonal = new Vector2(3.4f * velocidadSalto, 2.5f * velocidadSalto);
                rb.velocity = diagonal * 1.5f;
            }
        }
    }

    private void CheckBox(float h)
    {
        if (transform.localScale.x < 0 && Physics2D.Raycast(transform.position, Vector2.right, 0.8f,mask) && h > 0)
        {
            anim.SetBool("Pushing",true);
        }else if (transform.localScale.x > 0 && Physics2D.Raycast(transform.position, Vector2.left, 0.8f,mask) && h < 0)
        {
            anim.SetBool("Pushing",true);
        }else
        {
            anim.SetBool("Pushing",false);
        }
    }

    private void SpeedState()
    {
        #region LimitSpeed
        if (rb.velocity.y > 7f)
        {
            rb.velocity = new Vector2(rb.velocity.x , 7f);
        }
        
        if (rb.velocity.x > 6f)
        {
            rb.velocity = new Vector2(6f , rb.velocity.y);
        }else if (rb.velocity.x < -6f)
        {
            rb.velocity = new Vector2(-6f , rb.velocity.y);
        }
        #endregion

        #region ViewDirection

        if (rb.velocity.x > 0.15f && transform.localScale.x > 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }else if (rb.velocity.x < -0.15f && transform.localScale.x < 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        #endregion
    }

    private void CheckSound()
    {
        if (isGrounded && Mathf.Abs(rb.velocity.x) > 0.3f)
        {
            if(!audio.isPlaying)
                audio.Play();
           
            audio.loop = true;
        }
        else
        {
            audio.loop = false;
        }
    }

    public void EndGame()
    {
        anim.SetTrigger("Final");
        Destroy(this);
    }

    public void OnObjectSpawn()
    {
        audio.enabled = true;
        rb.isKinematic = false;
        sBoxCol.enabled = true;
        sCircleCol.enabled = true;
        sBattery.enabled = true;
        enabled = true;
    }
}
