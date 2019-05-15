﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    private bool paused = false;
    public Canvas pauseMenu;
    public GameObject EventSystem; 

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(EventSystem);
        paused = false;
        pauseMenu.enabled = paused; 
    }

    // Update is called once per frame
    void Update()
    {
        pauseMenu.enabled = paused;

        if (Input.GetKeyDown(KeyCode.Escape))
            paused = togglePause();
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }

    public void OnButtonPressX()
    {
        paused = togglePause();
    }

    public void OnButtonPressMainMenu()
    {
        paused = togglePause();
        SceneManager.LoadScene(0);
        if (GameObject.FindGameObjectWithTag("Possessed") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Possessed"));
            Destroy(this.gameObject);
            Destroy(EventSystem);

        }
        else if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            Destroy(this.gameObject);
            Destroy(EventSystem);
        }
    }
    
    public void OnButtonPressQuit()
    {
        Application.Quit();
    }
}