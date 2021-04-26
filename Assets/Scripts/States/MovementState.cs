using UnityEngine;
using FSM;

public class MovementState : StateBase
{
  private PlayerController player;
  private InputManager inputManager;
  private Transform cameraTransform;

    public MovementState(PlayerController player) : base(false) {
        this.player = player;
        inputManager = player.GetComponent<InputManager>();
        cameraTransform = Camera.main.transform;
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
        if (player.canMove && inputManager.movementInput.magnitude > 0)
        {
            player.MoveDirection.x = inputManager.movementInput.x * player.speed;
            player.MoveDirection.z = inputManager.movementInput.y * player.speed;
            if (player.MoveDirection.magnitude > 0)
            {
                SetMoveDirectionRelativeToCamera();
                SetRotationBasedOnMoveDirection();
            }
        }
    }

    private void SetMoveDirectionRelativeToCamera()
    {
        Vector3 projectedCameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(projectedCameraForward, Vector3.up);
        player.MoveDirection = rotationToCamera * player.MoveDirection;
    }

    private void SetRotationBasedOnMoveDirection()
    {
        //float targetAngle = Mathf.Atan2(player.MoveDirection.x, player.MoveDirection.z) * Mathf.Rad2Deg;
        //player.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        // smooth turning based on the player's turn speed
        Quaternion rotationToMoveDirection = Quaternion.LookRotation(player.MoveDirection);
        rotationToMoveDirection.x = 0; rotationToMoveDirection.z = 0;   // this shouldn't be necessary once we have the player models in(?)
        player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, rotationToMoveDirection, player.turnSpeed * Time.deltaTime);

    }
}
