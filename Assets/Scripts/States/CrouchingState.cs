using UnityEngine;
using FSM;

public class CrouchingState : StateBase
{
    private PlayerController player;
    //private InputManager inputManager;

    public CrouchingState(PlayerController player) : base(false)
    {
        this.player = player;
        //inputManager = player.GetComponent<InputManager>();
    }

    public override void OnEnter()
    {
        //Debug.Log($"CrouchingState: OnEnter()");
        player.toggleCrouch = true;
        //player.transform.localScale = new Vector3(player.transform.localScale.x, player.originalScale / 2, player.transform.localScale.z);
    }

    public override void OnExit()
    {
        Debug.Log($"CrouchingState: OnExit()");
        player.toggleCrouch = false;
        //player.transform.localScale = new Vector3(player.transform.localScale.x, player.originalScale, player.transform.localScale.z);
    }

    public override void OnFixedUpdate()
    {
        // var movement = inputManager.moveInputDirection;
        /* if (movement.magnitude > 0) {
          float inputAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
          float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, inputAngle, ref turnSmoothVelocity, player.turnTime);
          player.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
          player.SetMove(Quaternion.Euler(0.0f, inputAngle, 0.0f) * Vector3.forward * player.currentSpeed, axis);
        }*/

        if (player.toggleCrouch)
        {
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.originalScale, player.transform.localScale.z);
        }
        else
        {
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.originalScale, player.transform.localScale.z);
        }
    }
}
