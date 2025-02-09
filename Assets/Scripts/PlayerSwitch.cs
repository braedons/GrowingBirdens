﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerSwitch : MonoBehaviour {
    public int startingPlayerCount = 1;
    private List<GameObject> players = new List<GameObject>();
    private int currPlayer = 0;

    private VitalityBar healthBar;
    private VitalityBar hungerBar;

    private CameraFollow camFollow;

    public BirdNumber birdNumber;

    public GameObject playerPrefab;

    private bool isPaused = false;

    private HashSet<GameObject> nestedPlayers = new HashSet<GameObject>();
    public LevelManager levelManager;

    private bool switchActive = true;

    void Start() {
        // Create players
        for (int i = 0; i < startingPlayerCount; i++) {
            CreatePlayer();
        }

        // Set new player locations
        AssignPlayerSpawns();
        
        // Disable all but 0th player's PlayerController
        for (int i = 1; i < players.Count; i++) {
            DisablePlayer(i);
        }

        camFollow = Camera.main.transform.parent.GetComponent<CameraFollow>();
        camFollow.UpdateTarget(players[currPlayer]);

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<VitalityBar>();
        healthBar.SetMaxVitality(players[currPlayer].GetComponent<Vitality>().GetMaxVitality(Vitality.Stat.Health));
        hungerBar = GameObject.FindGameObjectWithTag("HungerBar").GetComponent<VitalityBar>();
        hungerBar.SetMaxVitality(players[currPlayer].GetComponent<Vitality>().GetMaxVitality(Vitality.Stat.Satiety));

        SwitchUI(players[currPlayer]);
    }

    void Update() {
        if (Input.GetKeyDown("q") && !isPaused && switchActive) {
            // Switch control
            DisablePlayer(currPlayer);
            EnablePlayer(NextPlayer());

            // Update UI
            SwitchUI(players[currPlayer]);

            // Update camera
            camFollow.UpdateTarget(players[currPlayer]);
        }
    }

    int NextPlayer() {
        currPlayer = (currPlayer + 1) % players.Count;
        return currPlayer;
    }

    public void CreatePlayer() {
        GameObject clone = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        clone.transform.SetParent(gameObject.transform);
        players.Add(clone);
    }

    private void SwitchUI(GameObject player) {
        healthBar.SetVitality(player.GetComponent<Vitality>().GetVitality(Vitality.Stat.Health));
        hungerBar.SetVitality(player.GetComponent<Vitality>().GetVitality(Vitality.Stat.Satiety));
        birdNumber.SetBirdNumber(currPlayer+1);
        player.GetComponent<Inventory>().EditQuantity("Stick", 0); // Switches inventory count
    }

    public List<GameObject> GetPlayers() {
        return players;
    }

    public void AssignPlayerSpawns() {
        List<GameObject> spawns = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerSpawnPoint"));

        if (spawns.Count > 0) {
            foreach (var player in players) {
                int rand = Random.Range(0, spawns.Count);
                player.transform.position = spawns[rand].transform.position;
            }
        }
    }

    public void Pause() {
        isPaused = true;
        players[currPlayer].GetComponent<PlayerController>().SetActive(false);
    }

    public void Unpause() {
        isPaused = false;
        players[currPlayer].GetComponent<PlayerController>().SetActive(true);
    }

    public void NestPlayer(GameObject player) {
        nestedPlayers.Add(player);
        birdNumber.SetNestsLeft(players.Count - nestedPlayers.Count);

        // Set player inactive, freeze hunger, play animation
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.SetActive(false);
        pc.SetNested(true);
        player.GetComponent<Vitality>().FreezeHunger(true);
        player.GetComponent<Animator>().SetBool("Nested", true);

        // Check if all are nested
        if (nestedPlayers.Count == players.Count) {
            NextLevel();
        }
    }

    void NextLevel() {
        nestedPlayers = new HashSet<GameObject>();

        foreach (GameObject curr in players) {
            curr.GetComponent<PlayerController>().ResetPlayer();
        }

        CreatePlayer();

        // Switch control
        DisablePlayer(currPlayer);
        EnablePlayer(0);
        currPlayer = 0;

        // Reset UI
        SwitchUI(players[0]);
        birdNumber.SetNestsLeft(players.Count);

        // Update camera
        camFollow.UpdateTarget(players[currPlayer]);

        levelManager.LoadLevel(-1, -1);
    }

    private void EnablePlayer(int player) {
        if (!players[player].GetComponent<PlayerController>().IsNested()) {
            players[player].GetComponent<PlayerController>().SetActive(true);
            players[player].GetComponent<Rigidbody2D>().mass = 1;
        }
    }

    private void DisablePlayer(int player) {
        players[player].GetComponent<PlayerController>().SetActive(false);
        players[player].GetComponent<Rigidbody2D>().mass = 100;
    }

    public void SetActivePlayer(GameObject player) {
        for (int i = 0; i < players.Count; i++) {
            if (player == players[i]) {
                SwitchUI(players[i]);
                camFollow.UpdateTarget(players[i]);
                break;
            }
        }
    }

    public void SetSwitchActive(bool active) {
        switchActive = active;
    }
}
