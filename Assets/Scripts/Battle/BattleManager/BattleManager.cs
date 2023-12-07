using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private bool isBattleStarted = false;
    private float delayedTime = 0;
    private float turnDuration;
    private bool isPlayerTurnOver = false;
    private bool isEnemyTurnOver = false;
    private bool isCoroutineCalled = false;
    private GameObject enemyInstance;

    [SerializeField] private BattleTrigger battleTrigger;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject playerBattleUI;
    [SerializeField] private GameObject globalTurnDuration;
    [SerializeField] private GameObject battleUIController;

    public void InitBattle()
    {
        isBattleStarted = true;
        delayedTime = 0;
        battleUIController.GetComponent<BattleUIController>().InitBattle();
        player.GetComponent<PlayerInBattle>().InitBattle();
        playerBattleUI.GetComponent<PlayerBattleUI>().InitBattle();
        turnDuration = globalTurnDuration.GetComponent<GlobalTurnDuration>().getTurnDuration();
    }

    public void PlayerTurnOver()
    {
        isPlayerTurnOver = true;
    }

    public void EnemyTurnOver()
    {
        isEnemyTurnOver = true;
    }

    public void BattleOver()
    {
        isBattleStarted = false;
        isPlayerTurnOver = false;
        isEnemyTurnOver = false;
        isCoroutineCalled = false;
        StopAllCoroutines();
        battleUIController.GetComponent<BattleUIController>().BattleOver();
        player.GetComponent<PlayerInBattle>().BattleOver();
        playerBattleUI.GetComponent<PlayerBattleUI>().BattleOver();
    }

    private void Start()
    {
        battleTrigger.OnPlayerEnterTrigger += BttleTrigger_OnPlayerEnterTrigger;
    }

    private void BttleTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        InitBattle();
        enemyInstance = Instantiate(enemy, new Vector2(Random.Range(-4.0f, -2.0f), Random.Range(1.0f, 2.0f)), Quaternion.identity);
    }

    private IEnumerator WaitTurn()
    {
        yield return new WaitUntil(() => isPlayerTurnOver && isEnemyTurnOver);
        // reassign for next turn
        isPlayerTurnOver = false;
        isEnemyTurnOver = false;
        isCoroutineCalled = false;
        delayedTime = 0;
        battleUIController.GetComponent<BattleUIController>().TurnOver();
        playerBattleUI.GetComponent<PlayerBattleUI>().TurnOver();
    }

    private void Update()
    {
        if (isBattleStarted)
        {
            delayedTime += Time.deltaTime;

            if (delayedTime >= turnDuration)
            {
                if (!isCoroutineCalled)
                {
                    isCoroutineCalled = true;
                    player.GetComponent<PlayerInBattle>().PlayTurn();
                    enemyInstance.GetComponent<EnemyInBattle>().PlayTurn();
                    playerBattleUI.GetComponent<PlayerBattleUI>().WaitTurn();
                    StartCoroutine(WaitTurn());
                }
            }
        }
    }
}
