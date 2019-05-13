﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * The goal of this is to set target of possession for the phantom
 * 
 * returns a Gameobject
 */
public class PhantomRange : MonoBehaviour
{

    public List<GameObject> inRange = new List<GameObject>();

    public GameObject primaryTarget;

    public PhantomControls controller;

    public GameObject phantom;
    public GameObject parent;

    Scene currentScene;
    int buildIndex;

    // on start set up link between this is and phantom controlls
    private void Start()
    {
        controller = gameObject.GetComponentInParent<PhantomControls>();
        currentScene = SceneManager.GetActiveScene();
        buildIndex = currentScene.buildIndex;
        phantom = this.gameObject;
    }

    private void Update()
    {
        // if there are more than 1 potential targets in range
        if (inRange.Count > 1)
        {
            GameObject closest = null;

            // finds closest potential targets
            foreach (GameObject target in inRange)
            {
                if (closest == null)
                {
                    closest = target;
                }
                else
                {
                    if (Vector2.Distance(this.transform.position, closest.transform.position) >
                      Vector2.Distance(this.transform.position, target.transform.position))
                    {
                        closest = target;
                    }
                }
            }
            // sets the target to the closest one
            primaryTarget = closest;
        }
        // if there is just one potential targets it makes that the primary
        else if (inRange.Count == 1)
        {
            primaryTarget = inRange[0];
        }
        // no targets in range no primary targets
        else
        {
            primaryTarget = null;
        }

        // sets the target in here to the primary one
        controller.phantomTarget = primaryTarget;

        if (phantom.transform.parent != null)
            parent = phantom.transform.parent.gameObject;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("something is hitting me (the phantom): " + other.transform.name);

        // checks if the triggerd object is in the right layer if it is it adds it to potential list
        if(!GameObject.FindGameObjectWithTag("Possessed"))
        if (other.gameObject.tag == "mage" || other.gameObject.tag == "Melee" || other.gameObject.tag == "healer" || other.gameObject.tag == "Scribe" )
        {
            inRange.Add(other.gameObject);
        }

        if(other.gameObject.name == "Reaper2.0" || other.gameObject.tag == "Reaper")
        {
            controller.Die();
        }

        if (other.tag == "Exit")
        {
            //load the next scene?
            Debug.Log("scene #: " + buildIndex);
            if (buildIndex == 0)
            {
                buildIndex++;
            }
            if (parent != null)
            {
                DontDestroyOnLoad(parent);
            }
            else DontDestroyOnLoad(phantom);

            SceneManager.LoadScene(buildIndex + 1);
            buildIndex++;
           //parent.transform.position = GameObject.FindGameObjectWithTag("EntryPoints").transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // checks if its in the right layer then if it is it removes it from potential list

        if (other.gameObject.tag == "mage" || other.gameObject.tag == "Melee" || other.gameObject.tag == "healer" || other.gameObject.tag == "Scribe")
        {
            inRange.Remove(other.gameObject);
        }
    }
}