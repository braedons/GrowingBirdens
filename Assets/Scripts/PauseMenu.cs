using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour {
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    private SpriteRenderer cursorRenderer;

    public PlayerSwitch playerSwitch;

    // For options menu
    public AudioMixer mixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private void Start() {
        // cursorRenderer = GameObject.FindGameObjectWithTag("Cursor").GetComponent<SpriteRenderer>();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();

        int currResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            resolutionOptions.Add(resolutions[i].width + " x " + resolutions[i].height);
            
            if (resolutions[i].width == Screen.width
                    && resolutions[i].height == Screen.height)
                currResIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currResIndex;
        resolutionDropdown.RefreshShownValue();
        
        // Set volume slider to initial
        float currVol;
        if (mixer.GetFloat("volume", out currVol))
            volumeSlider.value = Mathf.Pow(10, currVol / 20);

        // Set fullscreen to initial
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        // Set menus false
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);

        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;

        playerSwitch.Unpause();
    }

    void Pause() {
        Time.timeScale = 0f;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        isPaused = true;

        playerSwitch.Pause();
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Options(bool show) {
        pauseMenuUI.SetActive(!show);
        optionsMenuUI.SetActive(show);
    }

    public void SetVolume(float vol) {
        mixer.SetFloat("volume", Mathf.Log10(vol) * 20);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int index) {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public bool IsPaused() {
        return isPaused;
    }
}
