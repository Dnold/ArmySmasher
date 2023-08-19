using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //The time between rounds
    public int waitingTime;
    //Amount of total rounds
    public int roundCounter;

    //UI//
    public TMP_Text roundTimerText;
    public TMP_Text roundCounterText;

    //Gamemanger
    
    private void Start()
    {
        StartCoroutine(RoundCycle());
    }

    IEnumerator RoundCycle()
    {
        bool firstTime = true;
        while (true)
        {
            if (!firstTime)
            {
                for (float i = waitingTime; i > 0; i--)
                {
                    DisplayTime(i);
                    yield return new WaitForSeconds(1);
                }
                StartRound();
            }
            else
            {
                for (float i = 3; i > 0; i--)
                {
                    DisplayTime(i);
                    yield return new WaitForSeconds(1);
                }
                StartRound();
                firstTime = false;
            }
            roundCounter++;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void DisplayTime(float time)
    {
        roundCounterText.text = "Round " + roundCounter;
        roundTimerText.text = time + "";
    }
    public void StartRound()
    {
        Crystal player = GameManager.instance.GetCrystal(Player.player);
        Crystal enemy = GameManager.instance.GetCrystal(Player.enemy);
        GameManager.instance.SpawnSoldier(player);
        GameManager.instance.SpawnSoldier(enemy);
        int passiveIncome = GameManager.instance.passiveIncome;
        player.AddMoney(passiveIncome);
        enemy.AddMoney(passiveIncome);
    }
}
