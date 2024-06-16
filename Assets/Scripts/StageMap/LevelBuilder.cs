using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelBuilder;

/// <summary> 
/// 이 클래스는 기존에 만들어져 있던 레벨 조각들로 레벨을 생성한다.
/// </summary>
public class LevelBuilder : MonoBehaviour
{
    string levelPieceFolderPath = "Prefabs/LevelPieces/";
    [SerializeField]
    int _mapSize = 7;
    Node[,] nodes;
    Node currentNode;

    ///<summary>
    /// 방향을 의미하는 열거형 타입이다.
    ///</summary>
    public enum DirType
    {
        None = 0,
        Left = -1,
        Right = 1,
        Down = -2,
        Up = 2
    }
    struct DirTuple
    {
        public DirType enterDir;
        public DirType exitDir;

        public DirTuple(DirType enter, DirType exit)
        {
            if(enter == exit)
            {
                Debug.Log("enter == exit");
            }
            enterDir = enter;
            exitDir = exit;
        }
    }


    Dictionary<DirTuple, List<GameObject>> _pieceDictionary;
    Dictionary<DirType, List<GameObject>> _startDictionary;
    Dictionary<DirType, List<GameObject>> _endDictionary;
    

    void Start()
    {
        nodes = new Node[_mapSize, _mapSize];
        for(int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                nodes[i,j] = new Node(i,j);
            }
        }

        InitStartDictionary();
        InitEndDictionary();
        InitPieceDictionary();

        currentNode = nodes[0, 0];

        List<GameObject> tmp;
        if(_startDictionary.TryGetValue(DirType.Right, out tmp))
        {
            nodes[0, 0].SetPieceInstance(tmp, DirType.Right);
        }
        nodes[0, 0].EnterType = DirType.Up;

        
        while (true)
        {
            currentNode = GetNewCurrentNode();

            List<DirTuple> possiblePieceTypes = AddAllPieceType();

            if (possiblePieceTypes.Count == 0)
            {
                List<GameObject> list;
                _endDictionary.TryGetValue(currentNode.EnterType, out list);
                currentNode.SetPieceInstance(list, DirType.Down);

                break;
            }
            else
            {
                DirTuple newPieceType;
                newPieceType = possiblePieceTypes[UnityEngine.Random.Range(0, possiblePieceTypes.Count)];
                List<GameObject> tm = new List<GameObject>();
                if(_pieceDictionary.TryGetValue(newPieceType, out tm))
                {
                    currentNode.SetPieceInstance(tm, newPieceType.exitDir);

                }
            }
        }
        List<GameObject> allPieceList = new List<GameObject>();
        foreach (List<GameObject> list in _pieceDictionary.Values)
        {
            allPieceList.AddRange(list);
        }
        for (int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                if(nodes[i,j].Piece == null)
                {
                    nodes[i, j].SetPieceInstance(allPieceList, DirType.Down);
                }
            }
        }
    }
    ///<summary>
    /// 시작 딕셔너리를 초기화 하는 함수이다.
    ///</summary>
    void InitStartDictionary()
    {
        _startDictionary = new Dictionary<DirType, List<GameObject>>();
        _startDictionary.Add(DirType.Down, Resources.LoadAll<GameObject>(levelPieceFolderPath + "Starting/" + DirType.Down.ToString()).ToList<GameObject>());
        _startDictionary.Add(DirType.Right, Resources.LoadAll<GameObject>(levelPieceFolderPath + "Starting/" + DirType.Right.ToString()).ToList<GameObject>());
        _startDictionary.Add(DirType.Left, Resources.LoadAll<GameObject>(levelPieceFolderPath + "Starting/" + DirType.Left.ToString()).ToList<GameObject>());

    }

    ///<summary>
    /// 들어오는 방향과 나가는 방향의 조각을 초기화 하는 함수이다.
    ///</summary>
    void InitPieceDictionary()
    {
        _pieceDictionary = new Dictionary<DirTuple, List<GameObject>>();

        _pieceDictionary.Add(new DirTuple(DirType.Left, DirType.Down), Resources.LoadAll<GameObject>(levelPieceFolderPath + "LD").ToList<GameObject>());
        _pieceDictionary.Add(new DirTuple(DirType.Left, DirType.Right), Resources.LoadAll<GameObject>(levelPieceFolderPath + "LR").ToList<GameObject>());
        _pieceDictionary.Add(new DirTuple(DirType.Right, DirType.Left), Resources.LoadAll<GameObject>(levelPieceFolderPath + "LR").ToList<GameObject>());
        _pieceDictionary.Add(new DirTuple(DirType.Right, DirType.Down), Resources.LoadAll<GameObject>(levelPieceFolderPath + "RD").ToList<GameObject>());
        _pieceDictionary.Add(new DirTuple(DirType.Up, DirType.Down), Resources.LoadAll<GameObject>(levelPieceFolderPath + "UD").ToList<GameObject>());
        _pieceDictionary.Add(new DirTuple(DirType.Up, DirType.Left), Resources.LoadAll<GameObject>(levelPieceFolderPath + "UL").ToList<GameObject>());
        _pieceDictionary.Add(new DirTuple(DirType.Up, DirType.Right), Resources.LoadAll<GameObject>(levelPieceFolderPath + "UR").ToList<GameObject>());

    }
    ///<summary>
    /// 끝 딕셔너리를 초기화 하는 함수이다.
    ///</summary>
    void InitEndDictionary()
    {
        _endDictionary = new Dictionary<DirType, List<GameObject>>();
        _endDictionary.Add(DirType.Up, Resources.LoadAll<GameObject>(levelPieceFolderPath + "Ending/" + DirType.Up.ToString()).ToList<GameObject>());
        _endDictionary.Add(DirType.Left, Resources.LoadAll<GameObject>(levelPieceFolderPath + "Ending/" + DirType.Left.ToString()).ToList<GameObject>());
        _endDictionary.Add(DirType.Right, Resources.LoadAll<GameObject>(levelPieceFolderPath + "Ending/" + DirType.Right.ToString()).ToList<GameObject>());

    }

    ///<summary>
    /// 새로운 currentNode를 반환한다.
    ///</summary>

    /// <returns> 
    /// 새로운 currentNode
    ///</returns>
    Node GetNewCurrentNode()
    {
        currentNode.SetLookedTrue();
        switch (currentNode.ExitType)
        {
            case DirType.Left:
                if(!CheckIsPossible(currentNode.X - 1, currentNode.Y))
                {
                    Debug.Log("Error");
                    return null;
                }
                nodes[currentNode.X - 1, currentNode.Y].EnterType = DirType.Right;
                return nodes[currentNode.X - 1, currentNode.Y];

            case DirType.Right:
                if (!CheckIsPossible(currentNode.X + 1, currentNode.Y))
                {
                    Debug.Log("Error");
                    return null;
                }
                nodes[currentNode.X + 1, currentNode.Y].EnterType = DirType.Left;
                return nodes[currentNode.X + 1, currentNode.Y];

            case DirType.Down:
                if (!CheckIsPossible(currentNode.X, currentNode.Y + 1))
                {
                    Debug.Log("Error");
                    return null;
                }
                nodes[currentNode.X, currentNode.Y + 1].EnterType = DirType.Up;
                return nodes[currentNode.X, currentNode.Y + 1];
        }

        Debug.Log("Error");
        return null;
    }

    ///<summary>
    /// 특정 타일이 이동 가능한지 확인하는 함수이다.
    ///</summary>

    /// <returns> 
    /// 이동 가능한지 여부
    ///</returns>

    /// <param name="x">
    /// 확인할 타일의 x좌표
    ///</param>
    /// <param name="y">
    /// 확인할 타일의 y좌표
    ///</param>
    bool CheckIsPossible(int x, int y)
    {
        if(x < _mapSize && y < _mapSize && x >= 0 && y >= 0)
        {
            if(!nodes[x, y].IsLooked)
            {
                return true;
            }
        }
        return false;
    }


    ///<summary>
    /// 특정 타일이 이동 가능한지 확인하는 함수이다.
    ///</summary>

    /// <returns> 
    /// 이동 가능한지 여부
    ///</returns>

    /// <param name="node">
    /// 원점
    ///</param>
    /// <param name="dirType">
    /// 이동할 방향
    ///</param>
    bool CheckIsPossible(Node node ,DirType dirType)
    {
        switch (dirType)
        {
            case DirType.Right:
                return CheckIsPossible(node.X + 1, node.Y);
            case DirType.Left:
                return CheckIsPossible(node.X - 1, node.Y);
            case DirType.Down:
                return CheckIsPossible(node.X, node.Y + 1);
            default:
                Debug.Log("case ����");
                return false;
        }

        
    }

    ///<summary>
    /// 가능한 dirType을 list에 저장한다
    ///</summary>
    /// <param name="list">
    /// 가능한 dirType을 저장한 컨테이너
    ///</param>
    /// <param name="dirType">
    /// 이동할 방향
    ///</param>

    void AddPieceType(List<DirTuple> list, DirType dirType)
    {
        if (CheckIsPossible(currentNode, dirType))
        {
            switch (currentNode.EnterType)
            {
                case DirType.Right:
                    list.Add(new DirTuple(DirType.Right, dirType));
                    break;
                case DirType.Left:
                    list.Add(new DirTuple(DirType.Left, dirType));
                    break;
                case DirType.Up:
                    list.Add(new DirTuple(DirType.Up, dirType));
                    break;
                default:
                    Debug.Log("case ����");
                    break;
            }
        }
    }
    ///<summary>
    /// 특정 타일이 이동 가능한 타일들을 반환한다.
    ///</summary>

    /// <returns> 
    /// 이동 가능한 타일 타입
    ///</returns>


    List<DirTuple> AddAllPieceType()
    {
        List<DirTuple> possiblePieceTypes = new List<DirTuple>();

        AddPieceType(possiblePieceTypes, DirType.Left);
        AddPieceType(possiblePieceTypes, DirType.Right);
        AddPieceType(possiblePieceTypes, DirType.Down);

        return possiblePieceTypes;
    }
}