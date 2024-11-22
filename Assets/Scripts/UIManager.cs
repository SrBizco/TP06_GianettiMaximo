using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    private bool isGamePaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainPanel.SetActive(true);
        }
        else
        {
            mainPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            mainPanel.SetActive(true);
        }
        else
        {
            mainPanel.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
        Time.timeScale = 1;
        mainPanel.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        mainPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Gameplay"))
        {
            mainPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            mainPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
        }
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        mainPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
         #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit(); // Cierra la aplicación en una construcción (build).
    #endif
        Debug.Log("El juego se ha cerrado.");
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
    public void SetUIVolume(float volume)
    {
        audioMixer.SetFloat("UIVolume", Mathf.Log10(volume) * 20);
    }

    public void TogglePause()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuPanel.SetActive(false);
        isGamePaused = false;
    }

    public void BackToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }
    public void ToggleVictory()
    {
        victoryPanel.SetActive(true);
    }
    public void ToggleDefeat()
    {
        defeatPanel.SetActive(true);
    }
}