using UnityEngine;
using FSM;

public class MovementState : StateBase
{
  private PlayerController player;
  private InputManager inputManager;


    public MovementState(PlayerController player) : base(false) {
        this.player = player;
        inputManager = player.GetComponent<InputManager>();
    }

    public override void OnEnter()
    {
        //Debug.Log($"MovementState: OnEnter()");
        player.canMove = true;
    }

    public override void OnExit()
    {
        //Debug.Log($"MovementState: OnExit()");
        player.canMove = false;
    }

    public override void OnFixedUpdate() {
        if (player.canMove && inputManager.moveInputDirection.magnitude > 0)
        {
            /*Vector3 direction = new Vector3(inputManager.moveInputDirection.x, 0, inputManager.moveInputDirection.y).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref player.turnSmoothVelocity, player.turnTime);
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);*/
            //float y = player.MoveDirection.y;
           
            player.controller.Move(player.MoveDirection * player.speed * Time.deltaTime);
            //rotates the player model to the move direction
            Quaternion rotationToMoveDirection;
            if (player.MoveDirection.magnitude > 0)
            {
                rotationToMoveDirection = Quaternion.LookRotation(player.MoveDirection);
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, rotationToMoveDirection, player.turnSpeed * Time.deltaTime);
            }
        }
    }
}
