using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator anim;
    
    public void IniciarJuego()
    {
        StartCoroutine(Inicio());
    }
    
    IEnumerator Inicio()
    {
        anim.SetTrigger("End");
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("Level1");
    }
}
