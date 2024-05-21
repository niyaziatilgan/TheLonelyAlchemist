using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }


    public bool onTarget;

    public GameObject selectedObject;


    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;

    public Image handIcon;
    public Image centerDotImage;

    public bool handIsVisible;

    //tree
    public GameObject selectedTree;
    public GameObject chopHolder;

    public float caloriesSpentAttacking;

    public GameObject selectedStorageBox;


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
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            Animal animal = selectionTransform.GetComponent<Animal>();

            if (animal && animal.playerInRange)
            {

                if (animal.isDead)
                {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_text.text = animal.animalName;
                    interaction_Info_UI.SetActive(true);

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && EquipSystem.Instance.IsThereASwingLock() == false)
                    {
                        StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                        PlayerState.Instance.currentCalories -= caloriesSpentAttacking;


                    }
                }

            }

            EnemyAI enemy = selectionTransform.GetComponent<EnemyAI>();

            if (enemy && enemy.playerInAttackRange && enemy.playerInSightRange)
            {

                interaction_text.text = enemy.bossName;
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && enemy.playerCanAttackRange/*&& EquipSystem.Instance.isThereASwingLock() == false*/)
                {
                    StartCoroutine(DealDamageToEnemy(enemy, 0.3f, EquipSystem.Instance.GetWeaponDamage()));

                }

            }

            if (enemy && enemy.isDead)
            {
                interaction_text.text = "Loot";
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;

                if (Input.GetMouseButtonDown(0))
                {
                    Lootable lootable = enemy.GetComponent<Lootable>();
                    Loot(lootable);
                }

            }

            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();

            if (storageBox && storageBox.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Open";
                interaction_Info_UI.SetActive(true);

                selectedStorageBox = storageBox.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    StorageManager.Instance.OpenBox(storageBox);
                }
            }
            else
            {
                if (selectedStorageBox != null)
                {
                    selectedStorageBox = null;
                }
            }

            InteractableObject ourInteractable = selectionTransform.GetComponent<InteractableObject>();

            if (ourInteractable && ourInteractable.playerRange)
            {
                onTarget = true;
                selectedObject = ourInteractable.gameObject;

                interaction_text.text = ourInteractable.GetItemName();
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;

            }

            if (!ourInteractable && !animal && !enemy )
            {
                onTarget = false;
                handIsVisible = false;

                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

            if (!ourInteractable && !animal && !choppableTree && !enemy && !storageBox)
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }

            Campfire campfire = selectionTransform.GetComponent<Campfire>();

            if (campfire && campfire.playerInRange)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    campfire.ToggleCampfire();
                }
            }

        }

    }

    private void Loot(Lootable lootable)
    {
        if (lootable.wasLootCalculated == false)
        {
            List<LootRecieved> recievedLoot = new List<LootRecieved>();

            foreach (LootPossibility loot in lootable.possibleLoot)
            {

                // 0 -> 1 (50% drop rate) 1/2 0,1
                // -1 -> 1 (30% drop rate) 1/3 -1, 0 , 1
                // -2 -> 1 (25% drop rate) 1/4 -2, -1, 0 , 1
                // -3 -> 1 (20% drop rate) 1/5 -3, -2, -1, 0 , 1
                //-10 -> 1 (8% drop rate) 1/12 

                // -3 -> 2 (1/6, 1/6 ---> 2/6) -3, -2, -1, 0, 1 (%17), 2(%17) (33% to get something)

                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1); // 0 - 2    0 1 2 3
                if (lootAmount > 0)
                {
                    LootRecieved lt = new LootRecieved();
                    lt.item = loot.item;
                    lt.amount = lootAmount;

                    recievedLoot.Add(lt);

                }
            }

            lootable.finalLoot = recievedLoot;
            lootable.wasLootCalculated = true;
        }

        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i = 0; i < lootRecieved.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name + "_Model"),
                    new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.2f, lootSpawnPosition.z),
                    Quaternion.Euler(0, 0, 0));
            }
        }

        //Kan yerde kalsinmi
        //if (lootable.GetComponent<Animal>())
        //{
        //    lootable.GetComponent<Animal>.bloodPuddle.transform.SetParent(lootable.transform.parent);
        //}

        Destroy(lootable.gameObject);



    }
    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);
    }

    IEnumerator DealDamageToEnemy(EnemyAI enemy, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        enemy.TakeDamage(damage);
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}