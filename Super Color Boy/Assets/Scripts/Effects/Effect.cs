using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    [Header("Basic Settings")]     // Unity Inspector Header.
    public string title = "New Effect";
    public bool isInstantaneous = false;
    [Header("Tick Settings")]     // Unity Inspector Header.
    public bool isTick = false;
    public int numberOfTicks = 0;
    public bool ticksDecay = true;
    protected float tickTimer;
    [SerializeField] protected float ticksPerSecond = 1f;
    [Header("Continuous Settings")]     // Unity Inspector Header.
    public bool isContinuous = false;
    public float duration = 0f;
    public bool durationDecay = true;
    protected Entity entity;
    protected EffectApplier effectList;
    [HideInInspector] public bool isDone = false;

    public virtual void Setup(GameObject entityGO, EffectApplier effectList) {
        entity = entityGO.GetComponent<Entity>();
        this.effectList = effectList;
        if (!isTick)
            numberOfTicks = 0;
        if (!isContinuous)
            duration = 0f;
    }

    public void CountTime() {
        if (isContinuous && duration > 0f) {
            if (durationDecay)
                duration -= Time.deltaTime;
            ApplyContinuousEffect();
        }

        if (isTick && numberOfTicks > 0) {
            if (tickTimer < 1 / ticksPerSecond)
                tickTimer += Time.deltaTime;
            else {
                ApplyTickEffect();
                if (ticksDecay)
                    numberOfTicks--;
                tickTimer -= 1 / ticksPerSecond;
            }
        }

        if (duration <= 0f && numberOfTicks <= 0)
            isDone = true;

        if (isDone)
            ApplyEndEffect();
    }

    public virtual void ApplyStartEffect() {

    }

    protected virtual void ApplyTickEffect() {

    }

    protected virtual void ApplyContinuousEffect() {

    }

    protected virtual void ApplyEndEffect() {

    }

    public virtual void StackTickEffect(Effect newEffect) {

    }

    public virtual void StackContinuousEffect(Effect newEffect) {

    }
}