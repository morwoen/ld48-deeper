using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloDoorOpener : MonoBehaviour
{

    public Animation animation;
    void OnTriggerEnter(Collider other)
{
    animation.Play();
    Debug.Log("editing");
}
}
