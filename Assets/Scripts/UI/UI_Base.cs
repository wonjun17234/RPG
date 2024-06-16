using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    Dictionary<Type, List<UnityEngine.Object>> _uiObjectDic = new Dictionary<Type, List<UnityEngine.Object>>(); 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 캔버스 하위에 있는 모든 UI Object를 Dictionary에 저장
    /// </summary>
    /// <typeparam name="T">
    /// 저장할 오브젝트의 타입
    /// </typeparam>
    /// <param name="enumType">
    /// 저장할 UI Object 목록
    /// </param>
    protected void SaveUIObjectByEnum<T>(Type enumType) where T : UnityEngine.Object
    {
        string[] UIObjectNameArr = Enum.GetNames(enumType);
        List<UnityEngine.Object> uiObjectList = new List<UnityEngine.Object>();
        
        List<RectTransform> rectTransformList = new List<RectTransform>();

        for(int i =0; i < UIObjectNameArr.Length; i++)
        {
            rectTransformList = GetComponentsInChildren<RectTransform>().ToList<RectTransform>();
            for(int j =0; j < rectTransformList.Count; j++)
            {
                if(rectTransformList[j].name == UIObjectNameArr[i])
                {
                    uiObjectList.Add(rectTransformList[j].GetComponent<T>());
                }
            }
        }
        
        _uiObjectDic.Add(typeof(T), uiObjectList);
    }
    protected T GetUIObject<T>(int enumNum) where T : UnityEngine.Object
    {
        List<UnityEngine.Object> uiObjectList = new List<UnityEngine.Object>();
        _uiObjectDic.TryGetValue(typeof(T), out uiObjectList);
        return (T) uiObjectList[enumNum];
    }
}
