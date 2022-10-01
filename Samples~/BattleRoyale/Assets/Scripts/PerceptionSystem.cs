using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionSystem : MonoBehaviour  {
    [SerializeField] private float fieldOfView = 45;
    [SerializeField] private float lengthOfView = 20;
    [SerializeField] private LayerMask animalsLayerMask;
    public Action<GameObject> AnimalDetected { get; set; }

    public void Update() {
        DetectAnimals();
    }

    public void DetectAnimals() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, lengthOfView, animalsLayerMask);
        foreach(Collider collider in colliders) {
            if(collider.gameObject != this.gameObject) {
                Vector3 preyDirection = (collider.transform.position - transform.position).normalized;
                if(Vector3.Angle(transform.forward, preyDirection) < fieldOfView) {
                    GameObject detectedAnimal = collider.gameObject;
                    AnimalDetected.Invoke(detectedAnimal);
                }
            }
        }
    }

}

