﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController control;

    private LevelData levelData = new LevelData();
    public static LevelData ldLoaded = new LevelData();
    private PhantomControls player;
    //note: this is now part of uiControl - use that!
    private UIController uiControl;

    private List<Transform> entryPoints = new List<Transform>();

    private AsyncOperation sceneAsync;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(this.gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //LoadAllScenes(SceneManager.sceneCountInBuildSettings);
        PrintAllSceneInfo();
    }

    public void PrintAllSceneInfo()
    { 
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            Debug.Log("Scene at buildIndex of " + i + " is scene " + SceneManager.GetSceneByBuildIndex(i).name + ".");
        }
    }


    //todo: make load all scenes funciton;
    public void LoadAllScenes(int numScenesInBuild)
    {
        Debug.Log("loading all scenes; number of scenes in build is: " + numScenesInBuild);
        for (int i = 0; i < numScenesInBuild; i++)
        {
            if (!SceneManager.GetSceneByBuildIndex(i).isLoaded)
            {
                StartCoroutine(loadSceneAsyncly(i));
                //loadScene(i);
            }
            else
            {
                StopCoroutine(loadSceneAsyncly(i));
            }

        }
        //StopAllCoroutines();
    }

    public void loadScene(int buildIndex)
    {
        Debug.Log("loading scene at index: " + buildIndex);
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
    }

    public void disableScene(int buildIndex)
    {
        Debug.Log("unloading scene at index: " + buildIndex);
        SceneManager.UnloadSceneAsync(buildIndex);
    }

    IEnumerator loadSceneAsyncly(int buildIndex)
    {
        Debug.Log("loading scene at index: " + buildIndex);

        AsyncOperation scene = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
        scene.allowSceneActivation = false;
        sceneAsync = scene;

        while (!scene.isDone)
        {
            Debug.Log("Loading scene " + scene.ToString() + " Progress: " + scene.progress);
            yield return null;
        }

        //enableScene(buildIndex);
    }

    void enableScene(int buildIndex)
    {
        Debug.Log("enabling scene at: " + buildIndex);
        //Activate the Scene
        sceneAsync.allowSceneActivation = true;

        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(buildIndex);
        if (sceneToLoad.IsValid())
        {
            Debug.Log("Scene is Valid; moving player");

            StartCoroutine(loadSceneAsyncly(buildIndex));

            SceneManager.MoveGameObjectToScene(player.gameObject, sceneToLoad);
            SceneManager.SetActiveScene(sceneToLoad);
        }
    }

    void disableOtherScenes()
    {
        Debug.Log("Disabling scenes");
        for (int count = 0; count < (SceneManager.sceneCountInBuildSettings); count++)
        {
            if (SceneManager.GetSceneAt(count).isLoaded)
            {
                if (SceneManager.GetSceneAt(count) != SceneManager.GetActiveScene())
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(count));
                }

            }
        }
    }


    private void Update()
    {
        //LoadAllScenes();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(loadSceneAsyncly(1));
            enableScene(1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(loadSceneAsyncly(2));
            enableScene(2);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
        }
    }

    public void SaveLevel()
    {
        Debug.Log("Save and load Controller saving.");

        player = GameObject.Find("Phantom2.0").GetComponent<PhantomControls>();
        uiControl = GameObject.Find("Phantom2.0").GetComponent<UIController>();

        PlayerData playerDat = new PlayerData();

        //scene stuff
        levelData.sceneNum = player.gameObject.scene.buildIndex;
        Debug.Log("Build index of this scene?: " + player.gameObject.scene.buildIndex);
        Debug.Log("levelData.scenenum?: " + levelData.sceneNum);

        //player stuff
        playerDat.currentHealth = uiControl.currentHealth;

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

            enemyDat.currentHealth = meleeEnemy.gameObject.GetComponent<UIController>().currentHealth;
            enemyDat.currentMana = meleeEnemy.gameObject.GetComponent<UIController>().currentMana;
            levelData.enemyList.Add(enemyDat);
        }

        //mageStuff
        foreach (GameObject mageEnemy in GameObject.FindGameObjectsWithTag("mage"))
        {
            EnemyData enemyDat = new EnemyData();

            enemyDat.xPos = mageEnemy.transform.position.x;
            enemyDat.yPos = mageEnemy.transform.position.y;
            enemyDat.zPos = mageEnemy.transform.position.z;

            enemyDat.currentHealth = mageEnemy.gameObject.GetComponent<UIController>().currentHealth;
            enemyDat.currentMana = mageEnemy.gameObject.GetComponent<UIController>().currentMana;
            levelData.enemyList.Add(enemyDat);
        }

        //healerStuff
        foreach (GameObject healerEnemy in GameObject.FindGameObjectsWithTag("healer"))
        {
            EnemyData healerDat = new EnemyData();
            healerDat.xPos = healerEnemy.transform.position.x;
            healerDat.yPos = healerEnemy.transform.position.y;
            healerDat.zPos = healerEnemy.transform.position.z;

            healerDat.currentHealth = healerEnemy.gameObject.GetComponent<UIController>().currentHealth;
            healerDat.currentMana = healerEnemy.gameObject.GetComponent<UIController>().currentMana;
            levelData.enemyList.Add(healerDat);
        }

        //commmoner stuff
        foreach (GameObject commonEnemy in GameObject.FindGameObjectsWithTag("Commoner"))
        {
            EnemyData commonerDat = new EnemyData();
            commonerDat.xPos = commonEnemy.transform.position.x;
            commonerDat.yPos = commonEnemy.transform.position.y;
            commonerDat.zPos = commonEnemy.transform.position.z;

            commonerDat.currentHealth = commonEnemy.gameObject.GetComponent<UIController>().currentHealth;
            commonerDat.currentMana = commonEnemy.gameObject.GetComponent<UIController>().currentMana;
            levelData.enemyList.Add(commonerDat);
        }

        //nopossess stuff
        foreach (GameObject noPossess in GameObject.FindGameObjectsWithTag("NoPossess"))
        {
            EnemyData noPossessDat = new EnemyData();
            noPossessDat.xPos = noPossess.transform.position.x;
            noPossessDat.yPos = noPossess.transform.position.y;
            noPossessDat.zPos = noPossess.transform.position.z;

            noPossessDat.currentHealth = noPossess.gameObject.GetComponent<UIController>().currentHealth;
            noPossessDat.currentMana = noPossess.gameObject.GetComponent<UIController>().currentMana;
            levelData.enemyList.Add(noPossessDat);
        }
        /*
         * To be added:
         * Data for healer, commoner, doggos, things with 'nopossess' tag-done
         * Mana in level data-done
         * 
         * To be fixed:
         * References to player health, should be referencing UIController-done
         * Remove save points and replace with saving via possessing scribe, frameworkish is in phantomcontrols script
         * 
         * 
         * 
         * 
         * */
        SaveAndLoad.savedGames.Add(this.levelData);
        Debug.Log("SavedGames count: " + SaveAndLoad.savedGames.Count);
        SaveAndLoad.Save();
    }

    public void LoadLevel()
    {
        ldLoaded = SaveAndLoad.Load();

        //load in player data from the data given from the file
        SceneManager.LoadScene(SaveAndLoad.savedGames[0].sceneNum);
    }

    public void SetPlayerPos(PhantomControls phantom, LevelData ldLoaded)
    {
        Vector3 levelEntry = new Vector3(0, 0, 0);
        if (ldLoaded.player.xPos == 0 && ldLoaded.player.yPos == 0 && ldLoaded.player.zPos == 0)
        {
            Transform[] transformsFromScene = FindObjectsOfType<Transform>();
            foreach (Transform objectInScene in transformsFromScene)
            {
                if (objectInScene.tag == "EntryPoints")
                {
                    entryPoints.Add(objectInScene);
                }
            }
            levelEntry = entryPoints[0].transform.position;
            phantom.transform.position = levelEntry;
            //Vector3 newPlayerPos = GameObject.Find("EntryPoint").transform.position;
        }
        else
        {
            Vector3 newPlayerPos = new Vector3(ldLoaded.player.xPos, ldLoaded.player.yPos, ldLoaded.player.zPos);
            Debug.Log("load level setting player pos to:" + newPlayerPos);
            phantom.transform.position = levelEntry;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        //LoadAllScenes(SceneManager.sceneCountInBuildSettings);

        player = GameObject.Find("Phantom2.0").GetComponent<PhantomControls>();
        SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetActiveScene());
        //Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(level);
        //loadScene(level);
        SetPlayerPos(player, ldLoaded);
    }
}
