using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance { get; set; }

    public GameObject placementHoldingSpot;
    public GameObject enviromentPlaceables;


    public bool inPlacementMode;
    [SerializeField] bool isValidPlacement;

    [SerializeField] GameObject itemToBePlaced;
    public GameObject inventoryItemToDestory;

    [SerializeField] GameObject placementModeUI;

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

    public void ActivatePlacementMode(string itemToPlace)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToPlace));

        item.name = itemToPlace;

        item.transform.SetParent(placementHoldingSpot.transform, false);

        itemToBePlaced = item;

        inPlacementMode = true;
    }

    private void Update()
    {

        if (inPlacementMode)
        {
            placementModeUI.SetActive(true);
        }
        else
        {
            placementModeUI.SetActive(false);
        }

        if (itemToBePlaced != null && inPlacementMode)
        {
            if (IsCheckValidPlacement())
            {
                isValidPlacement = true;
                itemToBePlaced.GetComponent<PlacebleItem>().SetValidColor();
            }
            else
            {
                isValidPlacement = false;
                itemToBePlaced.GetComponent<PlacebleItem>().SetInvalidColor();
            }
        }

        if (Input.GetMouseButtonDown(0) && inPlacementMode && isValidPlacement)
        {
            PlaceItemFreeStyle();
            DestroyItem(inventoryItemToDestory);
            SoundManager.Instance.PlayOneShotMusic(SoundManager.Instance.placementSound);
        }

        // Cancel Placement
        if (Input.GetKeyDown(KeyCode.X))
        {
            inventoryItemToDestory.SetActive(true);
            inventoryItemToDestory = null;
            DestroyItem(itemToBePlaced);
            itemToBePlaced = null;
            inPlacementMode = false;
        }
    }

    private bool IsCheckValidPlacement()
    {
        if (itemToBePlaced != null)
        {
            return itemToBePlaced.GetComponent<PlacebleItem>().isValidToBeBuilt;
        }

        return false;
    }

    private void PlaceItemFreeStyle()
    {
        itemToBePlaced.transform.SetParent(enviromentPlaceables.transform, true);

        itemToBePlaced.GetComponent<PlacebleItem>().SetDefaultColor();
        itemToBePlaced.GetComponent<PlacebleItem>().enabled = false;

        itemToBePlaced = null;

        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        inPlacementMode = false;
    }

    private void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
}
