using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    //[SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);

        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject tempItemReference = itemBeingDragged;

        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent == transform.root)
        {
            tempItemReference.SetActive(false);

            AlertDialogManager dialogManager = FindAnyObjectByType<AlertDialogManager>();

            dialogManager.ShowDilaog("Do you want to drop this item?", (response) =>
            {
                if (response)
                {
                    DropItemIntoTheWorld(tempItemReference);

                }
                else
                {
                    transform.position = startPosition;
                    transform.SetParent(startParent);

                    tempItemReference.SetActive(true);
                }
            });

            transform.position = startPosition;
            transform.SetParent(startParent);

        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void DropItemIntoTheWorld(GameObject tempItemReference)
    {
        string cleanName = tempItemReference.name.Split(new string[] { "(Clone)" }, StringSplitOptions.None)[0];

        InventorySystem.Instance.droppedItemsInventoryList.Add(cleanName);
        
        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        item.transform.position = Vector3.zero;
        Vector3 dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
        item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

        Transform itemsObject = FindAnyObjectByType<EnviromentManager>().gameObject.transform.Find("[DroppedItems]");
        item.transform.SetParent(itemsObject.transform);

        DestroyImmediate(tempItemReference.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
}
