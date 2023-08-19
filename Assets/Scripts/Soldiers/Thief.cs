using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Soldier
{
   
    
    public override void AttackAnimation()
    {
        base.AttackAnimation();
        
    }
    public override void SubHealth(int amount)
    {
        base.SubHealth(amount);
        
    }
    public override void SetDestination(Transform transforme)
    {
        Transform target = CheckForSafeSpot();
        destinationSetter.target = target.transform;
        target.transform.parent = transforme;
    }
    public Transform CheckForSafeSpot()
    {
        GameObject target = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        List<Soldier> enemies = FindAllEnemySoldiersInRange(attackRange);
        for (int i = 0; i < 10; i++)
        {
            foreach (Soldier enemy in enemies)
            {
                Vector3 dir = target.transform.position - enemy.transform.position;
                target.transform.position += dir.normalized * 2;

            }
        }
        return target.transform;
    }
    protected override void Update()
    {

        if (FindAllEnemySoldiersInRange(attackRange).Count > 0)
        {
            if (attack)
            {
                destinationSetter.target = FindClosestEnemySoldier(true).transform;
            }
            else
            {
                destinationSetter.target = CheckForSafeSpot();
            }
        }
        else if (FindAllEnemySoldiersInRange(detectionRange).Count > 0)
        {
            destinationSetter.target = FindClosestEnemySoldier(false).transform;
        }
        else
        {
            destinationSetter.target = enemyCrystal.transform;
        }
    }
}
