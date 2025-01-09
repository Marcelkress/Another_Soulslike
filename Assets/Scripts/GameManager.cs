using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    private InputSwitcher inputSwitcher;
    public Transform mainCamera;
    public Transform cameraPositionAtPlayer;
    public Transform cameraPositionAtMenu;
    public Transform ResetPosition;
    public Button playButton; // Reference to the button


    public GameObject menuCanvas;
    public GameObject playerHudCanvas;
    public GameObject winText;

    [Header("Settings")]
    public int targetFramerate;
    public int waitForReloadTime;
    public float cameraLerpSpeed;

    public enum GameState
    {
        atMenu,
        playing
    };

    public GameState currentState;

    void Start()
    {
        inputSwitcher = player.GetComponent<InputSwitcher>();
        inputSwitcher.SwitchToUIMap();
        currentState = GameState.atMenu;

        Application.targetFrameRate = targetFramerate;
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }

    public void OnPlay()
    {
        StartCoroutine(LerpCamera(cameraPositionAtPlayer));

        menuCanvas.SetActive(false);
        playerHudCanvas.SetActive(true);
        currentState = GameState.playing;
        StartCoroutine(WaitForInputActivate());
        Cursor.lockState = CursorLockMode.Locked;

        mainCamera.gameObject.GetComponent<Camera>().cullingMask += 1 << LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        if(EnemyHealth.enemyCount == 0)
        {
            winText.SetActive(true);
            StartCoroutine(WaitForReloadScene());
        }
    }

    public void OnDeath()
    {
        StartCoroutine(WaitForReloadScene());   
    }

    private IEnumerator WaitForInputActivate()
    {
        yield return new WaitForSeconds(3);
        inputSwitcher.SwitchToPlayerMap();
    }

    private IEnumerator WaitForReloadScene()
    {
        yield return new WaitForSeconds(waitForReloadTime);

        SceneManager.LoadScene(0);
    }

    private IEnumerator LerpCamera(Transform target)
    {
        // Set the main camera's parent to the cameraPositionAtPlayer
        mainCamera.SetParent(target);

        // Store the initial position and rotation of the camera
        Vector3 initialPosition = mainCamera.position;
        Quaternion initialRotation = mainCamera.rotation;

        // Store the target position and rotation
        Vector3 targetPosition = target.position;
        Quaternion targetRotation = target.rotation;

        // Initialize the lerp factor
        float lerpFactor = 0f;

        // Lerp the camera's position and rotation over time
        while (lerpFactor < 1f)
        {
            lerpFactor += Time.deltaTime * cameraLerpSpeed;
            mainCamera.position = Vector3.Slerp(initialPosition, targetPosition, lerpFactor);
            mainCamera.rotation = Quaternion.Slerp(initialRotation, targetRotation, lerpFactor);
            yield return null;
        }

        // Ensure the camera is exactly at the target position and rotation
        mainCamera.position = targetPosition;
        mainCamera.rotation = targetRotation;

        mainCamera.localPosition = Vector3.zero;
    }
}
