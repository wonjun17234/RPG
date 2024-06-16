using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : CharacterController
{
    [SerializeField]
    protected GameObject _player;
    [SerializeField]
    protected float _radius = 3;

    [SerializeField]
    protected float _spawnOffSet = 0;

    public float SpawnOffSet{
        get{ return _spawnOffSet; }
    }

    protected void DetectPlayer()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.GetComponent<PlayerController>() != null)
            {
                _player = collider2D.gameObject;
                break;
            }
            else
            {
                _player = null;
            }
        }
    }

    
}
