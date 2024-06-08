using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    string itemName;

    public bool swingWait = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && InventorySystem.Instance.isOpen == false
            && CraftingSystem.Instance.isOpen == false
            && SelectionManager.Instance.handIsVisible == false
            && swingWait == false)
        {
            if (EquipSystem.Instance.selectedItem != null)
            {
                string itemName = EquipSystem.Instance.selectedItem.name;

                if (itemName.Contains("Sword"))
                {
                    swingWait = true;
                    StartCoroutine(SwingSoundDelay());
                    animator.SetTrigger("swordhit");
                    StartCoroutine(NewSwingDelay());
                }

                if (itemName.Contains("Axe"))
                {
                    swingWait = true;
                    StartCoroutine(SwingSoundDelay());
                    animator.SetTrigger("hit");
                    StartCoroutine(NewSwingDelay());
                }

                if (itemName.Contains("GreatSword"))
                {
                    swingWait = true;
                    StartCoroutine(SwingSoundDelay());
                    animator.SetTrigger("greatswordhit");
                    StartCoroutine(NewSwingDelay());
                }

                if (itemName.Contains("Pickaxe"))
                {
                    swingWait = true;
                    StartCoroutine(SwingSoundDelay());
                    animator.SetTrigger("pickaxehit");
                    StartCoroutine(NewSwingDelay());
                }
            }



        }
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        GameObject selectedOre = SelectionManager.Instance.selectedOre;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }

        if (selectedOre != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.hittingOre);
            selectedOre.GetComponent<Mining>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(1f);
        swingWait = false;
    }
}