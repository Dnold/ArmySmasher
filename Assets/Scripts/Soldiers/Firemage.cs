using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firemage : Soldier
{
    public GameObject fireBallPrefab;
    public override void SetDestination(Transform transforme)
    {
        Transform target = CheckForSafeSpot();
        destinationSetter.target = target.transform;
        target.transform.parent = transforme;
    }
    protected override void Update()
    {
        if (FindAllEnemySoldiersInRange(attackRange).Count > 0)
        {
            destinationSetter.target = CheckForSafeSpot();
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
    public override void Attack()
    {
        Soldier closestEnemy = FindClosestEnemySoldier(true);

        if (closestEnemy != null)
        {
            ShootFireBall(closestEnemy.transform);
            AttackAnimation();
        }
    }
    void ShootFireBall(Transform position)
    {
        GameObject spawnedFireball = Instantiate(fireBallPrefab, transform.position, Quaternion.identity);
        Vector2 dir = position.position - transform.position;
        spawnedFireball.GetComponent<Projectile>().soldier = this;
        spawnedFireball.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 5);
        Debug.Log(dir);

    }
}
