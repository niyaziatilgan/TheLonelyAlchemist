using System.Collections.Generic;
using UnityEngine;

public class PlacebleItem : MonoBehaviour
{
    public bool isGrounded;
    public bool isOverlappingItems;
    public bool isValidToBeBuilt;

    [SerializeField] BoxCollider solidCollider;
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (isGrounded && isOverlappingItems == false)
        {
            isValidToBeBuilt = true;
        }
        else
        {
            isValidToBeBuilt = false;
        }

        var boxHeight = transform.lossyScale.y;

        RaycastHit groundHit;
        Debug.DrawRay(transform.position, Vector3.down ,Color.red, boxHeight *0.5f);
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, boxHeight, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    #region || --- On Triggers --- |
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && PlacementSystem.Instance.inPlacementMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = newRotation;

                isGrounded = true;
            }
        }

        if (other.CompareTag("Tree") || other.CompareTag("pickable"))
        {
            isOverlappingItems = true;
        }
    }
    #endregion

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") && PlacementSystem.Instance.inPlacementMode)
        {
            isGrounded = false;
        }

        if (other.CompareTag("Tree") || other.CompareTag("pickable") && PlacementSystem.Instance.inPlacementMode)
        {
            isOverlappingItems = false;
        }
    }

    #region || --- Set Outline Colors --- |
    public void SetInvalidColor()
    {
        if (outline != null)
        {
            outline.enabled = true;
            outline.OutlineColor = Color.red;
        }

    }

    public void SetValidColor()
    {
        if (outline != null)
        {
            outline.enabled = true;
            outline.OutlineColor = Color.green;
        }
    }

    public void SetDefaultColor()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
    #endregion
}
