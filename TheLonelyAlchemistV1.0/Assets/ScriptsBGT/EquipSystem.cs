using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> quickitemList = new List<string>();


    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;

    public GameObject selectedItemModel;



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
        PopulateSlotList();
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
           
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
            
        }

    }

    void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;


                //Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);

                //Changing the color
    
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<TMP_Text>().color = Color.gray;
                }

                TMP_Text toBeChanged = numbersHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<TMP_Text>();
                toBeChanged.color = Color.white;
            }
            else //We are trying to select the same slot
            {
                selectedNumber = -1; //null anlamina geliyor there is no number selected


                //Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                //Changing the color
                
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<TMP_Text>().color = Color.gray;
                }
            }
        }
    }

    public void quickSlotListReset()
    {
        
        foreach (Transform child in numbersHolder.transform)
        {
            child.transform.Find("Text").GetComponent<TMP_Text>().color = Color.gray;
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }


        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        SoundManager.Instance.PlaySound(SoundManager.Instance.quickSlotSound);
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(0.524f, 0.208f, 0.625f), Quaternion.Euler(0.974f, -88.887f, 4.136f));
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    bool checkIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.ReCalculateList();

    }


    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        }
        else
        {
            return 0;
        }
    }
    internal bool IsHoldingWeapon()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetComponent<Weapon>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal bool IsThereASwingLock()
    {
        if (selectedItemModel && selectedItemModel.GetComponent<EquipableItem>())
        {
            return selectedItemModel.GetComponent<EquipableItem>().swingWait;
        }
        else
        {
            return false;
        }
    }



}