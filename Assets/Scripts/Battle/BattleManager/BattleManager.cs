using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // bool to check if each object is inited for battle
    private bool isPlayerInited = false;

    private bool isBattleStarted = false;
    private GameObject alert;
    private float delayedTime = 0;
    private float turnDuration;
    private bool isTurnOver = false;
    private bool isCoroutineCalled = false;

    [SerializeField] private BattleTrigger battleTrigger;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject arrowUI;
    [SerializeField] private GameObject globalTurnDuration;
    [SerializeField] private GameObject battleUIController;

    public void InitBattle()
    {
        isBattleStarted = true;
        alert.GetComponent<BattleUIController>().InitBattle();
        arrowUI.GetComponent<ArrowUIInBattle>().InitBattle();
    }

    public void TurnOver()
    {
        isTurnOver = true;
        battleUIController.GetComponent<BattleUIController>().TurnOver();
    }

    private void Start()
    {
        alert = GameObject.FindGameObjectWithTag("BattleCanvas");
        battleTrigger.OnPlayerEnterTrigger += BttleTrigger_OnPlayerEnterTrigger;
        turnDuration = globalTurnDuration.GetComponent<GlobalTurnDuration>().getTurnDuration();
    }

    private void BttleTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        InitBattle();
        Instantiate(enemyPrefab, new Vector2(Random.Range(-4.0f, -2.0f), Random.Range(1.0f, 2.0f)), Quaternion.identity);
    }

    private IEnumerator WaitTurn()
    {
        yield return new WaitUntil(() => isTurnOver);
        // reassign for next turn
        isTurnOver = false;
        isCoroutineCalled = false;
        delayedTime = 0;
        arrowUI.GetComponent<ArrowUIInBattle>().TurnOver();
    }

    private void Update()
    {
        if (isBattleStarted)
        {
            if (!isPlayerInited)
            {
                player.GetComponent<PlayerInBattle>().InitBattle();
                isPlayerInited = true;
            }

            delayedTime += Time.deltaTime;

            if (delayedTime >= turnDuration)
            {
                if (!isCoroutineCalled)
                {
                    player.GetComponent<PlayerInBattle>().TakeTrun();
                    arrowUI.GetComponent<ArrowUIInBattle>().WaitTurn();
                    isCoroutineCalled = true;
                    StartCoroutine(WaitTurn());
                }
            }
        }
    }
}
