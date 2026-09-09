using UnityEngine;
using System.Collections;

public class TriggerHurt : MonoBehaviour
{
    [SerializeField]
    private int DamageCount = 20;

    [SerializeField]
    public float DamageRate = 3.0f;


    private float nextDamage = 0.0f;
    public void OnTriggerStay(Collider other)
    {
        PlayerPawn player = other.GetComponent<PlayerPawn>();
        if (player != null && Time.time > nextDamage)
        {
            nextDamage = Time.time + DamageRate;
            player.TakeDamage(DamageCount);
        }
    }
}