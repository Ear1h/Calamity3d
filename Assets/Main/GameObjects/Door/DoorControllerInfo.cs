using System;
using UnityEngine;

[System.Serializable]
public class DoorControllerInfo
{
    [Flags]
    public enum DoorFlags
    {
        None = 0,
        StayOpen = 1 << 0,
        Smooth = 1 << 1,
    }

    [Flags]
    public enum KeyType
    {
        None = 0,
        RedKey = 1 << 0,
        BlueKey = 1 << 1,
        YellowKey = 1 << 2,
    }

    [field: SerializeField] public KeyType Keys { get; set; }
    [field: SerializeField] public DoorFlags Flags { get; set; }
}