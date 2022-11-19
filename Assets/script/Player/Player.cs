using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private State<Player>[] state;
    private StateMachine<Player> stateMachine;

    private void Start()
    {
        
    }

}

namespace PlayerFsm
{
    public class Idle : State<Player>
    {
        float oldTime;
        public override void Enter(Player enemy)
        {
        }
        public override void Excute(Player enemy)
        {
        }

        public override void Exit(Player enemy)
        {
        }
    }


}