using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public bool isOpen;


    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();
    

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    //public bool isFull;

    //PickupPopUp

    public GameObject pickupAlert;
    public TMP_Text pickupName;
    public Image pickupImage;


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


    void Start()
    {
        isOpen = false;

        PopulateSlotList();

        Cursor.visible = false;

    }

    void PopulateSlotList()
    {

        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {

                slotList.Add(child.gameObject);

            }
        }

    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;


        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (CraftingSystem.Instance.isOpen == false)
            {
               Cursor.lockState = CursorLockMode.Locked;
               Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
            
        }
    }

    public void AddToInventory(string itemName)
    {

        //SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);

        whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            itemList.Add(itemName);

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);


        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }


    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        Invoke("PopupDelay", 1.0f);

    }

    public void PopupDelay()
    {
        pickupAlert.SetActive(false);
    }


    public bool CheckSlotsAvailable(int emptyNeeded)
    {

        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {
                emptySlot += 1;
            }

        }

        if (emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private GameObject FindNextEmptySlot() 
    {

        foreach (GameObject slot in slotList)
        {

            if (slot.transform.childCount == 0)
            {

                return slot;

            }


        }

        return new GameObject();


    }


    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (int i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0 )
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1;

                }


            }
        }

        for (int i = EquipSystem.Instance.quickSlotsList.Count - 1; i >= 0; i--)
        {
            if (EquipSystem.Instance.quickSlotsList[i].transform.childCount > 0)
            {
                if (EquipSystem.Instance.quickSlotsList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(EquipSystem.Instance.quickSlotsList[i].transform.GetChild(0).gameObject);

                    counter -= 1;

                }

            }
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
        Debug.Log("InventorySystemRemoveItemCalisti");

    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList )
        {

            if (slot.transform.childCount > 0)
            {

                string name = slot.transform.GetChild(0).name; //Stone (Clone)

                string str1 = "(Clone)";

                string result = name.Replace(str1,"");


                itemList.Add(result);

            }

        }

        EquipSystem.Instance.quickitemList.Clear();

        foreach (GameObject quickslot in EquipSystem.Instance.quickSlotsList)
        {

            if (quickslot.transform.childCount > 0)
            {

                string name = quickslot.transform.GetChild(0).name; //Stone (Clone)

                string str1 = "(Clone)";

                string result = name.Replace(str1, "");


                EquipSystem.Instance.quickitemList.Add(result);
                Debug.Log("InventorySystem ekledi");

            }

        }

    }

}