﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior class Q ability
public class WarriorQAbility : Ability {

    public GameObject shieldGO;
    public float stunTime = 2f;

    void Start ()
    {
        anim = GetComponent<Animator>();
        this.slowDown = 50;
        this.coolDown = 0;
        this.damage = 20;
        this.onCoolDown = false;
        player = GetComponent<Player>();
    }
	
	void Update()
    {
        if (!isLocalPlayer)
            return;
        GetInput();
    }

    protected override void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !onCoolDown)
        {
            StartCoroutine("CoolDown");
            CmdSendAnimationParameter(true);
        }

    }

    IEnumerator CoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
    }

    protected void WarriorQSlowDown()
    {
        this.SlowDown();
    }

    protected void WarriorQUndoSlow()
    {
        this.UndoSlow();
    }

    void SendShield()
    {
        CmdSendAnimationParameter(false);
        if (!isServer)
            return;
        Vector3 position = this.transform.position + (transform.right * .6f);
        Quaternion rotationAmount = Quaternion.Euler(0, 0, -90);
        Quaternion rotation = transform.rotation * rotationAmount;
        GameObject shield = Instantiate(shieldGO, position, rotation);

        NetworkServer.Spawn(shield);
        shield.GetComponent<ShieldToss>().damage = damage;
        shield.GetComponent<ShieldToss>().stunTime = stunTime;
    }


    [Command]
    void CmdSendAnimationParameter(bool state)
    {
        RpcRecieveAnimationParameter(state);
    }

    [ClientRpc]
    void RpcRecieveAnimationParameter(bool state)
    {
        if (state)
            anim.SetBool("QPressed", true);
        else
            anim.SetBool("QPressed", false);
    }
}
