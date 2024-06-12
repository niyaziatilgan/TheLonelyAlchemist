using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneChanger : MonoBehaviour
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
        yield return new WaitForSeconds (35f);
        SceneManager.LoadScene(1);

    }
}
