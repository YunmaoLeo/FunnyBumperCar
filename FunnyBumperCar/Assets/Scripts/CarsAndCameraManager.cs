using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarsAndCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;
    public static CarsAndCameraManager Instance { get; private set; }

    private Transform P1CarTransform;
    private Transform P2CarTransform;

    private bool isPause = false;
    [SerializeField] private Transform PauseGameUI;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterCar(Transform car, int playerIndex)
    {
        if (playerIndex == 0)
        {
            P1CarTransform = car;
        }
        else
        {
            P2CarTransform = car;
        }

        cameraTargetGroup.AddMember(car, 1, 10);
    }

    public void RegisterVisualEffect(Transform effect, float time = 1f)
    {
        cameraTargetGroup.AddMember(effect, 1f, 10);
        StartCoroutine(RemoveTransformAfterCertainTime(effect, time * 0.8f));
    }

    IEnumerator RemoveTransformAfterCertainTime(Transform transform, float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraTargetGroup.RemoveMember(transform);
    }

    public Transform GetHostileCar(Transform transformWithRb)
    {
        if (transformWithRb.CompareTag("Car"))
        {
            return P1CarTransform == transformWithRb ? P2CarTransform : P1CarTransform;
        }
        else
        {
            return GetHostileCar();
        }
    }

    public Transform GetHostileCar()
    {
        return Random.Range(0, 2) == 1 ? P1CarTransform : P2CarTransform;
    }

    public void PauseGame()
    {
        if (!isPause)
        {
            isPause = true;
            Time.timeScale = 0f;
            PauseGameUI.gameObject.SetActive(true);
        }
        else
        {
            isPause = false;
            Time.timeScale = 1f;
            PauseGameUI.gameObject.SetActive(false);
        }
    }

    public void PauseGame(InputAction.CallbackContext obj)
    {
        PauseGame();
    }
}