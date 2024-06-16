using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EagleController : EnemyController
{
    [SerializeField]
    private EagleState _currentState;
    public enum EagleState
    {
        None,
        Idle,
        Move,
        DiveAttack,
        Returning,
        Hurt
    }
    Animator _animator;
    float time;

    float _speed;

    bool _isAttackEnd = false;

    [SerializeField]
    int _groundLayer;

   
    Defines.DirType _dirType;
    public EagleState CurrentState
    {
        get{ return _currentState; }
        set
        {
            _currentState = value;
            switch(_currentState)
            {
                case EagleState.None:
                    break;
                case EagleState.Idle:
                    _animator.Play("Idle");
                    StartCoroutine(WaitForMove());
                    break;
                case EagleState.Move:
                    _animator.Play("Move");
                    int random = Random.Range(0, 4);
                    switch (random)
                    {
                        case 0:
                            _dirType = Defines.DirType.Left;
                            break;
                        case 1:
                            _dirType = Defines.DirType.Up;
                            break;
                        case 2:
                            _dirType = Defines.DirType.Down;
                            
                            break;
                        case 3:
                            _dirType = Defines.DirType.Right;
                            break;
                        default:
                            _dirType = Defines.DirType.None;
                            break;
                    }
                    
                    RaycastHit2D hitDown = Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, 5, _groundLayer);
                    RaycastHit2D hitUp = Physics2D.Raycast(transform.position, UnityEngine.Vector2.up, 1, _groundLayer);

                    if (hitDown.collider != null && hitUp.collider == null)
                    {
                        _dirType = Defines.DirType.Up;
                    }


                    if (_dirType == Defines.DirType.Left || _dirType == Defines.DirType.Right)
                    {
                        FlipCharacter(-(int)_dirType);
                    }

                    time = 0;
                    StartCoroutine(WaitForIdle());
                    break;
                case EagleState.DiveAttack:
                    _animator.Play("DiveAttack");
                    StartCoroutine(DoAttack());
                    break;
                case EagleState.Returning:
                    _animator.Play("Returning");
                    break;
                case EagleState.Hurt:
                    _animator.Play("Hurt");
                    break;
            }
        }
    }

    IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(2);
        CurrentState = EagleState.Move;
    }

    IEnumerator WaitForIdle()
    {
        yield return new WaitForSeconds(2);
        
        if(_dirType == Defines.DirType.Up)
        {
            CurrentState = EagleState.Move;
        }
        else
        {
            CurrentState = EagleState.Idle;
        }

    }

    IEnumerator DoAttack()
    {
        Defines.DirType dirTypeToPlayer = _player.transform.position.x - transform.position.x > 0 ? Defines.DirType.Right : Defines.DirType.Left;
        _isAttackEnd = false;

        
        FlipCharacter(-(int)dirTypeToPlayer);
        

        while (!_isAttackEnd)
        {
            transform.position += new Vector3((int)dirTypeToPlayer * Time.deltaTime * 4, -1 * Time.deltaTime * 4, 0);
            
            yield return null;
        }

        CurrentState = EagleState.Move;


    }
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _groundLayer = 1 << LayerMask.NameToLayer("Ground");
        
        CurrentState = EagleState.Idle;
        time = 0;
        _speed = 2;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case EagleState.Idle:
                OnIdle();
                break;
            case EagleState.Move:
                OnMove();
                break;
        }
    }

    void OnIdle()
    {
        DetectPlayer();
        if(_player != null)
        {
            CurrentState = EagleState.DiveAttack;
        }
    }

    void OnMove()
    {
        DoMove(_dirType);
        DetectPlayer();
        
        if (_player != null && _dirType != Defines.DirType.Up)
        {
            CurrentState = EagleState.DiveAttack;
        }
    }

    void DoMove(Defines.DirType dirType)
    {
        UnityEngine.Vector3 vector3;

        switch (dirType)
        {
            case Defines.DirType.Left:
                vector3 = UnityEngine.Vector2.left;
                break;
            case Defines.DirType.Right:
                vector3 = UnityEngine.Vector2.right;
                break;
            case Defines.DirType.Up:
                vector3 = UnityEngine.Vector2.up;
                break;
            case Defines.DirType.Down:
                vector3 = UnityEngine.Vector2.down;
                break;
            default :
                vector3 = UnityEngine.Vector2.zero;
                break;
        }

        transform.position = transform.position + vector3 * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _isAttackEnd = true;
    }
}
