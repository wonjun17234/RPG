using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariable : MonoBehaviour
{
    public Action OnDeadAction = null;
    [SerializeField]
    private float _hp = 100;
    private float _atkDamage = 10;

    public float AtkDamage{
        get { return _atkDamage;}
    }
    private float _maxhp = 100;
    public float Maxhp{
        get { return _maxhp; }
    }
    
    
    public float Hp
    {
        get { return _hp; }
        set 
        { 
            if(value <= 0)
            {
                _hp = 0;
                if(OnDeadAction != null)
                {
                    OnDeadAction.Invoke();
                }
            }
            else
            {
                _hp = value;
            }
        }
    }
    public void GetDemage(float demage)
    {
        Hp -= demage;
    }


}
