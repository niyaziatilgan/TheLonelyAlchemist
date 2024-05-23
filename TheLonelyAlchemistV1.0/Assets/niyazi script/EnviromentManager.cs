using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentManager : MonoBehaviour
{

    public static EnviromentManager Instance { get; set; }

    public GameObject allItems;
    public GameObject allPlaceables;
    public GameObject allTrees;
    public GameObject droppedItems;
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
}
