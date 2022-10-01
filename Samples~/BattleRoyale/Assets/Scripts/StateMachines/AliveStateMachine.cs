using HFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AliveStateMachine : StateMachine {
    private AliveParams aliveParams;
    private List<Material> materials;
    private Coroutine damageVFXCoroutine;
    private Coroutine damageAnimationCoroutine;
    private int numFlickers = 3;
    private float flickerDelay = 0.05f;
    private float damageAnimationLength;
    private Color originalColor;

    [Serializable] public class AliveParams {
        [field:SerializeField]public float MaxHealth { get; set; } = 70;
        [field: SerializeField] public float CurrentHealth { get; set; } = 70;
        [field: SerializeField] public AnimationCurve DamagedScaleAnimation {get; set; }
        [field: SerializeField] public Color DamagedColor {get; set; }
        [field: SerializeField] public GameObject Graphics { get; set; }
        public MonoBehaviour MonoBehaviour { get; set; }
        public Transform Transform { get; set; }
        public MeshRenderer [] MeshRenderers { get; set; }
    }

    public AliveStateMachine(AliveParams aliveParams, params StateObject[] stateObjects) : base(stateObjects) {
        this.aliveParams = aliveParams;

        materials = new List<Material>();
        foreach(MeshRenderer renderer in aliveParams.MeshRenderers) {
            materials.AddRange(renderer.materials);
        }
        originalColor = materials[0].color;

        damageAnimationLength = 0;
        if (aliveParams.DamagedScaleAnimation.keys.Length > 0) {
            damageAnimationLength = aliveParams.DamagedScaleAnimation.keys[aliveParams.DamagedScaleAnimation.length - 1].time;
        }
    }

    protected override void OnExit() {
        aliveParams.MonoBehaviour.StopCoroutine(damageVFXCoroutine);
        aliveParams.MonoBehaviour.StopCoroutine(damageAnimationCoroutine);
    }

    public void OnDamaged(float damage) {
        aliveParams.CurrentHealth = Math.Max(0, aliveParams.CurrentHealth - damage);
        if(aliveParams.CurrentHealth > 0) {
            damageVFXCoroutine = aliveParams.MonoBehaviour.StartCoroutine(PlayDamagedVFX());
            damageAnimationCoroutine = aliveParams.MonoBehaviour.StartCoroutine(PlayDamageAnimation());
        }
    }

    private IEnumerator PlayDamageAnimation() {
        float animationElapsedTime = 0;
        while (animationElapsedTime < damageAnimationLength) {
            float yScale = aliveParams.DamagedScaleAnimation.Evaluate(animationElapsedTime);
            aliveParams.Graphics.transform.localScale = new Vector3(1, yScale, 1);
            animationElapsedTime += Time.deltaTime;
            yield return null;
        }
        aliveParams.Graphics.transform.localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator PlayDamagedVFX() {
        for(int i = 0; i < numFlickers; i++) {
            foreach (Material material in materials) {
                material.color = Color.red;
            }
            yield return new WaitForSeconds(flickerDelay);

            foreach (Material material in materials) {
                material.color = originalColor;
            }
            yield return new WaitForSeconds(flickerDelay);
        }
    }

}
