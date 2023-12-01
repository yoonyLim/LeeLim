using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIController : MonoBehaviour
{
    private bool isBattleStarted = false;
    private bool isTriggerOver = false;
    private CanvasGroup group;
    private GameObject childAlert;
    private CanvasGroup childAlertCvsGrp;
    public void InitBattle()
    {
        isBattleStarted = true;
    }
    private IEnumerator LingerBattleAlert()
    {
        yield return new WaitForSeconds(2);
        isBattleStarted = false;
        isTriggerOver = true;
    }
    private void Awake()
    {
        childAlert = gameObject.transform.GetChild(0).gameObject;
        childAlertCvsGrp = childAlert.GetComponent<CanvasGroup>();
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
    }
    private void Update()
    {
        if (isBattleStarted)
        {
            gameObject.SetActive(true);
            childAlert.SetActive(true);
            if (group.alpha < 1)
            {
                group.alpha += Time.deltaTime;
                childAlertCvsGrp.alpha += Time.deltaTime;
            } 
            else
            {
                StartCoroutine(LingerBattleAlert());
            }
        }
        else if (isTriggerOver)
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
