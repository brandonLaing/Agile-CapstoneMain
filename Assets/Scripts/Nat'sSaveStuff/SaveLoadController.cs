﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController control;

    private LevelData levelData = new LevelData();
    private PhantomControls player;
    //note: this is now part of uiControl - use that!
    private playerHealth hpScript;
    
    void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(this.gameObject);
            control = this;
        }
        else if(control != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.Find("Phantom2.0").GetComponent<PhantomControls>();
        hpScript = player.GetComponent<playerHealth>();
    }

    public void SaveLevel()
    {
        Debug.Log("Save and load Controller saving.");

        PlayerData playerDat = new PlayerData();

        //scene stuff
        levelData.sceneNum = player.gameObject.scene.buildIndex;
        Debug.Log("Build index of this scene?: " + player.gameObject.scene.buildIndex);
        Debug.Log("levelData.scenenum?: " + levelData.sceneNum);

        //player stuff
        playerDat.currentHealth = hpScript.currentHealth;

        playerDat.xPos = player.gameObject.transform.position.x;
        playerDat.yPos = player.gameObject.transform.position.y;
        playerDat.zPos = player.gameObject.transform.position.z;

        playerDat.isPossessing = player.isPossessing;
        levelData.player = playerDat;
        
        //melee stuff
        foreach (GameObject meleeEnemy in GameObject.FindGameObjectsWithTag("Melee"))
        {
            EnemyData enemyDat = new EnemyData();

            enemyDat.xPos = meleeEnemy.transform.position.x;
            enemyDat.yPos = meleeEnemy.transform.position.y;
            enemyDat.zPos = meleeEnemy.transform.position.z;

            enemyDat.currentHealth = meleeEnemy.gameObject.GetComponent<AIHealth>().currentHealth;
            levelData.enemyList.Add(enemyDat);
        }

        //mageStuff
        foreach (GameObject mageEnemy in GameObject.FindGameObjectsWithTag("mage"))
        {
            EnemyData enemyDat = new EnemyData();

            enemyDat.xPos = mageEnemy.transform.position.x;
            enemyDat.yPos = mageEnemy.transform.position.y;
            enemyDat.zPos = mageEnemy.transform.position.z;

            enemyDat.currentHealth = mageEnemy.gameObject.GetComponent<AIHealth>().currentHealth;
            levelData.enemyList.Add(enemyDat);
        }
        
        SaveAndLoad.savedGames.Add(this.levelData);
        SaveAndLoad.Save();
    }

    public void LoadLevel()
    {
        Debug.Log("SavedGames count: " + SaveAndLoad.savedGames.Count);
        LevelData ld = SaveAndLoad.Load();

        //load in player data from the data given from the file
        SceneManager.LoadScene(SaveAndLoad.savedGames[0].sceneNum);
        player.transform.position = new Vector3(ld.player.xPos, ld.player.yPos, ld.player.zPos);
    }

}