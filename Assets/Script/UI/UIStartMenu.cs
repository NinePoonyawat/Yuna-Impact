using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartMenu : MonoBehaviour
{
    public string nextScene;
    public GameObject image;
    public GameObject text;

    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(SwitchScene());
        }
    }

    IEnumerator SwitchScene()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.2f);
        image.SetActive(true);
        text.SetActive(false);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextScene);
    }
}
