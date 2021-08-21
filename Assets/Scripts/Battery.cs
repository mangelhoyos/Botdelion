using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using ChromaticAberration = UnityEngine.Rendering.Universal.ChromaticAberration;

public class Battery : MonoBehaviour
{
    #region CashVariables

    private PlayerMovement sPlayerMovement;
    private Rigidbody2D sRb;
    private BoxCollider2D sBoxCol;
    private CircleCollider2D sCircleCol;

    #endregion

    public delegate void Delegate();
    private Delegate ChargeDelegate;
     public static Battery INSTANCE { get; private set; } //Singleton

    [HideInInspector]public float battery;
    private float MAXBATTERY = 100f;
    private bool isDead;
    private Light2D _light;
    private ParticleSystem ps;

    public VolumeProfile profile;
    private ChromaticAberration chrom;

    private float deltaTime;

    void Awake()
    {
        #region CashVariables
        sPlayerMovement = transform.root.GetComponent<PlayerMovement>();
        sRb = transform.root.GetComponent<Rigidbody2D>();
        sBoxCol = transform.root.GetComponent<BoxCollider2D>();
        sCircleCol = transform.root.GetComponent<CircleCollider2D>();
        #endregion

        _light = GetComponent<Light2D>();
        profile.TryGet(out chrom);
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        ps.Stop();
        battery = 100;
        INSTANCE = this;
        ChargeDelegate = UnCharge;
        battery = MAXBATTERY;
        isDead = false;
    }

    void Update()
    {
        ChargeDelegate();
        UpdateBattery();
    }
    
    private void UpdateBattery()
    {
        battery = Mathf.Clamp(battery, 0f, 100f);

        _light.intensity = battery / 100f;

        if (battery <= 0 && !isDead)
        {
            ps.Stop();
            isDead = true;
            StartCoroutine(DeactivateRobot());
        }
    }
    
    private IEnumerator DeactivateRobot()
    {
        sPlayerMovement.enabled = false;
        yield return new WaitForSeconds(2f);
        sRb.isKinematic = true;
        sBoxCol.enabled = false;
        sCircleCol.enabled = false;
        ObjectPooler.INSTANCE.SpawnFromPool("Player",Vector3.zero, quaternion.identity);
        enabled = false;
    }

    public void isCharging(bool state)
    {
        if (state)
        {
            ChargeDelegate = Charge;
        }else
        {
            ChargeDelegate = UnCharge;
        }
        
    }
    
    public void Charge()
    {
        battery += Time.deltaTime * 9f;
        chrom.intensity.value = Mathf.Lerp(chrom.intensity.value,1,3 * Time.deltaTime);
        if(!ps.isPlaying)
            ps.Play();
    }

    public void UnCharge()
    {
        ps.Stop();
        battery -= Time.deltaTime * 2.2f;
        chrom.intensity.value = Mathf.Lerp(chrom.intensity.value,0,3 * Time.deltaTime);
    }
}
