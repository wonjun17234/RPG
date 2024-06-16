using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    protected CharacterVariable _characterVariable;

    public CharacterVariable CharacterVariable
    {
        get { return _characterVariable; }
        set { _characterVariable = value; }
    }

    protected GameObject _hpBar;
    protected virtual void Start()
    {
        CharacterVariable = GetComponent<CharacterVariable>();
        if(CharacterVariable == null)
        {
            CharacterVariable = gameObject.AddComponent<CharacterVariable>();
        }

        _hpBar = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/SubItem/UI_HpBar"), gameObject.transform);
        _hpBar.GetComponent<UI_HpBar>().CharacterVariable = _characterVariable;
    }
    protected void FlipCharacter(float vectorX)
    {
        if (vectorX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
