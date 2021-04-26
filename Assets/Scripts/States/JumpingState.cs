using UnityEngine;
using FSM;

public class JumpingState : StateBase
{
    private PlayerController player;
    private InputManager inputManager;

    public JumpingState(PlayerController player) : base(true)
    {
        this.player = player;
        inputManager = player.GetComponent<InputManager>();
    }

    public override void OnEnter()
    {
        player.animator.SetTrigger("Jump");
    }

    public override void OnUpdate()
    {
        player.velocity.y = Mathf.Sqrt(player.jumpHeight * -2 * player.gravity);
        fsm.StateCanExit();
    }
}
