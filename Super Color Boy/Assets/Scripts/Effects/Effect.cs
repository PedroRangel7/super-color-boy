using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    #region Variables
    [Header("Basic Settings")]          // Unity Inspector Header.
    public string title = "New Effect";                         // Effect's title.
    public bool isInstantaneous = false;                        // Whether the effect is done instantaneously.
    [Header("Tick Settings")]           // Unity Inspector Header.
    public bool isTick = false;                                 // Whether the effect is done on ticks.
    public int numberOfTicks = 0;                               // Total number of ticks.
    [SerializeField] protected float ticksPerSecond = 1f;       // Amount of ticks every second.
    public bool ticksDecay = true;                              // Whether ticks will decay overtime (if false, the effect will last indefinitely)
    protected float tickTimer;                                  // Current tick timer.
    [Header("Continuous Settings")]     // Unity Inspector Header.
    public bool isContinuous = false;                           // Whether the effect is done continuously.
    public float duration = 0f;                                 // The duration in seconds of the effect.
    public bool durationDecay = true;                           // Whether duration will decay overtime (if false, the effect will last indefinitely)
    protected Entity entity;                                    // Reference to the entity to which the effect applies.
    protected EffectApplier effectList;                         // Reference to the EffectApplier list which contains the effect.
    [HideInInspector] public bool isDone = false;               // Whether the effect is done and ready to be removed from EffectApplier list.
    #endregion

    #region Basic
    public virtual void Setup(GameObject entityGO, EffectApplier effectList) {
        entity = entityGO.GetComponent<Entity>();       // Get 'entity' reference.
        this.effectList = effectList;                   // Set 'effectList' reference.
        if (!isTick)                // If the effect is not done over ticks...
            numberOfTicks = 0;      // ... Set ticks number to 0.
        if (!isContinuous)      // If the effect is not done continuously...
            duration = 0f;      // ... Set duration to 0.
    }

    public void CountTime() {
        if (isContinuous && duration > 0f) {        // If the effect is done continuously and it's duration is greater than 0...
            ApplyContinuousEffect();                // ...Applies continuous effect.
            if (durationDecay)                      // If the duration has decay...
                duration -= Time.deltaTime;         // ... Subtract time since last frame from 'duration'.
        }

        if (isTick && numberOfTicks > 0) {          // If the effect is done over ticks and it's number of ticks is greater than 0...
            if (tickTimer < 1 / ticksPerSecond)     // ... If 'tickTimer' has not reached the seconds per tick value...
                tickTimer += Time.deltaTime;        // ... Add time since last frame to 'tickTimer'.
            else {                                  // ... Else (if the tick timer is enough)...
                ApplyTickEffect();                  // ... Applies tick effect.
                tickTimer -= 1 / ticksPerSecond;    // Substract the seconds per tick value from 'tickTimer'.
                if (ticksDecay)                     // If the ticks have decay...
                    numberOfTicks--;                // ... Subtract 'numberOfTicks' by 1.
            }
        }

        if (duration <= 0f && numberOfTicks <= 0)   // If the effect's duration and number of ticks has reached 0...
            isDone = true;                          // ... Flag the effect as done so it will be removed from the list.

        if (isDone)                 // If the effect is flagged as done...
            ApplyEndEffect();       // ... Apply it's end effect.
    }
    #endregion

    #region Apply
    public virtual void ApplyStartEffect() {
        // Overriden by each effect. Applies when effect is added to the entity.
    }

    protected virtual void ApplyTickEffect() {
        // Overriden by each effect. Applies each tick.
    }

    protected virtual void ApplyContinuousEffect() {
        // Overriden by each effect. Applies each frame.
    }

    protected virtual void ApplyEndEffect() {
        // Overriden by each effect. Applies when effect is done (removed from the entity).
    }
    #endregion

    #region Stack
    public virtual void StackTickEffect(Effect newEffect) {
        // Overriden by each effect. Defines what happens to the ticks system when two of the same effect are added to the same entity.
    }

    public virtual void StackContinuousEffect(Effect newEffect) {
        // Overriden by each effect. Defines what happens to the duration system when two of the same effect are added to the same entity.
    }
    #endregion
}