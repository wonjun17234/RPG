using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BunnyController : EnemyController
{
    
    public enum BunnyState{
        None,
        idle,
        jump,
    }
    
    [SerializeField]
    float _jumpPower = 3;
    Animator _animator;
    
    BunnyState _currentState;

    public BunnyState CurrentState{
        get{ return _currentState; }
        set
        {
            _currentState = value;
            switch(_currentState)
            {
                case BunnyState.None:
                    break;
                case BunnyState.idle:
                    _animator.Play("idle");
                    StartCoroutine(WaitForJump());
                    break;
                case BunnyState.jump:
                    _animator.Play("jump");
                    
                    if (_player != null)
                    {
                        FlipCharacter((_player.transform.position - transform.position).normalized.x);
                        GetComponent<Rigidbody2D>().AddForce(
                            new Vector3((_player.transform.position - transform.position).normalized.x * _jumpPower, 1 * _jumpPower, 0)
                            , ForceMode2D.Impulse);
                    }
                    else
                    {
                        int random = Random.Range(-1, 1);
                        FlipCharacter(random);
                        GetComponent<Rigidbody2D>().AddForce(
                            new Vector3(random * _jumpPower, 1 * _jumpPower, 0)
                            , ForceMode2D.Impulse);
                    }
                    
                    break;
            }
        }
    }

    IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(2);
        CurrentState = BunnyState.jump;
    }

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        CurrentState = BunnyState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case BunnyState.idle:
                OnIdle(); 
                break;
            case BunnyState.jump:
                OnJump();
                break;
        }
    }

    void OnIdle()
    {
        DetectPlayer();
    }
    

    void OnJump()
    {
        if(GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            CurrentState = BunnyState.idle;
        }
    }
}
