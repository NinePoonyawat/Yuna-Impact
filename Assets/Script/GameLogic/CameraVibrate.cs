using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVibrate : MonoBehaviour
{
    public Camera cam;

    void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }

    public void Vibrate(float duration, float magnitude)
    {
        StartCoroutine(CamVibrate(duration,magnitude));
    }

    public IEnumerator CamVibrate(float duration, float magnitude)
    {
        Vector3 originalPos = cam.transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f,1f) * magnitude;

            cam.transform.position = new Vector3(originalPos.x + x,originalPos.y + y,originalPos.z);
            Debug.Log(cam.transform.localPosition);
            elapsed += Time.deltaTime;

            yield return null;
        }

        cam.transform.localPosition = originalPos;
    }
}
