using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, refineScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsButton, refineButton;

    //Craft Buttons
    Button craftAxeButton, craftStickButton;

    //Requirement Text
    TMP_Text AxeReq1, AxeReq2, StickReq1;

    public bool isOpen;

    //All Blueprints
    public Blueprint AxeBlueprint = new Blueprint("Axe", 1 , 2,"Stone", 3 ,"Stick", 3);
    public Blueprint StickBlueprint = new Blueprint("Stick", 2 , 1, "Log", 1, "", 0);


    public static CraftingSystem Instance { get; set; }


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

        toolsButton = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate { OpenToolsCategory(); });

        refineButton = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineButton.onClick.AddListener(delegate { OpenRefineCategory(); });

        //Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TMP_Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TMP_Text>();

        craftAxeButton = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(AxeBlueprint); });

        //Stick
        StickReq1 = refineScreenUI.transform.Find("Stick").transform.Find("req1").GetComponent<TMP_Text>();

        craftStickButton = refineScreenUI.transform.Find("Stick").transform.Find("Button").GetComponent<Button>();
        craftStickButton.onClick.AddListener(delegate { CraftAnyItem(StickBlueprint); });




    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        for (int i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }


        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);

            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        StartCoroutine(calculate());
    }

    public IEnumerator calculate()
    {
        
        yield return 0;

        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;


            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);

            if (InventorySystem.Instance.isOpen == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            }

            isOpen = false;
        }
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;

                case "Stick":
                    stick_count += 1;
                    break;
                case "Log":
                    log_count += 1;
                    break;
            }
        }

        //-------AXEEEEE-----//

        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

        //-------Stick x 2-----//

        StickReq1.text = "1 Log [" + log_count + "]";

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftStickButton.gameObject.SetActive(true);
        }
        else
        {
            craftStickButton.gameObject.SetActive(false);
        }


    }
}
