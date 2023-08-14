using System.Collections;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    private Camera lookAtCamera;
    private bool isLookingAt;

    public void SetLookAtCamera(Camera targetCamera)
    {
        lookAtCamera = targetCamera;
        isLookingAt = true;
    }

    private void FixedUpdate()
    {
        if (!isLookingAt) return;
        if (lookAtCamera == null)
        {
            isLookingAt = false;
        }
        else
        {
            transform.LookAt(-lookAtCamera.transform.position);
        }
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void HideWithDelay(float f)
    {
        StartCoroutine(HideDelayCoroutine(f));
    }

    IEnumerator HideDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideUI();
    }
}