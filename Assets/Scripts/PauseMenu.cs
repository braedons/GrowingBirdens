using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    private SpriteRenderer cursorRenderer;

    public PlayerSwitch playerSwitch;

    private void Start() {
        cursorRenderer = GameObject.FindGameObjectWithTag("Cursor").GetComponent<SpriteRenderer>();
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
        pauseMenuUI.SetActive(false);
        cursorRenderer.enabled = false;
        Time.timeScale = 1f;
        isPaused = false;

        playerSwitch.Unpause();
    }

    void Pause() {
        Time.timeScale = 0f;
        cursorRenderer.enabled = true;
        pauseMenuUI.SetActive(true);
        isPaused = true;

        playerSwitch.Pause();
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
