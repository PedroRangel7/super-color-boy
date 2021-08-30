using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]   // Every object with this script (Collectable) also needs a Collider2D component.
public class Collectable : MonoBehaviour
{
    [SerializeField] private Effect[] effects;
    [SerializeField] private float lifeTime = 0f;
    [SerializeField] private bool destroyOnCollect = true;
    [SerializeField] private string[] collectorTags = { "Player" };

    private void Start() {
        if (lifeTime > 0f)
            Destroy(gameObject, lifeTime);
    }

    private void Collect(Collider2D collision) {
        foreach (Effect effect in effects)
            collision.gameObject.GetComponent<Entity>().effects.Add(effect);
        if (destroyOnCollect)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collectorTags.Contains(collision.gameObject.tag))
            Collect(collision);
    }
}
