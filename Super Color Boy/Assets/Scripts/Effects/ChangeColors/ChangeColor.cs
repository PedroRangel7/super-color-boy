using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeColor", menuName = "Effect/ChangeColor")]
public class ChangeColor : Effect
{
    [Header("Effect Settings")]     // Unity Inspector Header.
    [SerializeField] private Color newColor;
    private Color previousColor;
    private SpriteRenderer spriteRenderer;

    public override void ApplyStartEffect() {
        spriteRenderer = entity.gameObject.GetComponent<SpriteRenderer>();
        previousColor = spriteRenderer.color;
        spriteRenderer.color = newColor;
    }

    protected override void ApplyEndEffect() {
        spriteRenderer.color = previousColor;
    }
}
