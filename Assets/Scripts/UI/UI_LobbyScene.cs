using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Base
{
    private enum ButtonList
    {
        StartButton,
        MaxHpUp,
        MaxHpDown,
    }

    private enum TextList
    {
        CoinCount,
        CurrentMaxHp,
    }

    int _coin;
    float _maxHp;

    void Start()
    {
        _coin = Managers.Instance.DataManager.Coin;
        _maxHp = Managers.Instance.DataManager.MaxHp;
        SaveUIObjectByEnum<Button>(typeof(ButtonList));
        BindFunctionToHandler(GetUIObject<Button>((int)ButtonList.StartButton).gameObject, Defines.UiEventType.PointDown, OnStartButton);

        BindFunctionToHandler(GetUIObject<Button>((int)ButtonList.MaxHpDown).gameObject, Defines.UiEventType.PointDown, OnMaxHpDown);
        BindFunctionToHandler(GetUIObject<Button>((int)ButtonList.MaxHpUp).gameObject, Defines.UiEventType.PointDown, OnMaxHpUp);

        SaveUIObjectByEnum<Text>(typeof(TextList));
        ShowCoinCount();
        ShowMaxHp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ShowCoinCount()
    {
        GetUIObject<Text>((int)TextList.CoinCount).text = "남은 코인 수 : " + _coin.ToString();
    }
    void ShowMaxHp()
    {
        GetUIObject<Text>((int)TextList.CurrentMaxHp).text = "현재 최대 체력 : " + _maxHp.ToString();
    }

    void OnStartButton(PointerEventData eventData)
    {
        SceneManager.LoadScene("CaveScene");
        Managers.Instance.DataManager.Coin = _coin;
        Managers.Instance.DataManager.MaxHp = _maxHp;
    }

    void OnMaxHpUp(PointerEventData eventData)
    {
        if(_coin > 0)
        {
            _coin -= 1;
            _maxHp += 10;
        }
        ShowCoinCount();
        ShowMaxHp();
    }
    void OnMaxHpDown(PointerEventData eventData)
    {
        if(_maxHp > 100)
        {
            _coin += 1;
            _maxHp -= 10;
        }
        ShowCoinCount();
        ShowMaxHp();
    }
}
