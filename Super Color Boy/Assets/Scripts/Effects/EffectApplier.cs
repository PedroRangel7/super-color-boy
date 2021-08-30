using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectApplier : List<Effect>
{
    public GameObject entityGO;
    public new void Add(Effect newEffect) {
        Effect existingEffect = Find(e => e.title == newEffect.title);
        newEffect = Object.Instantiate(newEffect);
        newEffect.Setup(entityGO, this);

        if (existingEffect != null) {
            if (existingEffect.isTick || newEffect.isTick)
                existingEffect.StackTickEffect(newEffect);
            if (existingEffect.isContinuous || newEffect.isContinuous)
                existingEffect.StackContinuousEffect(newEffect);
        }
        else if (!newEffect.isInstantaneous)
            base.Add(newEffect);
        newEffect.ApplyStartEffect();
    }

    public void ApplyEffects() {
        if (Count < 1)
            return;

        foreach (Effect effect in this)
            effect.CountTime();

        RemoveAll(e => e.isDone);
    }

    public EffectApplier(GameObject entity) {
        entityGO = entity;
    }
}