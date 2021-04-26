using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
  [SerializeField]
  private Vector3 currentCheckpointPosition;
  [SerializeField]
  private Quaternion currentCheckpointRotation;

  void SaveCheckpoint(Vector3 position, Quaternion rotation) {
    currentCheckpointPosition = position;
    currentCheckpointRotation = rotation;
  }

  public void LoadCheckpoint() {
    // The CharacterController must be disabled when changing the position, otherwise nothing happens
    var cc = GetComponent<CharacterController>();
    cc.enabled = false;
    transform.SetPositionAndRotation(currentCheckpointPosition, currentCheckpointRotation);
    cc.enabled = true;

    GetComponent<HealthController>()?.Respawn();
  }

  private void OnEnable() {
    SaveCheckpoint(transform.position, transform.rotation);
  }

  private void OnTriggerEnter(Collider other) {
    string collisionTag = other.gameObject.tag;

    switch (collisionTag) {
      case "Checkpoint":
        var spawnPoint = other.transform.GetChild(0).transform;
        SaveCheckpoint(spawnPoint.position, spawnPoint.rotation);
        break;
      case "Deadzone":
        LoadCheckpoint();
        break;
      default:
        return;
    }
  }
}
