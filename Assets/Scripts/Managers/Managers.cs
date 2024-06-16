using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;

    DataManager _dataManager = new DataManager();

    

    public DataManager DataManager
    {
        get 
        {
            return _dataManager; 
        }
    }
    public static Managers Instance
    {
        get
        {
            if(s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if(go == null)
                {
                    go = new GameObject("@Managers");
                }
                s_instance = go.AddComponent<Managers>();
                DontDestroyOnLoad(go);
            }
            return s_instance;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
