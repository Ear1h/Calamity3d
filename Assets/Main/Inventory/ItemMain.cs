using UnityEngine;
using System.Collections;
using TMPro;
using System;
using System.Collections.Generic;

public class MainInventory : Actor
{
    [Flags]
    public enum ItemFlagsPublic
    {
        None = 0,
        Rotating = 1 << 0,
        FloatBoobing = 1 << 1,
    }

    [Flags]
    public enum ItemFlagsInternal
    {
        None = 0,
        AlwaysPickup = 1 << 0,
    }



    [SerializeField] protected PickupMessageUI PickupMessageUI;
    protected string PickupMessage;
    
    protected float  PickupMessageDelay;
    private Vector3 startposition;
    private float bobStartTime;
    private float bobAmount;

    protected int Amount;
    protected int MaxAmount;
    public (int Amount, int MaxAmount) AmountCounter
    {
        get => (Amount, MaxAmount);
        set => (Amount, MaxAmount) = value;
    }

    [SerializeField] protected ItemFlagsPublic flags;
    protected ItemFlagsInternal intFlags;

    

    public virtual string StringMessage()
    {
        if (PickupMessageUI != null)
        {
            PickupMessageUI.ShowMessage(
                PickupMessage,
                PickupMessageDelay
            );
        }

        return PickupMessage;
    }

    protected virtual void RotateItem()
    {
        this.transform.Rotate(xAngle: 0, yAngle: -30f * Time.deltaTime, zAngle: 0, relativeTo: Space.Self);
    }

    private void Start()
    {
        startposition = transform.position;
        bobStartTime = Time.time;

        Renderer renderer = GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            bobAmount = renderer.bounds.size.y * 0.25f;
        }
    }
    protected virtual void FloatBob()
    {
        float floatbobY = Mathf.Sin((Time.time - bobStartTime) * 2f) * bobAmount;
        this.transform.position = startposition + Vector3.up * floatbobY;
    }

    public virtual bool TryPickup(Actor toucher, bool pickup = false)
    {
        return false;
    }

    public virtual int AbsorbDamage(int damage, ref int newdamage) { return damage; }

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (TryPickup(actor))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (flags.HasFlag(ItemFlagsPublic.Rotating))
            RotateItem();

        if (flags.HasFlag(ItemFlagsPublic.FloatBoobing))
            FloatBob();
    }
}