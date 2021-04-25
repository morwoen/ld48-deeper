using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : MonoBehaviour
{
  [SerializeField]
  private GameObject cameraObject;

  [Header("Detection")]
  [SerializeField]
  private float detectionRadius = 0.4f;
  private SpotlightRenderer beam;

  [Header("Movement")]
  [SerializeField]
  private bool inverse = false;
  [SerializeField]
  private float movementSpeed = 1;
  [SerializeField]
  private Transform[] lookAtLocations;

  private Vector3 currentLookLocation;
  private int nextLookLocation;
  private GameObject player;

  void Start() {
    nextLookLocation = 0;
    currentLookLocation = cameraObject.transform.forward;
    if (inverse) {
      currentLookLocation = -currentLookLocation;
    }

    if (lookAtLocations.Length > 0) {
      currentLookLocation = lookAtLocations[0].position;
    }

    player = FindObjectOfType<PlayerMovement>().gameObject;
    beam = GetComponentInChildren<SpotlightRenderer>();
  }

  void Update() {
    if (lookAtLocations.Length > 0) {
      var currentTargetLocation = lookAtLocations[nextLookLocation];

      if (Vector3.Distance(currentTargetLocation.position, currentLookLocation) < 0.01) {
        nextLookLocation = (nextLookLocation + 1) % lookAtLocations.Length;
      }

      currentLookLocation = Vector3.MoveTowards(currentLookLocation, currentTargetLocation.position, movementSpeed * Time.deltaTime);

      if (inverse) {
        cameraObject.transform.LookAt(2 * cameraObject.transform.position - currentLookLocation);
      } else {
        cameraObject.transform.LookAt(currentLookLocation);
      }
    } else {
      currentLookLocation = cameraObject.transform.forward;
      if (inverse) {
        currentLookLocation = -currentLookLocation;
      }
    }

    // Collision detection
    var detectionAngle = Mathf.Atan2(detectionRadius, 1) * Mathf.Rad2Deg;
    var directionToPlayer = player.transform.position - beam.transform.position;
    if (Vector3.Angle(directionToPlayer, currentLookLocation) < detectionAngle && directionToPlayer.magnitude <= beam.MaxDistance) {
      Debug.Log("Player detected");
    }
  }
}
