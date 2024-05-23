using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public TMP_Text messageText;
    public Button okButton;
    public Button cancelButton;

    private System.Action<bool> responseCallBack;

    private void Start()
    {
        dialogBox.SetActive(false);

        okButton.onClick.AddListener(() => HandleResponse(true));
        cancelButton.onClick.AddListener(() => HandleResponse(false));
    }

    public void ShowDilaog(string message, System.Action<bool> callback)
    {
        responseCallBack = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    private void HandleResponse(bool response)
    {
        dialogBox.SetActive(false);
        responseCallBack?.Invoke(response);
    }




}
