using UnityEngine;

public class RechargeArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Battery.INSTANCE.isCharging(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Battery.INSTANCE.isCharging(false);
        }
    }
}
