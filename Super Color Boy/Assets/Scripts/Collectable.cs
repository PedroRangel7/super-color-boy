using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]   // Every object with this script (Collectable) also needs a Collider2D component.
public class Collectable : MonoBehaviour
{
    #region Variables
    [SerializeField] private Effect[] effects;                          // Array of effects added to the collector.
    [SerializeField] private float lifeTime = 0f;                       // Lifetime of the collectable. It will be destroyed after this time.
    [SerializeField] private bool destroyOnCollect = true;              // Whether the collectable will be destroyed when collected.
    [SerializeField] private string[] collectorTags = { "Player" };     // Which GameObject tags can collect the collectable.
    #endregion

    #region Basic
    private void Start() {
        if (lifeTime > 0f)                      // If 'lifeTime' is greater than 0...
            Destroy(gameObject, lifeTime);      // ... Set the timer for the collectable to be destroyed.
    }

    private void Collect(Collider2D collision) {
        foreach (Effect effect in effects)                                          // For each Effect in 'effects'...
            collision.gameObject.GetComponent<Entity>().effects.Add(effect);        // ... Add it to the entity's effects list.
        if (destroyOnCollect)           // If the collectable is set to be destroyed when collected...
            Destroy(gameObject);        // ... Destroy it.
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collectorTags.Contains(collision.gameObject.tag))       // If 'collectorTags' array contains a string which corresponds to the tag of the collider (collector)...
            Collect(collision);                                     // ... Make the collectable be collected by 'collider'.
    }
    #endregion
}
