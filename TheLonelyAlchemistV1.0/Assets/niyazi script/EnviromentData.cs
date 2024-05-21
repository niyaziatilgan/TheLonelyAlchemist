using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnviromentData 
{
    public List<string> pickedupItems;

    public EnviromentData(List<string> _pickedupItems) 
    {
        pickedupItems = _pickedupItems;
    }
}
