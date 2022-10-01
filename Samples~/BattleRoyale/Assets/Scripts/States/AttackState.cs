using HFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State {
    public bool IsOnCooldown { get; private set; }
    
    private AttackParams attackParams;
    private float attackElapsedTime;
    private Vector3 initialPosition;
    private float attackAnimationDuration;
    private float rotationSpeed = 10;
    private float moveForwardThreshold = 0.6f;
    private Vector3 dampVelocity;
    private float smoothTime = 0.05f;
    private Coroutine dealDamageCoroutine;
    private Vector3 lastPreyPosition;
    private Vector3 PreyPosition {
        get {
            if (attackParams.Prey == null) { // has prey been destroyed?
                return lastPreyPosition;
            } else {
                lastPreyPosition = attackParams.Prey.transform.position;
                return lastPreyPosition;
            }
        }
    }
     
    [Serializable]
    public class AttackParams {
        [field: SerializeField] public float AttackDamage { get; set; } = 10;
        [field: SerializeField] public AnimationCurve AttackAnimationForwardDirection { get; set; }
        [field: SerializeField] public AnimationCurve AttackAnimationScale { get; set; }
        [field: SerializeField] public float AttackRange { get; set; } = 1;
        [field: SerializeField] public float AttackCooldown { get; set; } = 3;
        public Transform Transform { get; set; }
        public CharacterMovement CharacterMovement { get; set; }
        public MonoBehaviour MonoBehaviour { get; set; }
        public GameObject Prey { get; set; }
    }

    public AttackState(AttackParams attackParams) {
        this.attackParams = attackParams;

        float duration1 = 0, duration2 = 0;
        if (attackParams.AttackAnimationForwardDirection.keys.Length > 0){
            duration1 = attackParams.AttackAnimationForwardDirection.keys[attackParams.AttackAnimationForwardDirection.length - 1].time;
        }
        if (attackParams.AttackAnimationScale.keys.Length > 0) {
            duration2 = attackParams.AttackAnimationScale.keys[attackParams.AttackAnimationScale.length - 1].time;
        }
        attackAnimationDuration = Mathf.Max(duration1, duration2);

        IsOnCooldown = false;
    }

    protected override void OnEnter() {
        attackElapsedTime = 0;
        initialPosition = attackParams.Transform.position;
        attackParams.CharacterMovement.SetDetectCollisions(false);
        attackParams.CharacterMovement.SetEnableOverlapRecovery(false);
        dealDamageCoroutine = attackParams.MonoBehaviour.StartCoroutine(DealDamage());
        OnUpdate();
    }

    protected override void OnExit() {
        attackParams.CharacterMovement.SetDetectCollisions(true);
        attackParams.CharacterMovement.SetEnableOverlapRecovery(true);
        attackParams.MonoBehaviour.StartCoroutine(AttackCooldown());
        attackParams.MonoBehaviour.StopCoroutine(dealDamageCoroutine);
        dealDamageCoroutine = null;
        attackParams.Transform.localScale = new Vector3(1, 1, 1);
    }

    protected override void OnUpdate() {
        AttackAnimation();
    }

    private void AttackAnimation() {
        Transform transform = attackParams.Transform;
        
        // Animate position
        Vector3 preyDirection = PreyPosition - initialPosition;
        preyDirection.y = 0;
        float distanceToPrey = preyDirection.magnitude;
        preyDirection.Normalize();
        float forwardDirectionOffset = attackParams.AttackAnimationForwardDirection.Evaluate(attackElapsedTime);
        if (attackElapsedTime >= moveForwardThreshold) {
            forwardDirectionOffset *= distanceToPrey;
        }
        transform.position = Vector3.SmoothDamp(transform.position, initialPosition + preyDirection * forwardDirectionOffset, ref dampVelocity, smoothTime);

        // Animate rotation
        Quaternion targetRotation = Quaternion.LookRotation(preyDirection, Vector3.up);
        attackParams.Transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                           targetRotation, 
                                                           rotationSpeed * Time.deltaTime);
        // Animate scale
        float zScale = attackParams.AttackAnimationScale.Evaluate(attackElapsedTime);
        attackParams.Transform.localScale = new Vector3(1,1,zScale);

        attackElapsedTime += Time.deltaTime;
    }

    
    public bool HasFinishedAnimation() {
        return attackElapsedTime >= attackAnimationDuration;
    }

    private IEnumerator AttackCooldown() {
        IsOnCooldown = true;
        yield return new WaitForSeconds(attackParams.AttackCooldown);
        IsOnCooldown = false;
    }

    private IEnumerator DealDamage() {
        AnimalController animalController = attackParams.Prey.GetComponent<AnimalController>();
        yield return new WaitForSeconds(moveForwardThreshold);
        animalController.DealDamage(attackParams.AttackDamage);
    }
}

