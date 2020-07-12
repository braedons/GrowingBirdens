using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int startingWidth = 3;
    private int currWidth;
    public int startingHeight = 3;
    private int currHeight;

    public float templateWidth;
    public float templateHeight;
    public List<GameObject> levelTemplates; // Widths can differ, heights must be the same
    private float bottomOfMap;

    public PlayerSwitch playerSwitch;
    public Grid grid;

    private bool gameOver = false;
    public GameObject restartMenu;


    // Start is called before the first frame update
    void Awake()
    {
        currWidth = startingWidth;
        currHeight = startingHeight;
        bottomOfMap = -templateHeight * 2;

        GameObject.FindGameObjectWithTag("Cursor").GetComponent<SpriteRenderer>().enabled = false;

        LoadLevel(startingWidth, startingHeight);
    }

    public void LoadLevel(int width, int height) {
        // If inputs are -1, just increment the size
        if (width == -1)
            width = ++currWidth;
        if (height == -1)
            height = ++currHeight;
        

        // Delete previous level templates
        foreach (Transform child in grid.transform) {
            Destroy(child.gameObject);
        }

        // Delete all nests
        foreach (GameObject nest in GameObject.FindGameObjectsWithTag("Nest")) {
            Destroy(nest);
        }

        // Generate grid and objects
        Vector3 spawnPoint = grid.transform.position;
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {                    
                Instantiate(PickTemplate(), spawnPoint, Quaternion.identity).transform.SetParent(grid.transform);
                spawnPoint.x += templateWidth;
            }

            spawnPoint.x = grid.transform.position.x;
            spawnPoint.y += templateHeight;
        }

        // Set new player locations
        playerSwitch.AssignPlayerSpawns();
    }

    public GameObject PickTemplate() {
        int rand = Random.Range(0, levelTemplates.Count);
        return levelTemplates[rand];
    }

    public void GameOver() {
        if (!gameOver) {
            gameOver = true;
            Debug.Log("Game over");
            restartMenu.SetActive(true);
        }
    }

    public void Restart() {
        restartMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }

    public float GetBottomOfMap() {
        return bottomOfMap;
    }
}
