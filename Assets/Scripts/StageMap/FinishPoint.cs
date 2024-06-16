using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.transform.tag == "Player")
        {
            Managers.Instance.DataManager.Coin += 10;
            SceneManager.LoadScene("LobbyScene");
        }
    }
}
