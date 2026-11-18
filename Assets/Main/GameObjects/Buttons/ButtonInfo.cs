using System;
using UnityEngine;

[System.Serializable]
public class ButtonInfo
{
    [Flags]
    public enum KeyType
    {
        None = 0,
        RedKey = 1 << 0,
        BlueKey = 1 << 1,
        YellowKey = 1 << 2,
    }
}