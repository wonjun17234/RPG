using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
    List<GameObject> _enemys;

    public List<GameObject> Enemys
    {
        get 
        { 
            if(_enemys == null)
            {
                _enemys = new List<GameObject>();
                _enemys = Resources.LoadAll<GameObject>("Prefabs/Enemys").ToList<GameObject>();
            }
            return _enemys; 
        }
    }

    public int Coin
    {
        get { return PlayerPrefs.GetInt("Coin"); }
        set { PlayerPrefs.SetInt("Coin", value); }
    }

    public float MaxHp
    {
        get { return PlayerPrefs.GetFloat("MaxHp", 100f); }
        set { PlayerPrefs.SetFloat("MaxHp", value); }
    }

}
