using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    public float currentHealth;
    public float maxHealth;


    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;
    public PlayerMovement playerMovements;
    public MouseMovement mouseMovements;

    public bool playerDead;

    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydrationActive;
    public bool isDecrasingHydration;

    public GameObject DeathSceneCanvas;

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
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;
        StartCoroutine(decreaseHydration());
        StartCoroutine(decreaseHealth());
        playerDead = false;

        playerMovements = playerBody.GetComponent<PlayerMovement>();
        mouseMovements = playerBody.GetComponent<MouseMovement>();
    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            if (currentHydrationPercent > 0)
            {
                currentHydrationPercent -= 1;
                yield return new WaitForSeconds(7);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator decreaseHealth()
    {
        while (true)
        {
            if (isDecrasingHydration == true)
            {
                currentHealth -= 1;
                yield return new WaitForSeconds(10);
            }
            else
            {
                yield return null;
            }

        }

    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

        if (currentHealth <= 0)
        {
            DeathSceneCanvas.SetActive(true);

            playerMovements.enabled = false;
            mouseMovements.enabled = false;

            MenuManager.Instance.UICanvas.SetActive(false);
            MenuManager.Instance.menuCanvas.SetActive(false);

            playerDead = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }

        if (currentHydrationPercent <= 0)
        {
            isDecrasingHydration = true;

        }
        else
        {
            isDecrasingHydration = false;
        }


    }


    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }
    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }
    public void setHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }
}
