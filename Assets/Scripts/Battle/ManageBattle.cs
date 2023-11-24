using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBattle : MonoBehaviour
{
    private bool isBattleStarted = false;
    private GameObject alert;
    private float delayedTime = 0;
    public float threshHold = 5.0f;
    private int move = 0;

    public void StartBattle()
    {
        isBattleStarted = true;
        alert.GetComponent<ControlBattleUI>().StartBattle();

    }
    private void Start()
    {
        alert = GameObject.FindGameObjectWithTag("BattleCanvas");
    }

    private void Update()
    {
        if (isBattleStarted)
        {
            delayedTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move = 1; // 1 for basic attack
            } 
            else if (Input.GetMouseButtonDown(0))
            {
                move = 2; // 2 for movement
            }
            if (delayedTime >= threshHold)
            {
                delayedTime = 0;
                if (move == 1)
                {
                    Debug.Log("Attack");
                } 
                else if (move == 2)
                {
                    Debug.Log("Move");
                }
                else
                {
                    Debug.Log("nothing happened...");
                }
            }
        }
    }
}
