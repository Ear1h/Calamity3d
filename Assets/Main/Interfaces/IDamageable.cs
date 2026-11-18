using UnityEngine;

public interface IDamageable
{
    public int DamageActor(int damage, Actor source, GameObject inflictor);
}
