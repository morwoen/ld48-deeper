using UnityEngine;
using FSM;

public class MovementState : StateBase
{
  private PlayerController player;
  //private InputManager inputManager;

  public MovementState(PlayerController player) : base(false) {
    this.player = player;
    //inputManager = player.GetComponent<InputManager>();
  }

    public override void OnEnter()
    {
        player.canMove = true;
    }

    public override void OnExit()
    {
        player.canMove = false;
    }

    public override void OnFixedUpdate() {
    // var movement = inputManager.moveInputDirection;
    /* if (movement.magnitude > 0) {
      float inputAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, player.turnTime);
      player.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
      player.SetMove(Quaternion.Euler(0.0f, inputAngle, 0.0f) * Vector3.forward * player.currentSpeed, axis);
    }*/
  }
}
