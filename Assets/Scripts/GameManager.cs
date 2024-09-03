using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this if you're using UI elements like Text
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject bug; // Prefab of the bug to spawn
    public float maxX; // Maximum X distance for spawning
    public RectTransform spawnPoint; // Spawn point for the bugs
    public float spawnRate; // Time between each spawn
    public Canvas canvas; // Reference to the Canvas
    public TextMeshProUGUI scoreText; // Reference to the UI TextMeshPro component that displays the score
    public GameObject tapText;
    public GameObject Logo;
    public Button exitButton; // Reference to the exit button

    bool gameStarted = false;
    int score = 0; // Score counter

    void Start()
    {
        // Assuming the canvas is using Screen Space - Overlay or Camera
        // Create a new GameObject for the spawn point
        GameObject spawnPointObject = new GameObject("SpawnPoint");

        // Add RectTransform and set it up
        RectTransform rectTransform = spawnPointObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, 0); // No size since it's just a point
        rectTransform.anchorMin = new Vector2(0.5f, 1); // Anchored to the top middle
        rectTransform.anchorMax = new Vector2(0.5f, 1); // Anchored to the top middle
        rectTransform.anchoredPosition = new Vector2(0, canvas.GetComponent<RectTransform>().sizeDelta.y / 2); // Set to top of the canvas

        // Set the parent of the spawn point to the canvas
        rectTransform.SetParent(canvas.transform, false);

        // Assign the spawn point object's RectTransform to the spawnPoint variable
        spawnPoint = rectTransform;

        exitButton.onClick.AddListener(OnExitButtonClicked);

    }

    void Update()
    {
        // Start spawning bugs when the first mouse button is clicked and the game hasn't started yet
        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            StartSpawning();
            gameStarted = true;
            tapText.SetActive(false);
            Logo.SetActive(false);
            exitButton.gameObject.SetActive(false);

        }

    }

    private void StartSpawning()
    {
        InvokeRepeating("SpawnBug", 0.5f, spawnRate);
    }

    private void SpawnBug()
    {
        GameObject bugInstance = Instantiate(bug) as GameObject;
        bugInstance.transform.SetParent(canvas.transform, false);
        bugInstance.SetActive(true); // Ensure the bug instance is active

        RectTransform bugRectTransform = bugInstance.GetComponent<RectTransform>();
        bugRectTransform.anchoredPosition = new Vector2(Random.Range(-maxX, maxX), spawnPoint.anchoredPosition.y);

        score++; // Increment score
        scoreText.text = score.ToString();
    }

    // Method called when the exit button is clicked
    public void OnExitButtonClicked()
    {
        // Send the score to the Android app
        SendScoreToAndroidApp(score);

        // Exit the Unity game
        Application.Quit();
    }
    // Method to send the score to the Android app
    private void SendScoreToAndroidApp(int finalScore)
    {
        // Create a new AndroidJavaClass object for the UnityPlayer class
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // Get the current Unity activity from the UnityPlayer class
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        // Create a new AndroidJavaClass object for your custom Android class that handles score submission
        AndroidJavaClass gameActivity = new AndroidJavaClass("com.asma.appgametry.GameActivity");
        // Call the static method of your custom class that handles score submission
        gameActivity.CallStatic("SubmitScore", currentActivity, finalScore);
    }
}
