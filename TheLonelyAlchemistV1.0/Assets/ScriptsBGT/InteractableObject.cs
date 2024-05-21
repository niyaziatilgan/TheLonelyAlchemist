using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool playerRange;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            //if the Inventory is not Full
            if (InventorySystem.Instance.CheckSlotsAvailable(1))
            {
                InventorySystem.Instance.AddToInventory(ItemName);

                InventorySystem.Instance.itemsPickedup.Add(gameObject.name);




                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventory is full");
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
        }
    }

}


