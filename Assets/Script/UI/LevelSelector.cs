using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void PlayScene()
    {
        SceneManager.LoadScene("Street01");
    }

    public void Exit()
	{
		Application.Quit();
	}
}
