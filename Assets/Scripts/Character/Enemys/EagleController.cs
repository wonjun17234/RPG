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
                    _dirType = (Defines.DirType)(Random.value > 0.5f ? 1 : -1);
                    FlipCharacter((int)_dirType);
                    time = 0;
                    StartCoroutine(WaitForIdle());
                    break;
                case EagleState.DiveAttack:
                    _animator.Play("DiveAttack");
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
        CurrentState = EagleState.Idle;
    }
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _groundLayer = 1 << LayerMask.NameToLayer("Ground");
        
        CurrentState = EagleState.Idle;
        time = 0;
        _speed = 1;
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
    }

    void OnMove()
    {
        SetY();
        SetX();

    }

    void SetY()
    {
        time += Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100, _groundLayer);
        if(hit.collider != null)
        {
            Vector3 vector3 = transform.position;
            vector3.y = Mathf.Lerp(transform.position.y, hit.transform.position.y + 4f, time / (transform.position.y - hit.transform.position.y));
            transform.position = vector3;
        }
        else
        {
            Debug.Log("hit null");
        }
        
    }
    void SetX()
    {
        if(_player != null)
        {
            if ((_player.transform.position - transform.position).x > 0)
            {
                transform.position =  transform.position + new Vector3(_speed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position = transform.position + new Vector3(-_speed * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            transform.position = transform.position + new Vector3(-_speed * Time.deltaTime * (int)_dirType, 0, 0);
        }
    }
}
