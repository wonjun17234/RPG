using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    [SerializeField]
    CharacterVariable _characterVariable;
    public CharacterVariable CharacterVariable{
        set
        {
            _characterVariable = value;
        }
    }
    enum SliderEnum{
        hpBar,
    }

    
    Slider _slider;

    public Slider Slider
    {
        get { return _slider; }
    }

    void Start()
    {
        
        SaveUIObjectByEnum<Slider>(typeof(SliderEnum));
        _slider = GetUIObject<Slider>((int)SliderEnum.hpBar);
        _slider.maxValue = _characterVariable.Maxhp;

        
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _characterVariable.Hp;
    }
}
