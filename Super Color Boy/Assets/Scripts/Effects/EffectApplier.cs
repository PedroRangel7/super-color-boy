using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectApplier : List<Effect>
{
    #region Variables
    public GameObject entityGO;     // Reference to the entity which contains this EffectApplier.
    #endregion

    #region Basic
    public new void Add(Effect newEffect) {     // Overrides 'List<>' Add() function.
        Effect existingEffect = Find(e => e.title == newEffect.title);      // Tries to find any existing effect on the list which corresponds to the one being added.
        newEffect = Object.Instantiate(newEffect);      // Creates a new instance of the effect.
        newEffect.Setup(entityGO, this);        // Setups the new effect, giving reference to the entity and to this list.

        if (existingEffect != null) {                                       // If an identical effect has been found on the list...
            if (existingEffect.isTick || newEffect.isTick)                  // ... If the effect is done over ticks...
                existingEffect.StackTickEffect(newEffect);                  // ... Stack it (specific for each effect).
            if (existingEffect.isContinuous || newEffect.isContinuous)      // ... If the effect is done continuously...
                existingEffect.StackContinuousEffect(newEffect);            // ... Stack it (specific for each effect).
        }
        else if (!newEffect.isInstantaneous)    // If no identical effect has been found on the list and the effect is not instantaneous...
            base.Add(newEffect);                // ... Add the effect to the list.
        newEffect.ApplyStartEffect();           // Apply the start effect.
    }

    public void ApplyEffects() {    // Applies each effect on the list.
        if (Count < 1)      // If no effect is in the list...
            return;         // ... Return (cancel the function).

        foreach (Effect effect in this)     // For each effect in the list...
            effect.CountTime();             // Count time to the effect, applying it's actions.

        RemoveAll(e => e.isDone);       // Remove all effects flagged as done.
    }
    #endregion

    #region Constructors
    public EffectApplier(GameObject entity) {   // Constructor for 'EffectApplier'.
        entityGO = entity;      // Sets entity reference.
    }
    #endregion
}