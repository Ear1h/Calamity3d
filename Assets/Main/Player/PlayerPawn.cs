using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPawn: Actor
{
    public override int DamageActor(int damage)
    {
        return base.DamageActor(damage);  
    }
}