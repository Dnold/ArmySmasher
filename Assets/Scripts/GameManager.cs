using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;

public class SoldierQueueItem
{
    public int id;
    public GameObject soldier;
}
public class GameManager : MonoBehaviour
{
    public Crystal PlayerCrystal;
    public Crystal EnemyCrystal;

    public GameObject[] soldiers;
    int currentSoldierIndex = 0;

    public int startMoney = 100;
    public float enemyLoadTime = 2f;
    public int selectedSoldierID = 0;

    public int passiveIncome;
    public static GameManager instance;

    public TMP_Text moneyText;

    public Color playerColor;
    public Color enemyColor;

    Queue<GameObject> playerSoldierQueue = new Queue<GameObject>();
    Queue<GameObject> enemySoldierQueue = new Queue<GameObject>();

    List<GameObject> soldierQueueItems = new List<GameObject>();

    public GameObject loadedSoldierUI;
    public GameObject queueUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    public void Start()
    {
        PlayerCrystal.money = startMoney;
        EnemyCrystal.money = startMoney;
        moneyText.text = startMoney + " Gold";

        StartCoroutine(EnemyQueueLoader());

    }
    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("SampleScene");
        }
        moneyText.text = PlayerCrystal.money + " Gold";
    }
    public Crystal GetCrystal(Player player)
    {
        if (player == Player.player)
        {
            return PlayerCrystal;
        }
        else
        {
            return EnemyCrystal;
        }
    }

    public void LoadSoldierIntoQueue(int id, Player player)
    {
        GameObject soldierObj = soldiers[id];
        Soldier soldier = soldierObj.GetComponent<Soldier>();

        if (player == Player.player)
        {
            if (soldier.cost + ComputeEntireQueueCost(Player.player) <= PlayerCrystal.money)
            {
                soldier.GetComponent<Soldier>().id = playerSoldierQueue.Count + 1;
                playerSoldierQueue.Enqueue(soldierObj);
                UpdateLoadingSoldierUI();
            }
        }
        else
        {
            if (soldier.cost + ComputeEntireQueueCost(Player.enemy) <= EnemyCrystal.money)
            {
                soldier.GetComponent<Soldier>().id = enemySoldierQueue.Count + 1;
                enemySoldierQueue.Enqueue(soldierObj);
            }
        }
    }
    IEnumerator EnemyQueueLoader()
    {
        while (true)
        {
            int index = ChooseRandomSoldierIndex();
            LoadSoldierIntoQueue(index, Player.enemy);
            yield return new WaitForSeconds(enemyLoadTime);
        }
    }
    void UpdateLoadingSoldierUI()
    {
        foreach (GameObject preQueueItem in soldierQueueItems)
        {
            Destroy(preQueueItem.gameObject);
        }
        soldierQueueItems.Clear();
        foreach (GameObject a in playerSoldierQueue)
        {
            GameObject ui = Instantiate(loadedSoldierUI, queueUI.transform);
            ui.GetComponent<Image>().sprite = a.GetComponent<SpriteRenderer>().sprite;
            soldierQueueItems.Add(ui);
        }
    }

    public int ChooseRandomSoldierIndex()
    {
        return UnityEngine.Random.Range(0, soldiers.Length);
    }
    public int ComputeEntireQueueCost(Player player)
    {
        int n = 0;
        if (player == Player.player)
        {
            foreach (GameObject queueItem in playerSoldierQueue)
            {
                n += queueItem.GetComponent<Soldier>().cost;
            }
        }
        else
        {
            foreach (GameObject queueItem in enemySoldierQueue)
            {
                n += queueItem.GetComponent<Soldier>().cost;
            }
        }
        return n;

    }
    public void SpawnSoldier(Crystal crystal)
    {
        GameObject soldierPrefab = null;
        if (crystal.player == Player.player && playerSoldierQueue.Count > 0)
        {
            soldierPrefab = playerSoldierQueue.Dequeue();
            UpdateLoadingSoldierUI();
        }
        else if (crystal.player == Player.enemy && enemySoldierQueue.Count > 0)
        {
            soldierPrefab = enemySoldierQueue.Dequeue();
        }
        if (soldierPrefab != null)
        {
            Soldier spawnedSoldier = Instantiate(soldierPrefab, crystal.transform.position + GetRandomSpawnLocationOffset(), Quaternion.identity).GetComponent<Soldier>();
            spawnedSoldier.id = currentSoldierIndex;
            currentSoldierIndex++;
            spawnedSoldier.player = crystal.player;
            if (crystal.player == Player.player)
            {
                spawnedSoldier.GetComponent<SpriteRenderer>().color = playerColor;
            }
            else
            {
                spawnedSoldier.GetComponent<SpriteRenderer>().color = enemyColor;
            }

            crystal.money -= spawnedSoldier.cost;
            if (crystal.money < 0)
            {
                crystal.money = 0;
            }
        }
    }
    public Vector3 GetRandomSpawnLocationOffset()
    {
        Vector3 offset = new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-7, 7), 0);
        return offset;
    }
    public void AddMoneyToCrystal(int amount, Player player)
    {
        if (player == Player.player)
        {
            PlayerCrystal.AddMoney(amount);

        }
        if (player == Player.enemy)
        {
            EnemyCrystal.AddMoney(amount);
        }
    }
}
