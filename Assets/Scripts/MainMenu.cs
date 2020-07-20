using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    public GameObject helpMenuUI;

    // For options menu
    public AudioMixer mixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private void Start() {
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

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    // ---------------------------------------------- Options
    public void Options(bool show) {
        mainMenuUI.SetActive(!show);
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

    // -------------------------------------------------- Help
    public void Help(bool show) {
        mainMenuUI.SetActive(!show);
        helpMenuUI.SetActive(show);
    }
}