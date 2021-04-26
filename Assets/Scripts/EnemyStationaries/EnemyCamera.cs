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
  private GlobalAlertSystem gas;
  private bool playerDetected = false;
  private AudioSource audioSource;

  void Start() {
    nextLookLocation = 0;
    currentLookLocation = cameraObject.transform.forward;
    if (inverse) {
      currentLookLocation = -currentLookLocation;
    }

    if (lookAtLocations.Length > 0) {
      currentLookLocation = lookAtLocations[0].position;
    }

    player = FindObjectOfType<PlayerController>().gameObject;
    beam = GetComponentInChildren<SpotlightRenderer>();
    gas = FindObjectOfType<GlobalAlertSystem>();
    audioSource = GetComponent<AudioSource>();
  }

  void Update() {
    // Collision detection
    var detectionAngle = Mathf.Atan2(detectionRadius, 1) * Mathf.Rad2Deg;
    var directionToPlayer = player.transform.position - beam.transform.position;
    if (Vector3.Angle(directionToPlayer, currentLookLocation) < detectionAngle && directionToPlayer.magnitude <= beam.MaxDistance) {
      RaycastHit hit;
      if (Physics.Raycast(beam.transform.position, directionToPlayer, out hit, beam.MaxDistance)) {
        if (hit.collider && hit.collider.CompareTag("Player")) {
          if (!playerDetected) {
            gas?.IncreaseAlertLevel();
            audioSource?.Play();
            playerDetected = true;
          }
        } else {
          playerDetected = false;
        }
      } else {
        playerDetected = false;
      }
    } else {
      playerDetected = false;
    }

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
    }
  }
}
