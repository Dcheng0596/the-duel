﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior classs Mouse 2 ability
public class WarriorM2Ability : Ability {

    BoxCollider2D m2;
    public float stunTime = 1;

    void Start ()
    {
        nAnim = GetComponent<NetworkAnimator>();
        nAnim.animator.SetBool("M2Pressed", false);
        this.slowDown = 20;
        this.coolDown = 5;
        this.damage = 15;
        this.onCoolDown = false;
        player = GetComponent<Player>();
    }
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;
        GetInput();
    }


    protected override void GetInput()
    {
        if (Input.GetMouseButton(1) && !onCoolDown)
        {
            StartCoroutine("CoolDown");
            nAnim.animator.SetBool("M2Pressed", true);          
        }      

    }

    protected void WarriorM2SlowDown()
    {
        this.SlowDown();
    }

    protected void WarriorM2UndoSlow()
    {
        this.UndoSlow();
    }
    // Deals damage at collision contact point and creates approriate damage text


    protected void WarriorM2CreateCollider()
    {
        GetComponent<OnDamage>().ability = OnDamage.Ability.M2;
        this.CreateCollider();
    }

    protected void WarriorM2DestroyCollider()
    {
        this.DestroyCollider();
    }

    protected override void CreateCollider()
    {
        if (!isLocalPlayer)
            return;

        m2 = gameObject.AddComponent<BoxCollider2D>();
        m2.isTrigger = true;
        m2.transform.Translate(Vector3.up * .00001f);
        m2.size = new Vector2(.3f, 1);
        m2.offset = new Vector2(.3f, 0);
    }
    
    protected override void DestroyCollider()
    {
        if (!isLocalPlayer)
            return;
        Destroy(m2);
        nAnim.animator.SetBool("M2Pressed", false);
    }


    IEnumerator CoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
    }
}
