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

    public float moveSpeed;
    public float Acceleration;
    public float Deceleration;
    public float velPower;

    public Animator playerAnimator;

    private GameObject player;
    private State<Player>[] state;
    private StateMachine<Player> stateMachine;
    [HideInInspector]
    public Rigidbody2D playerRigid { get; private set; }
    [HideInInspector]
    public float moveDirection;
    private bool m_isright;


    private void Start()
    {

        state = new State<Player>[2];
        state[(int)playerState.Idle] = new PlayerFsm.Idle();
        state[(int)playerState.Move] = new PlayerFsm.Move();


        stateMachine = new StateMachine<Player>();
        stateMachine.SetUp(this, state[(int)playerState.Idle]);
        player = this.gameObject;
        playerRigid = player.GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        stateMachine.Excute();
        playerLook();
    }

    public void ChangeState(playerState newState)
    {
        stateMachine.ChangeState(state[(int)newState]);
    }

    public void PlayerMoving(float direction)
    {
        if (direction != 0)
        {
            moveDirection = direction;
        }
        float targetSpeed = moveSpeed * direction;
        float speedDifference = targetSpeed - playerRigid.velocity.x;
        float acceleretionRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Acceleration : Deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * acceleretionRate, velPower)
            * Mathf.Sign(speedDifference);

        if (player != null)
            playerRigid.AddForce(movement * Vector2.right);

        moveDirection = moveDirection > 0 ? 1 : -1;
    }
    public void playerLook()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_isright = false;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

            m_isright = true;
        }
        if (m_isright) transform.localScale = Vector3.one;
        else transform.localScale = new Vector3(-1, 1, 1);

    }

    

}

namespace PlayerFsm
{
    public class Idle : State<Player>
    {
        float oldTime;
        public override void Enter(Player enemy)
        {
            enemy.playerAnimator.SetBool("isWalk", false);
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
        public override void Enter(Player enemy)
        {
            enemy.playerAnimator.SetBool("isWalk", true);
            Debug.Log("move");
        }
        public override void Excute(Player enemy)
        {
            float h = Input.GetAxisRaw("Horizontal");
            enemy.PlayerMoving(h);
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                enemy.ChangeState(playerState.Idle);
            }
        }

        public override void Exit(Player enemy)
        {
        }
    }

}