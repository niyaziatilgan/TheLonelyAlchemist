using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, refineScreenUI, upgradeScreenUI, potionScreenUI, survivalScreenUI;

    public List<string> inventoryItemList = new List<string>();
    public List<string> quickSLOTitemList = new List<string>();

    //Category Buttons
    Button toolsButton, refineButton, upgradeButton, potionButton, survivalButton;

    //Craft Buttons
    Button craftAxeButton,craftPickaxeButton, craftStickButton, craftSwordButton, upgradeSwordButton, craftEnigmaButton, craftMagicButton, craftImmortalityButton, craftCampfireButton, craftChestButton;

    //Requirement Text
    TMP_Text AxeReq1, AxeReq2,PickaxeReq1,PickaxeReq2, StickReq1, SwordReq1, SwordReq2, GSwordReq1, GSwordReq2, EnigmaReq1, EnigmaReq2, MagicReq1, MagicReq2, ImmortalityReq1, ImmortalityReq2, CampfireReq1, CampfireReq2, ChestReq1, ChestReq2;

    public bool isOpen;

    //Craft Blueprints
    public Blueprint AxeBlueprint = new Blueprint("Axe", 1 , 2,"Stone", 3 ,"Stick", 3);
    public Blueprint SwordBlueprint = new Blueprint("Sword", 1, 2, "Stone", 3, "Stick", 3);
    public Blueprint PickaxeBlueprint = new Blueprint("Pickaxe", 1, 2, "Stone", 3, "Stick", 3);
    public Blueprint StickBlueprint = new Blueprint("Stick", 2 , 1, "Log", 1, "", 0);

    public Blueprint CampfireBlueprint = new Blueprint("Campfire", 1, 2, "Stone", 5, "Stick", 3);
    public Blueprint ChestBlueprint = new Blueprint("Chest", 1, 2, "Log", 4, "Iron", 2);

    //Potion Blueprints
    public Blueprint EnigmaBlueprint = new Blueprint("EnigmaElixir", 1, 2, "BasicPotion", 1, "GreenHerb", 1);
    public Blueprint MagicBlueprint = new Blueprint("MagicElixir", 1, 2, "EnigmaElixir", 1, "RedHerb", 1);
    public Blueprint ImmortalityBlueprint = new Blueprint("ImmortalityElixir", 1, 2, "MagicElixir", 1, "BlueHerb", 1);

    //Upgrade Blueprints
    public Blueprint SwordUpgradeBlueprint = new Blueprint("GreatSword", 1, 2, "Sword", 1, "Ruby", 1);



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

        upgradeButton = craftingScreenUI.transform.Find("UpgradeButton").GetComponent<Button>();
        upgradeButton.onClick.AddListener(delegate { OpenUpgradeCategory(); });

        potionButton = craftingScreenUI.transform.Find("PotionButton").GetComponent<Button>();
        potionButton.onClick.AddListener(delegate { OpenPotionCategory(); });


        survivalButton = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalButton.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        //Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TMP_Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TMP_Text>();

        craftAxeButton = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftAnyItem(AxeBlueprint); });

        PickaxeReq1 = toolsScreenUI.transform.Find("Pickaxe").transform.Find("req1").GetComponent<TMP_Text>();
        PickaxeReq2 = toolsScreenUI.transform.Find("Pickaxe").transform.Find("req2").GetComponent<TMP_Text>();

        craftPickaxeButton = toolsScreenUI.transform.Find("Pickaxe").transform.Find("Button").GetComponent<Button>();
        craftPickaxeButton.onClick.AddListener(delegate { CraftAnyItem(PickaxeBlueprint); });

        //Sword
        SwordReq1 = toolsScreenUI.transform.Find("Sword").transform.Find("req1").GetComponent<TMP_Text>();
        SwordReq2 = toolsScreenUI.transform.Find("Sword").transform.Find("req2").GetComponent<TMP_Text>();

        craftSwordButton = toolsScreenUI.transform.Find("Sword").transform.Find("Button").GetComponent<Button>();
        craftSwordButton.onClick.AddListener(delegate { CraftAnyItem(SwordBlueprint); });

        //Stick
        StickReq1 = refineScreenUI.transform.Find("Stick").transform.Find("req1").GetComponent<TMP_Text>();

        craftStickButton = refineScreenUI.transform.Find("Stick").transform.Find("Button").GetComponent<Button>();
        craftStickButton.onClick.AddListener(delegate { CraftAnyItem(StickBlueprint); });

        //Campfire
        CampfireReq1 = survivalScreenUI.transform.Find("Campfire").transform.Find("req1").GetComponent<TMP_Text>();
        CampfireReq2 = survivalScreenUI.transform.Find("Campfire").transform.Find("req2").GetComponent<TMP_Text>();

        craftCampfireButton = survivalScreenUI.transform.Find("Campfire").transform.Find("Button").GetComponent<Button>();
        craftCampfireButton.onClick.AddListener(delegate { CraftAnyItem(CampfireBlueprint); });

        //Chest
        ChestReq1 = survivalScreenUI.transform.Find("Chest").transform.Find("req1").GetComponent<TMP_Text>();
        ChestReq2 = survivalScreenUI.transform.Find("Chest").transform.Find("req2").GetComponent<TMP_Text>();

        craftChestButton = survivalScreenUI.transform.Find("Chest").transform.Find("Button").GetComponent<Button>();
        craftChestButton.onClick.AddListener(delegate { CraftAnyItem(ChestBlueprint); });



        //UPGRADE//
        GSwordReq1 = upgradeScreenUI.transform.Find("GreatSword").transform.Find("req1").GetComponent<TMP_Text>();
        GSwordReq2 = upgradeScreenUI.transform.Find("GreatSword").transform.Find("req2").GetComponent<TMP_Text>();

        upgradeSwordButton = upgradeScreenUI.transform.Find("GreatSword").transform.Find("Button").GetComponent<Button>();
        upgradeSwordButton.onClick.AddListener(delegate { UpgradeAnyItem(SwordUpgradeBlueprint); });
        upgradeSwordButton.onClick.AddListener(() => quickslotListCalculate("Sword_Model(Clone)"));


        //Enigma
        EnigmaReq1 = potionScreenUI.transform.Find("EnigmaElixir").transform.Find("req1").GetComponent<TMP_Text>();
        EnigmaReq2 = potionScreenUI.transform.Find("EnigmaElixir").transform.Find("req2").GetComponent<TMP_Text>();

        MagicReq1 = potionScreenUI.transform.Find("MagicElixir").transform.Find("req1").GetComponent<TMP_Text>();
        MagicReq2 = potionScreenUI.transform.Find("MagicElixir").transform.Find("req2").GetComponent<TMP_Text>();

        ImmortalityReq1 = potionScreenUI.transform.Find("ImmortalityElixir").transform.Find("req1").GetComponent<TMP_Text>();
        ImmortalityReq2 = potionScreenUI.transform.Find("ImmortalityElixir").transform.Find("req2").GetComponent<TMP_Text>();

        craftEnigmaButton = potionScreenUI.transform.Find("EnigmaElixir").transform.Find("Button").GetComponent<Button>();
        craftEnigmaButton.onClick.AddListener(delegate { CraftAnyItem(EnigmaBlueprint); });

        craftMagicButton = potionScreenUI.transform.Find("MagicElixir").transform.Find("Button").GetComponent<Button>();
        craftMagicButton.onClick.AddListener(delegate { CraftAnyItem(MagicBlueprint); });

        craftImmortalityButton = potionScreenUI.transform.Find("ImmortalityElixir").transform.Find("Button").GetComponent<Button>();
        craftImmortalityButton.onClick.AddListener(delegate { CraftAnyItem(ImmortalityBlueprint); });




    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        upgradeScreenUI.SetActive(false);
        potionScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        upgradeScreenUI.SetActive(false);
        potionScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);
    }

    void OpenUpgradeCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        potionScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        upgradeScreenUI.SetActive(true);
    }

    void OpenPotionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        upgradeScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        potionScreenUI.SetActive(true);
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        upgradeScreenUI.SetActive(false);
        potionScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        StartCoroutine(craftedDelayForSound(blueprintToCraft));


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

    void UpgradeAnyItem(Blueprint blueprintToCraft)
    {

        SoundManager.Instance.PlayOneShotMusic(SoundManager.Instance.upgradeSound);

        StartCoroutine(craftedDelayForSound(blueprintToCraft));


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

    void quickslotListCalculate(string ItemBeingUpgrading)
    {
        GameObject toolHolder = EquipSystem.Instance.toolHolder;
        GameObject childObject = toolHolder.transform.GetChild(0).gameObject;
        string childName = childObject.name;

        Debug.Log(childName);
        

        if (toolHolder != null)
        {
            
            if (toolHolder.transform.childCount > 0 && childName == ItemBeingUpgrading)
            {
                Transform child = toolHolder.transform.GetChild(0);
                DestroyImmediate(child.gameObject);

                EquipSystem.Instance.selectedItemModel = null;
                EquipSystem.Instance.selectedItem = null;
                EquipSystem.Instance.selectedNumber = -1;
                EquipSystem.Instance.quickSlotListReset();

            }
            else
            {

            }
        }

    }

    public IEnumerator calculate()
    {
        
        yield return 0;

        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
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
            upgradeScreenUI.SetActive(false);
            potionScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);

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
        int ruby_count = 0;
        int sword_count = 0;
        int greenherb_count = 0;
        int redherb_count = 0;
        int blueherb_count = 0;
        int enigma_count = 0;
        int magic_count = 0;
        int immortality_count = 0;
        int basic_count = 0;
        int iron_count = 0;
        inventoryItemList = InventorySystem.Instance.itemList;
        quickSLOTitemList = EquipSystem.Instance.quickitemList;

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
                case "Ruby":
                    ruby_count += 1;
                    break;
                case "Sword":
                    sword_count += 1;
                    break;
                case "GreenHerb":
                    greenherb_count += 1;
                    break;
                case "RedHerb":
                    redherb_count += 1;
                    break;
                case "BlueHerb":
                    blueherb_count += 1;
                    break;
                case "EnigmaElixir":
                    enigma_count += 1;
                    break;
                case "MagicElixir":
                    magic_count += 1;
                    break;
                case "ImmortalityElixir":
                    immortality_count += 1;
                    break;
                case "BasicPotion":
                    basic_count += 1;
                    break;
                case "Iron":
                    iron_count += 1;
                    break;
            }
        }

        foreach (string itemName in quickSLOTitemList)
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
                case "Ruby":
                    ruby_count += 1;
                    break;
                case "Sword":
                    sword_count += 1;
                    break;
                case "GreenHerb":
                    greenherb_count += 1;
                    break;
                case "RedHerb":
                    redherb_count += 1;
                    break;
                case "BlueHerb":
                    blueherb_count += 1;
                    break;
                case "EnigmaElixir":
                    enigma_count += 1;
                    break;
                case "MagicElixir":
                    magic_count += 1;
                    break;
                case "ImmortalityElixir":
                    immortality_count += 1;
                    break;
                case "BasicPotion":
                    basic_count += 1;
                    break;
                case "Iron":
                    iron_count += 1;
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

        //-----Pickaxe----////
        PickaxeReq1.text = "3 Stone [" + stone_count + "]";
        PickaxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftPickaxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftPickaxeButton.gameObject.SetActive(false);
        }
        //-------SWORDDDDD-----//

        SwordReq1.text = "3 Stone [" + stone_count + "]";
        SwordReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftSwordButton.gameObject.SetActive(true);
        }
        else
        {
            craftSwordButton.gameObject.SetActive(false);
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



        //UPGRADE//

        //-------GREAT SWORD-----//

        GSwordReq1.text = "1 Sword [" + sword_count + "]";
        GSwordReq2.text = "1 Ruby [" + ruby_count + "]";

        if (sword_count >= 1 && ruby_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            upgradeSwordButton.gameObject.SetActive(true);
        }
        else
        {
            upgradeSwordButton.gameObject.SetActive(false);
        }

        //POTION

        EnigmaReq1.text = "1 Basic Potion [" + basic_count + "]";
        EnigmaReq2.text = "1 Green Herb [" + greenherb_count + "]";

        if (basic_count >= 1 && greenherb_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftEnigmaButton.gameObject.SetActive(true);
        }
        else
        {
            craftEnigmaButton.gameObject.SetActive(false);
        }

        MagicReq1.text = "1 Enigma Elixir [" + enigma_count + "]";
        MagicReq2.text = "1 Red Herb [" + redherb_count + "]";

        if (enigma_count >= 1 && redherb_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftMagicButton.gameObject.SetActive(true);
        }
        else
        {
            craftMagicButton.gameObject.SetActive(false);
        }

        ImmortalityReq1.text = "1 Magic Elixir [" + magic_count + "]";
        ImmortalityReq2.text = "1 Blue Herb [" + blueherb_count + "]";

        if (magic_count >= 1 && blueherb_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftImmortalityButton.gameObject.SetActive(true);
        }
        else
        {
            craftImmortalityButton.gameObject.SetActive(false);
        }


        CampfireReq1.text = "5 Stone [" + stone_count + "]";
        CampfireReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 5 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftCampfireButton.gameObject.SetActive(true);
        }
        else
        {
            craftCampfireButton.gameObject.SetActive(false);
        }


        ChestReq1.text = "4 Log [" + log_count + "]";
        ChestReq2.text = "2 Iron [" + iron_count + "]";

        if (log_count >= 4 && iron_count >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftChestButton.gameObject.SetActive(true);
        }
        else
        {
            craftChestButton.gameObject.SetActive(false);
        }

    }
}
