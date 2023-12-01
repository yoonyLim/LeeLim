using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleTrigger battleTrigger;
    private bool isBattleStarted = false;
    private GameObject alert;
    private float delayedTime = 0;
    public float TurnDuration = 5.0f;
    private int move = 0;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject arrowUI;

    public void InitBattle()
    {
        isBattleStarted = true;
        alert.GetComponent<BattleUIController>().InitBattle();
        arrowUI.GetComponent<ArrowUIInBattle>().InitBattle();
    }

    private void Start()
    {
        alert = GameObject.FindGameObjectWithTag("BattleCanvas");
        battleTrigger.OnPlayerEnterTrigger += BttleTrigger_OnPlayerEnterTrigger;
    }

    private void BttleTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        InitBattle();
        Instantiate(enemyPrefab, new Vector2(Random.Range(-4.0f, -2.0f), Random.Range(1.0f, 2.0f)), Quaternion.identity);
    }

    private void Update()
    {
        if (isBattleStarted)
        {
            player.GetComponent<PlayerInBattle>().InitBattle();

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
