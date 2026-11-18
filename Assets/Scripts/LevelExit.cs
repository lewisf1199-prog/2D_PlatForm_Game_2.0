using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Leave empty to automatically load the next scene in Build Settings.")]
    [SerializeField] private string nextSceneName = "";

    private bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLoading && other.CompareTag("Player"))
        {
            isLoading = true;
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("You beat the final level! Returning to the first scene.");
                SceneManager.LoadScene(0);
            }
        }
    }
}