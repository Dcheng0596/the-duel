﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior classe's Mouse 1 ability
public class WarriorM1Ability : Ability{

    PolygonCollider2D m1;

    void Start() {
        slowDown = 10;
        this.damage = 5;
        player = GetComponent<Player>();
        nAnim = GetComponentInParent<NetworkAnimator>();

    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        GetInput();
    }

    // Deals damage at collision contact point and creates approriate damage text
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isLocalPlayer)
            return;
        Debug.Log("M1");
        GameObject enemy = collision.gameObject;

        string uIdentity = enemy.transform.name;
        string myIdentity = this.gameObject.transform.name;

        if (enemy == null)
            return;
        if (enemy.transform.tag == "Player" && uIdentity != myIdentity)
        {
  
            this.CmdDealDamage(uIdentity, damage);

            ContactPoint2D[] contacts = new ContactPoint2D[1] ;
            collision.GetContacts(contacts);

            Vector2 colPos = enemy.transform.position;
            CmdSendDamageText(uIdentity, damage, colPos);

        }
    }

    // Wrapper functions for animation events
    protected void WarriorM1CreateCollider()
    {
        this.CreateCollider();
    }

    protected void WarriorM1DestroyCollider()
    {
        this.DestroyCollider();
    }

    protected void WarriorM1SlowDown()
    {
        this.SlowDown();
    }

    protected void WarriorM1UndoSlow()
    {
        this.UndoSlow();
    }
    ///////////////////////////////////////////

    protected override void CreateCollider()
    {
        if (!isLocalPlayer)
            return;

        m1 = gameObject.AddComponent<PolygonCollider2D>();
        m1.isTrigger = true;
        m1.transform.Translate(Vector3.up * .00001f);
        Vector2[] points = { new Vector2(0.2464974f, 0.5841055f), new Vector2(0.6117219f, 0.2829182f),
        new Vector2(0.4555643f, -0.3497448f), new Vector2(-0.01756102f, -0.6067277f)};
        m1.points = points;
    }
    protected override void DestroyCollider()
    {
        if(!isLocalPlayer)
            return;
        Destroy(m1);
        nAnim.animator.SetBool("M1Held", false);
    }

    protected override void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            player.isAttacking = true;
            
            nAnim.animator.SetBool("M1Held", true);
        }
 


    }

}
