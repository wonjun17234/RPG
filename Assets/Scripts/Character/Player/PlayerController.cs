using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterController
{
    public enum PlayerState{
        None,
        idle,
        jump,
        run,
        fall,
        dead
    }
    Animator _animator;
    [SerializeField]
    private int _speed;
    [SerializeField]
    private int _jumpPower;

    [SerializeField]
    PlayerState _currentState;
    public PlayerState CurrentState
    {
        get 
        { 
            return _currentState; 
        }
        set 
        {
            _currentState = value;
            switch (_currentState)
            {
                case PlayerState.None:
                    break;
                case PlayerState.jump:
                    _animator.Play("jump");
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, _jumpPower));
                    break;
                case PlayerState.idle:
                    _animator.Play("idle");
                    break;
                case PlayerState.run:
                    _animator.Play("run");
                    break;
                case PlayerState.fall:
                    _animator.Play("fall");
                    break;
                case PlayerState.dead:
                    _animator.Play("dead");
                    break;
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        CurrentState = PlayerState.idle;

        GetComponentInChildren<Weapon>().AtkDamage = _characterVariable.AtkDamage;

        _characterVariable.OnDeadAction -= OnHpZero;
        _characterVariable.OnDeadAction += OnHpZero;

        _characterVariable.Maxhp = Managers.Instance.DataManager.MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case PlayerState.None:
                break;
            case PlayerState.jump:
                OnJump();
                break;
            case PlayerState.idle:
                OnIdle();
                break;
            case PlayerState.run:
                OnRun();
                break;
            case PlayerState.fall:
                OnFall();
                break;
            case PlayerState.dead:
                OnDead();
                break;
        }
    }

    public void OnRun()
    {
        DoMove();
        if (Input.GetKey(KeyCode.W))
        {
            CurrentState = PlayerState.jump;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            CurrentState = PlayerState.idle;
        }
        if (GetComponent<Rigidbody2D>().velocity.y < -0.1f)
        {
            CurrentState = PlayerState.fall;
        }
    }
    public void OnIdle()
    {
        DoMove();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            CurrentState = PlayerState.run;
        }
        if (Input.GetKey(KeyCode.W))
        {
            CurrentState = PlayerState.jump;
        }
        if(GetComponent<Rigidbody2D>().velocity.y < -0.1f)
        {
            CurrentState = PlayerState.fall;
        }
    }
    public void OnFall()
    {
        DoMove();
        if (GetComponent<Rigidbody2D>().velocity.y >= 0)
        {
            CurrentState = PlayerState.idle;
        }
    }
    public void OnJump()
    {
        DoMove();
        if (GetComponent<Rigidbody2D>().velocity.y < -0.1f)
        {
            CurrentState = PlayerState.fall;
        }
    }
    void OnDead()
    {

    }
    void OnHpZero()
    {
        CurrentState = PlayerState.dead;
    }

    void DoMove()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + new Vector3(-_speed, 0, 0) * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + new Vector3(_speed, 0, 0) * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.otherCollider.name == "Weapon" && collision.transform.tag == "Enemy")
        {
            CurrentState = PlayerState.jump;
        }
        else if (collision.transform.tag == "Enemy")
        {
            _characterVariable.GetDemage(collision.gameObject.GetComponent<CharacterVariable>().AtkDamage);
        }

        Debug.Log("aaa");
    }
}
