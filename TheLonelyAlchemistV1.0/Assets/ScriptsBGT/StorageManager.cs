using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; set; }

    [SerializeField] GameObject storageBoxSmallUI;
    [SerializeField] StorageBox selectedStorage;
    public bool storageUIOpen;

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

    public void OpenBox(StorageBox storage)
    {
        SetSelectedStorage(storage);

        PopulateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(true);
        storageUIOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        SoundManager.Instance.PlayOneShotMusic(SoundManager.Instance.openBox);
    }

    private void PopulateStorage(GameObject storageUI)
    {

        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        foreach (string name in selectedStorage.items)
        {
   
            foreach (GameObject slot in uiSlots)
            {
                if (slot.transform.childCount < 1)
                {

                    var itemToAdd = Instantiate(Resources.Load<GameObject>(name), slot.transform.position, slot.transform.rotation);

                    itemToAdd.name = name;

                    itemToAdd.transform.SetParent(slot.transform);
                    break;
                }
            }
        }
    }

    public void CloseBox()
    {

        RecalculateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(false);
        storageUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        SoundManager.Instance.PlayOneShotMusic(SoundManager.Instance.closeBox);
    }

    private void RecalculateStorage(GameObject gameObject)
    {
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageBoxSmallUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        selectedStorage.items.Clear();

        List<GameObject> toBeDeleted = new List<GameObject>();

        foreach (GameObject slot in uiSlots)
        {
            if (slot.transform.childCount > 0 )
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                selectedStorage.items.Add(result);
                toBeDeleted.Add(slot.transform.GetChild(0).gameObject);
                Debug.Log(result);
            }
        }

        foreach (GameObject obj in toBeDeleted)
        {
            Destroy(obj);
        }

    }

    public void SetSelectedStorage(StorageBox storage)
    {
        selectedStorage = storage;
    }

    private GameObject GetRelevantUI(StorageBox storage)
    {
        return storageBoxSmallUI;
    }
}