using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public int dayInGame = 1;

    public TextMeshProUGUI dayUI;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        dayUI.text = $"Day: {dayInGame}";

    }
    public void TriggerNextDay()
    {
        dayInGame += 1;
        dayUI.text = $"Day: {dayInGame}";
    }
}
