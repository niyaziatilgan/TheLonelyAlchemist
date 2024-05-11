using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    //  PLAYER HEALTH //
    public float currentHealth;
    public float maxHealth;


    // PLAYER CALORIES //
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;


    // PLAYER HYDRATION //
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydrationActive;

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
    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(2);
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
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
