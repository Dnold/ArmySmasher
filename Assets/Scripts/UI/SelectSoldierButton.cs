using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectSoldierButton : MonoBehaviour
{
    public int id;
    
    public void SelectSoldier()                       
    {
        GameManager.instance.LoadSoldierIntoQueue(id, Player.player) ;
    }
}
