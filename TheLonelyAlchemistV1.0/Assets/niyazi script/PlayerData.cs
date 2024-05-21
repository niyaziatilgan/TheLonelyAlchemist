using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float[] playerStats;  // [0] - Health, [1] - Calories, [2] - Hydration

    public float[] playerPositionAndRotation;   // pozition x,y,z and rotation x,y,z

    public string[] inventoryContent;

    public string[] quickSlotContent;

    // public float[] inventoryContent;

    public PlayerData(float[] _playerStats, float[] _playerPosAndRot, string[] _inventoryContent, string[] _quickSlotContent) 
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPosAndRot;
        inventoryContent = _inventoryContent;
        quickSlotContent = _quickSlotContent;
    }
}
