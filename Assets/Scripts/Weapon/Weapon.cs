using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    private float _atkDamage = 10;
    public float AtkDamage
    {
        get { return _atkDamage;}
        set { _atkDamage = value; }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<CharacterVariable>().GetDemage(_atkDamage);
        }
    }
}
