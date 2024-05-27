using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
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
        DontDestroyOnLoad(gameObject);
    }

    string binaryPath;

    string jsonPathPersistant;

    string jsonPathProject;

    public bool isSavingtoJson;

    string fileName = "SaveGame";

    public bool isLoading;

    public Canvas loadingScreen;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar ;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region || ---------- General Section --------- ||


    #region || ---------- Saving --------- ||
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        data.enviromentData = GetEnviromentData();

        SavingTypeSwitch(data, slotNumber);
    }

    private EnviromentData GetEnviromentData()
    {
        List<string> itemsPickedup = InventorySystem.Instance.itemsPickedup;
        List<string> itemsDropped = InventorySystem.Instance.droppedItemsInventoryList;

        List<StorageData> allStorage = new List<StorageData>();
        List<CampfireData> allCampfire = new List<CampfireData>();

        foreach (Transform placeable in EnviromentManager.Instance.allPlaceables.transform)
        {
            if (placeable.gameObject.GetComponent<StorageBox>())
            {
                var sd = new StorageData();
                sd.items = placeable.gameObject.GetComponent<StorageBox>().items;
                sd.position = placeable.position;
                sd.rotation = new Vector3(placeable.rotation.x, placeable.rotation.y, placeable.rotation.z);

                allStorage.Add(sd);
            }

            if (placeable.gameObject.GetComponent<CampfireNew>())
            {
                var cd = new CampfireData();
                cd.position = placeable.position;
                cd.rotation = new Vector3(placeable.rotation.x, placeable.rotation.y, placeable.rotation.z);

                allCampfire.Add(cd);
            }
        }

        List<TreeData> treeToSave = new List<TreeData>();
        foreach (Transform tree in EnviromentManager.Instance.allTrees.transform)
        {
            if (tree.CompareTag("Tree"))
            {
                var td = new TreeData();
                td.name = "Tree_Parent";
                td.position = tree.position;
                td.rotation = new Vector3(tree.localRotation.x, tree.localRotation.y, tree.localRotation.z);

                treeToSave.Add(td);
            }
            else
            {
                var td = new TreeData();
                td.name = "Stump";
                td.position = tree.position;
                td.rotation = new Vector3(tree.position.x, tree.position.y, tree.position.z);

                treeToSave.Add(td);
            }
        }

        List<DroppedData> droppedToSave = new List<DroppedData>();

        foreach (Transform droppedItems in EnviromentManager.Instance.droppedItems.transform)
        {
            if (droppedItems.gameObject.GetComponent<InteractableObject>())
            {
                var dI = new DroppedData();
                dI.items = InventorySystem.Instance.droppedItemsInventoryList;
                dI.name = droppedItems.name;


                dI.position = droppedItems.position;
                dI.rotation = new Vector3(droppedItems.rotation.x, droppedItems.rotation.y, droppedItems.rotation.z);

                droppedToSave.Add(dI);
            }
        }


        return new EnviromentData(itemsPickedup,itemsDropped, allStorage, treeToSave, droppedToSave, allCampfire);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydrationPercent;


        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        string[] inventory = InventorySystem.Instance.itemList.ToArray();

        string[] quickSlots = GetQuickSlotsContent();

        return new PlayerData(playerStats, playerPosAndRot,inventory, quickSlots);
    }

    private string[] GetQuickSlotsContent()
    {
        List<string> temp = new List<string>();

        foreach(GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if(slot.transform.childCount != 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }
        return temp.ToArray();
    }

    public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if (isSavingtoJson)
        {
            SaveGameDataToJsonFile(gameData, slotNumber);
        }

        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }
    #endregion


    #region || ---------- Loading --------- ||

    public AllGameData loadingTypeSwitch(int slotNumber)
    {
        if (isSavingtoJson)
        {
            AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);
            return gameData;

        }
        else
        {
            AllGameData gameData =LoadGameDataFromBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        //player data
        SetPlayerData(loadingTypeSwitch(slotNumber).playerData);

        //enviroment data
        SetEnviromentData(loadingTypeSwitch(slotNumber).enviromentData);

        isLoading = false;

        DisableLoadingScreen();
    }

    private void SetEnviromentData(EnviromentData enviromentData)
    {
        foreach(Transform itemType in EnviromentManager.Instance.allItems.transform)
        {
            foreach(Transform item in itemType.transform)
            {
                if (enviromentData.pickedupItems.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.itemsPickedup = enviromentData.pickedupItems;

        foreach (StorageData storage in enviromentData.storage)
        {
            var storageBoxPrefab = Instantiate(Resources.Load<GameObject>("Chest2Model"),
                new Vector3(storage.position.x, storage.position.y, storage.position.z),
                Quaternion.Euler(storage.rotation.x, storage.rotation.y, storage.rotation.z));

            storageBoxPrefab.GetComponent<StorageBox>().items = storage.items;
            storageBoxPrefab.transform.SetParent(EnviromentManager.Instance.allPlaceables.transform);
        }

        foreach (CampfireData campfire in enviromentData.campfire)
        {
            var campfirePrefab = Instantiate(Resources.Load<GameObject>("CampfireModel"),
                new Vector3(campfire.position.x, campfire.position.y, campfire.position.z),
                Quaternion.Euler(campfire.rotation.x, campfire.rotation.y, campfire.rotation.z));


            campfirePrefab.transform.SetParent(EnviromentManager.Instance.allPlaceables.transform);
        }

        foreach (Transform tree in EnviromentManager.Instance.allTrees.transform)
        {
            Destroy(tree.gameObject);
        }

        foreach (TreeData tree in enviromentData.treeData)
        {
            var treePrefab = Instantiate(Resources.Load<GameObject>(tree.name),
                new Vector3(tree.position.x, tree.position.y, tree.position.z),
                Quaternion.Euler(tree.rotation.x, tree.rotation.y, tree.rotation.z));

            treePrefab.transform.SetParent(EnviromentManager.Instance.allTrees.transform);
        }


        foreach (DroppedData dropped in enviromentData.droppeditemdata)
        {
            string cleanName = dropped.name.Split(new string[] { "(Clone)" }, StringSplitOptions.None)[0];
            var droppedPrefab = Instantiate(Resources.Load<GameObject>(cleanName),
                new Vector3(dropped.position.x, dropped.position.y, dropped.position.z),
                Quaternion.Euler(dropped.rotation.x, dropped.rotation.y, dropped.rotation.z));

            InventorySystem.Instance.droppedItemsInventoryList = dropped.items;
            droppedPrefab.transform.SetParent(EnviromentManager.Instance.droppedItems.transform);
        }


    }

    private void SetPlayerData(PlayerData playerData)
    {
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];

        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

        // envanter sisteminin ayarlanmasý

        foreach (string item in playerData.inventoryContent)
        {
            InventorySystem.Instance.AddToInventory(item);
        }

        foreach (string item in playerData.quickSlotContent)
        {
            GameObject avaibleSlot = EquipSystem.Instance.FindNextEmptySlot();

            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));

            itemToAdd.transform.SetParent(avaibleSlot.transform, false);
        }

    }


    public void StartLoadedGame(int slotNumber)
    {
        ActivateLoadingScreen();

        isLoading = true;

        SceneManager.LoadScene("GameScene");

        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1f);

        LoadGame(slotNumber);
    }


    #endregion



    #endregion

    #region || ---------- To Binary Section --------- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
     BinaryFormatter formatter = new BinaryFormatter();

        
        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to" + binaryPath + fileName + slotNumber + ".bin");
    }

    public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
    {
        

        if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode .Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Data loaded from" + binaryPath + fileName + slotNumber + ".bin");

            return data;
        }

        else
        {
            return null;
        }
    }


    #endregion

    #region || ---------- To Json Section --------- ||

    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);

        //string encrypted = EncryptionDecryption(json);

        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.Write(json);
            print("Saved Game to Json file at :" + jsonPathProject + fileName + slotNumber + ".json");
        };
    }

    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
       using(StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();

            //string decrypted = EncryptionDecryption(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            return data;
        }
    }


    #endregion

    #region || ---------- Settings Section --------- ||

    #region || ---------- Volume Settings  --------- ||

    [System.Serializable]

    public class VolumeSettings
    {
        public float music;
        public float effects;
        public float master;
    }

    public void SaveVolumeSettings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            music = _music,
            effects = _effects,
            master = _master

        };
        
        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();

        print("Saved to Player Pref");
    }

    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        
    }
    public float LoadMusicVolume()
    {
        var volumeSettings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        return volumeSettings.music;
    }

    #endregion
    #endregion

    #region || ---------- Encryption --------- ||

    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "1234567";

        string result = "";

        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }
        return result;
    }






    #endregion

    #region || ---------- Loading Section --------- ||

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);
        
        //normalde unity.engine kullanmýyorum hata veriyor diye kullandým.

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        //animation

        //show

    }

    public void DisableLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    public void startGameCo()
    {
        StartCoroutine(startGame());
    }

    public IEnumerator startGame()
    {
        yield return new WaitForSeconds(1f);
        SaveManager.Instance.isLoading = false;
        SaveManager.Instance.DisableLoadingScreen();
    }



    #endregion
    #region || ---------- Utility --------- ||

    public bool DoesFileExists(int slotNumber)
    {
        if(isSavingtoJson)
        {
            if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))   //SaveGame1.json 
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
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExists(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }



    #endregion
}
