using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIController : MonoBehaviour
{
    private bool isInBattle = false;
    private bool isTriggerOver = false;
    private CanvasGroup group;
    private CanvasGroup childAlertCvsGrp;
    private float turnDuration;
    private bool isTurnOver = false;
    private bool isCoroutineCalled = false;

    [SerializeField] private GameObject childAlert;
    [SerializeField] private TextMeshProUGUI battleTimer;
    [SerializeField] private GameObject globalTurnDuration;
    public void InitBattle()
    {
        isInBattle = true;
    }

    public void TurnOver()
    {
        isTurnOver = true;
    }
    private IEnumerator WaitTurn()
    {
        yield return new WaitUntil(() => isTurnOver);
        // reassign for next turn
        turnDuration = globalTurnDuration.GetComponent<GlobalTurnDuration>().getTurnDuration();
        isTurnOver = false;
        isCoroutineCalled = false;
    }
    private void Start()
    {
        turnDuration = globalTurnDuration.GetComponent<GlobalTurnDuration>().getTurnDuration();
        childAlert.SetActive(false);
        childAlertCvsGrp = childAlert.GetComponent<CanvasGroup>();
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
    }
    private void Update()
    {
        if (isInBattle)
        {
            // battle alert fade in
            gameObject.SetActive(true);
            childAlert.SetActive(true);
            if (group.alpha < 1)
            {
                group.alpha += Time.deltaTime;
                childAlertCvsGrp.alpha += Time.deltaTime;
            } 
            else
            {
                isTriggerOver = true;
            }

            // timer logic
            if (turnDuration > 0)
            {
                turnDuration -= Time.deltaTime;
                int seconds = Mathf.FloorToInt(turnDuration % 60);
                battleTimer.text = string.Format("{0}", seconds + 1);
            }
            else if (turnDuration <= 0)
            {
                battleTimer.text = "0";

                if (!isCoroutineCalled)
                {
                    isCoroutineCalled = true;
                    StartCoroutine(WaitTurn());
                }
            }
        }

        // for battle alert
        if (isTriggerOver)
        {
            if (childAlertCvsGrp.alpha > 0)
            {
                childAlertCvsGrp.alpha -= Time.deltaTime;
            }
            else
            {
                isTriggerOver = false;
                childAlert.SetActive(false);
            }
        }
    }
}
