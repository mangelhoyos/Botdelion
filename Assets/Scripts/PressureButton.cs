using System;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public enum Eventos
    {
        Puerta,
        Cañon
    }

    private Animator anim;
    
    public Eventos evento;
    public Transform objetoEvento;
    public bool stayPressed = true;

    private Material buttonShader;

    private void Awake()
    {
        if (evento == Eventos.Puerta && stayPressed)
        {
            anim = objetoEvento.GetComponent<Animator>();
        }

        buttonShader = GetComponent<SpriteRenderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Box")) && !stayPressed)
        {
            switch (evento)
            {
                case Eventos.Puerta:
                    objetoEvento.GetComponent<Animator>().SetTrigger("Open");
                    buttonShader.SetInt("IsPressed",1);
                    Destroy(this);
                    break;
                
                case Eventos.Cañon:
                    Canon.INSTANCE.Shoot();
                    buttonShader.SetInt("IsPressed",1);
                    Destroy(this);
                    break;    
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Box")) && stayPressed)
        {
            switch (evento)
            {
                case Eventos.Puerta:
                    anim.SetBool("Open", true);
                    buttonShader.SetInt("IsPressed",1);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Box")) && stayPressed)
        {
            switch (evento)
            {
                case Eventos.Puerta:
                    anim.SetBool("Open", false);
                    buttonShader.SetInt("IsPressed",0);
                    break;
            }
        }
    }
}
