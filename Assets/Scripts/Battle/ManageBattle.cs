using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBattle : MonoBehaviour
{
    [SerializeField] private BattleTrigger battleTrigger;
    private bool isBattleStarted = false;
    private GameObject alert;
    private float delayedTime = 0;
    public float TurnDuration = 5.0f;
    private int move = 0;
    private GameObject player;
    [SerializeField] private GameObject enemyPrefab;

    public void StartBattle()
    {
        isBattleStarted = true;
        alert.GetComponent<ControlBattleUI>().StartBattle();
    }
    private void Start()
    {
        alert = GameObject.FindGameObjectWithTag("BattleCanvas");
        player = GameObject.FindGameObjectWithTag("Player");
        battleTrigger.OnPlayerEnterTrigger += BttleTrigger_OnPlayerEnterTrigger;
    }
    private void BttleTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        StartBattle();
        Instantiate(enemyPrefab, new Vector2(Random.Range(-4.0f, -2.0f), Random.Range(1.0f, 2.0f)), Quaternion.identity);
    }

    private void Update()
    {
        if (isBattleStarted)
        {
            player.GetComponent<PlayerInBattle>().inBattle = true;

            delayedTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move = 1; // 1 for basic attack
            } 
            else if (Input.GetMouseButtonDown(0))
            {
                move = 2; // 2 for movement
            }
            if (delayedTime >= TurnDuration)
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
