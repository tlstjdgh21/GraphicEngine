using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerState
{
    Idle,
    Move

}

public class Player : MonoBehaviour
{

    public float MoveSpeed;
    public float Acceleration;
    public float Deceleration;
    public float VelocityPower;

    public Animator playerAnimator;

    private GameObject player;
    private State<Player>[] state;
    private StateMachine<Player> stateMachine;

    [HideInInspector]
    public Rigidbody2D PlayerRigid { get; private set; }

    [HideInInspector]
    public float MoveDirection;

    private bool _isright;

    private bool _isStop;

    private void Start()
    {

        state = new State<Player>[2];
        state[(int)playerState.Idle] = new PlayerFsm.Idle();
        state[(int)playerState.Move] = new PlayerFsm.Move();


        stateMachine = new StateMachine<Player>();
        stateMachine.SetUp(this, state[(int)playerState.Idle]);
        player = this.gameObject;
        PlayerRigid = player.GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        stateMachine.Excute();
    }

    public void OnTriggerStop()
    {
        playerAnimator.SetBool("isWalk", false);
        PlayerRigid.velocity = Vector3.zero;
        _isStop = true;
    }

    public void ChangeState(playerState newState)
    {
        stateMachine.ChangeState(state[(int)newState]);
    }

    public void PlayerMoving(float direction)
    {
        if (direction != 0)
        {
            MoveDirection = direction;
        }
        float targetSpeed = MoveSpeed * direction;
        float speedDifference = targetSpeed - PlayerRigid.velocity.x;
        float acceleretionRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Acceleration : Deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * acceleretionRate, VelocityPower)
            * Mathf.Sign(speedDifference);

        if (player != null)
            if(!_isStop)
                PlayerRigid.AddForce(movement * Vector2.right);
        
        MoveDirection = MoveDirection > 0 ? 1 : -1;
    }

    public void LookPlayer(float h)
    {
        if (_isStop) return;

        //왼쪽 방향
        if (h < 0)
            _isright = false;
        //오른쪽 방향
        else if (h > 0)
            _isright = true;

        if (_isright) transform.localScale = Vector3.one;
        else transform.localScale = new Vector3(-1, 1, 1);
    }

}

namespace PlayerFsm
{
    public class Idle : State<Player>
    {
        float oldTime;
        public override void Enter(Player player)
        {
            player.playerAnimator.SetBool("isWalk", false);
            Debug.Log("idle");
        }
        public override void Excute(Player enemy)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                enemy.ChangeState(playerState.Move);
            }
        }
        public override void Exit(Player enemy)
        {
        }
    }
    public class Move : State<Player>
    {
        float oldTime;

        public override void Enter(Player player)
        {
            player.playerAnimator.SetBool("isWalk", true);
            Debug.Log("move");
        }

        public override void Excute(Player player)
        {
            float h = Input.GetAxisRaw("Horizontal");
            player.PlayerMoving(h);
            player.LookPlayer(h);
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                player.ChangeState(playerState.Idle);
            }
        }

        public override void Exit(Player enemy)
        {
        }
    }

}