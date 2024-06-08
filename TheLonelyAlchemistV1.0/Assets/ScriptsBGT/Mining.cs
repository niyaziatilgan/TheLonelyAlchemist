using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Mining : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeMined;

    public float oreMaxHealth;
    public float oreHealth;

    public Animator animator;

    public float caloriesSpentMiningIron = 20;

    private void Start()
    {
        oreHealth = oreMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


    public void GetHit()
    {
        animator.SetTrigger("mining");

        oreHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpentMiningIron;

        if (oreHealth <= 0)
        {
            OreIsDead();
        }

    }

    void OreIsDead()
    {
        Vector3 orePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeMined = false;
        SelectionManager.Instance.selectedOre = null;
        SelectionManager.Instance.oreHolder.gameObject.SetActive(false);

        GameObject brokenOre = Instantiate(Resources.Load<GameObject>("MinedOre"), new Vector3(orePosition.x, orePosition.y, orePosition.z), Quaternion.Euler(0, 0, 0));
        brokenOre.transform.SetParent(transform.parent.transform.parent.transform.parent);

        SoundManager.Instance.PlayOneShotMusic(SoundManager.Instance.oreBreaking);
    }

    private void Update()
    {
        if (canBeMined)
        {
            GlobalState.Instance.resourceHealth = oreHealth;
            GlobalState.Instance.resourceMaxHealth = oreMaxHealth;

        }
    }


}
