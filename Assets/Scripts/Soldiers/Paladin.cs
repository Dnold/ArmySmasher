using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : Soldier
{
   
    public ParticleSystem ps;
    public override void AttackAnimation()
    {
        base.AttackAnimation();
       
    }
    public override void SubHealth(int amount)
    {
        base.SubHealth(amount);
        
    }
}
