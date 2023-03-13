using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFacingCam : MonoBehaviour
{
    private Camera mainCamera;

    private SpriteRenderer[] sprites;

    private void Awake()
    {
        mainCamera = Camera.main;
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    public void TakeDamage()
    {
        StartCoroutine(Damage());
    }

    IEnumerator Damage()
    {
        foreach (SpriteRenderer sr in sprites)
        {
            sr.color = new Color(1f, 0.2f, 0.2f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer sr in sprites)
        {
            sr.color = new Color(1f, 1f, 1f);
        }
    }
}
