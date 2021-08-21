using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private Animator anim;
    private bool isUsed = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            isUsed = true;
            PlayerMovement.INSTANCE.EndGame();
            StartCoroutine(Final());
        }
    }

    IEnumerator Final()
    {
        yield return new WaitForSeconds(1);
        anim.SetTrigger("End");
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
}
