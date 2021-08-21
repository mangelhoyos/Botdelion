using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE { get; private set; } //Singleton
    
    void Awake()
    {
        INSTANCE = this;
    }
    

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 80;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        AudioManager.instance.Play("Theme");
        
        ObjectPooler.INSTANCE.SpawnFromPool("Player",Vector3.zero,quaternion.identity);
    }
}
