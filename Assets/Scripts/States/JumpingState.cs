using UnityEngine;
using FSM;

public class JumpingState : StateBase
{
    private PlayerController player;
    private InputManager inputManager;

    public JumpingState(PlayerController player) : base(false)
    {
        this.player = player;
        inputManager = player.GetComponent<InputManager>();
    }

    public override void OnFixedUpdate()
    {
        player.velocity.y = Mathf.Sqrt(player.jumpHeight * -2 * player.gravity);
        fsm.StateCanExit();
    }
}
