using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private GameObject enemy;

    [Header("Movement")]
    [SerializeField]
    private bool inverse = false;
    [SerializeField]
    private float movementSpeed = 1;
    [SerializeField]
    private Transform[] lookAtLocations;
    [SerializeField] public Animator animator;

    private GameObject player;
    private GlobalAlertSystem gas;
    private bool playerDetected = false;
    private Vector3 currentLookLocation;
    private int nextLookLocation;

    [Header("Detection")]
    [SerializeField]
    private float detectionRadius = 0.4f;
    private SpotlightRenderer beam;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        beam = GetComponentInChildren<SpotlightRenderer>();
        gas = FindObjectOfType<GlobalAlertSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var detectionAngle = Mathf.Atan2(detectionRadius, 1) * Mathf.Rad2Deg;
        var directionToPlayer = player.transform.position - beam.transform.position;
        if (Vector3.Angle(directionToPlayer, currentLookLocation) < detectionAngle && directionToPlayer.magnitude <= beam.MaxDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(beam.transform.position, directionToPlayer, out hit, beam.MaxDistance))
            {
                if (hit.collider && hit.collider.CompareTag("Player"))
                {
                    if (!playerDetected)
                    {
                        gas?.IncreaseAlertLevel();
                        playerDetected = true;
                    }
                }
                else
                {
                    playerDetected = false;
                }
            }
            else
            {
                playerDetected = false;
            }
        }
        else
        {
            playerDetected = false;
        }

        if (lookAtLocations.Length > 0)
        {
            var currentTargetLocation = lookAtLocations[nextLookLocation];

            if (Vector3.Distance(currentTargetLocation.position, currentLookLocation) < 0.01)
            {
                nextLookLocation = (nextLookLocation + 1) % lookAtLocations.Length;
            }

            currentLookLocation = Vector3.MoveTowards(currentLookLocation, currentTargetLocation.position, movementSpeed * Time.deltaTime);

            if (inverse)
            {
                enemy.transform.LookAt(2 * enemy.transform.position - currentLookLocation);
            }
            else
            {
                enemy.transform.LookAt(currentLookLocation);
            }
        }
    }
}

