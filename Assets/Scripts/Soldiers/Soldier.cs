using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System.Linq;

public enum Player
{
    player,
    enemy
}
public class Soldier : MonoBehaviour
{
    public int id;
    public AIDestinationSetter destinationSetter;
    public int damage;
    public Player player;
    public int health;
    public int maxHealth;
    public LayerMask soldier;
    public float detectionRange;
    public float attackRange;
    public float attackSpeed;
    protected bool attack = false;

    public int killValue;
    public int cost;

    protected Crystal enemyCrystal = null;
    private void Start()
    {
        enemyCrystal = FindEnemyCrystal();
        SetDestination(enemyCrystal.transform);
        health = maxHealth;
        StartCoroutine(AttackTimer());
    }
    protected virtual void Update()
    {
        Soldier closestSoldier = FindClosestEnemySoldier(false);
        if (closestSoldier != null)
        {
            SetDestination(closestSoldier.transform);
        }
        else
        {
            SetDestination(enemyCrystal.transform);
        }
    }
    public virtual void SubHealth(int amount)
    {
        if (health - amount < 0)
        {
            GiveEnemyMoneyForKill();
            Destroy(gameObject);
        }
        else
        {
            health -= amount;
        }
    }
    public void GiveEnemyMoneyForKill()
    {
        enemyCrystal.AddMoney(killValue);
    }
    public virtual void SetDestination(Transform transform)
    {
        destinationSetter.target = transform;
    }
    public Crystal FindEnemyCrystal()
    {

        Crystal[] crystals = FindObjectsOfType<Crystal>();

        foreach (Crystal c in crystals)
        {
            if (c.player != player)
            {
                return c;
            }
        }
        return null;
    }
    public List<Soldier> FindAllEnemySoldiersInRange(float range)
    {
        Collider2D[] soldiers = Physics2D.OverlapCircleAll(transform.position, range, soldier);
        List<Soldier> enemySoldiers = new List<Soldier>();
        if (soldiers.Length > 0)
        {
            foreach (Collider2D col in soldiers)
            {
                if (col.GetComponent<Soldier>().player != player)
                {
                    enemySoldiers.Add(col.GetComponent<Soldier>());
                }
            }
        }
        return enemySoldiers;
    }
    public Soldier FindClosestEnemySoldier(bool attack)
    {
        List<Soldier> enemySoldiersInRange = new List<Soldier>();
        if (!attack)
        {
            enemySoldiersInRange = FindAllEnemySoldiersInRange(detectionRange);
        }
        if (attack)
        {
            enemySoldiersInRange = FindAllEnemySoldiersInRange(attackRange);
        }
        Soldier cloestestSoldier = null;
        if (enemySoldiersInRange.Count > 0)
        {

            foreach (Soldier soldier in enemySoldiersInRange)
            {
                if (cloestestSoldier == null)
                {
                    cloestestSoldier = soldier;
                    continue;
                }

                if (Vector2.Distance(soldier.transform.position, transform.position) < Vector2.Distance(cloestestSoldier.transform.position, transform.position))
                {
                    cloestestSoldier = soldier;
                }
            }
        }
        return cloestestSoldier;
    }
    public virtual void Attack()
    {
        Soldier closestEnemy = FindClosestEnemySoldier(true);

        if (Vector2.Distance(transform.position, FindEnemyCrystal().transform.position) < attackRange)
        {
            FindEnemyCrystal().health -= damage;
            return;
        }
        if (closestEnemy != null)
        {
            closestEnemy.SubHealth(damage);
            AttackAnimation();
        }
    }
    public virtual void AttackAnimation()
    {

    }
    public IEnumerator AttackTimer()
    {
        while (true)
        {

            attack = true;
            yield return new WaitForSeconds(attackSpeed / 2.5f);
            Attack();
            attack = false;
            yield return new WaitForSeconds(attackSpeed);

        }
    }



}
