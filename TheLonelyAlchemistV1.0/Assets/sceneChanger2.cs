using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class sceneChanger2 : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(change());
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator change()
    {
        yield return new WaitForSeconds(45f);
        SceneManager.LoadScene(1);
    }
}
