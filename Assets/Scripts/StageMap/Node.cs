using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelBuilder;

public class Node
{
    private int _x;
    private int _y;

    public int X
    {
        get
        {
            return _x;
        }
    }

    public int Y
    {
        get
        {
            return _y;
        }
    }

    private GameObject _piece;
    public GameObject Piece
    {
        get
        {
            return _piece;
        }
    }

    public void SetPieceInstance(List<GameObject> tm, DirType exitDir, bool isEnd = false)
    {
        _piece = Object.Instantiate<GameObject>(tm[UnityEngine.Random.Range(0, tm.Count)]);
        _piece.AddComponent<LevelPiece>();
        _piece.transform.position = new Vector3(_x * 20, -_y * 20, 0);
        ExitType = exitDir;
        if(EnterType == ExitType)
        {
            Debug.Log("Error");
        }

        if(isEnd)
        {
            GameObject go = new GameObject("FinishPoint");
            go.AddComponent<BoxCollider2D>();
            go.AddComponent<FinishPoint>();
            go.transform.parent = _piece.transform;
            go.transform.localPosition = new Vector3(2.5f, -2.5f, 0);
        }
    }

    private bool _isLooked;

    public bool IsLooked
    {
        get
        {
            return _isLooked;
        }
    }

    public void SetLookedTrue()
    {
        _isLooked = true;
    }

    private DirType enterType;
    public DirType EnterType
    {
        get
        {
            return enterType;
        }
        set
        {
            if (enterType == DirType.None)
            {
                enterType = value;
            }
            else
            {
                Debug.Log("�̹� �ʱ�ȭ��");
                
            }

        }

    }

    private DirType exitType;

    public DirType ExitType
    {
        get
        {
            return exitType;
        }
        set
        {
            if(exitType == DirType.None)
            {
                exitType = value;
            }
            else
            {
                Debug.Log("�̹� �ʱ�ȭ��");
            }
            
        }
    }

    public Node(int x, int y)
    {
        this._x = x;
        this._y = y;
        this._isLooked = false;
        exitType = DirType.None;
        enterType = DirType.None;
    }
    
}
