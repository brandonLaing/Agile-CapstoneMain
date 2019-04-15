﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 target;
    public float speed = 25f;
    //public float maxDistance = 15f;
    public float damage;

    // Use this for initialization
    void Start()
    {
        if(this.transform.parent.gameObject.GetComponent<MageAI>())
        damage = this.transform.parent.gameObject.GetComponent<MageAI>().fireDamageAmount;
        if (this.transform.parent.gameObject.GetComponent<healerAI>())
            damage = this.transform.parent.gameObject.GetComponent<healerAI>().fireDamageAmount;
        this.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
    }

    public void setTarget(Vector3 targetPoint)
    {
        target = targetPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // checks if the triggerd object is in the right layer if it is it adds it to potential list
        if (other.gameObject.layer == LayerMask.NameToLayer("AI"))
        {
            print("Fireball Hit on: " + other.gameObject.name);
            other.gameObject.GetComponent<UIController>().takeDamage(damage);

        }
    }
}
