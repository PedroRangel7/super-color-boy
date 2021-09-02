using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]      // Every object with this script (ColorBehaviour) also needs a SpriteRenderer component.
public class ColorBehaviour : MonoBehaviour
{
    #region Variables
    public PaletteColor paletteColor;           // Current color.
    private SpriteRenderer spriteRenderer;      // SpriteRenderer reference.
    #endregion

    #region Basic
    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();     // Set 'spriteRenderer' reference.
    }

    private void Start() {
        ChangeColor(paletteColor);      // Change color to the predefined one.
    }

    public void ChangeColor(PaletteColor newPaletteColor) {
        paletteColor = newPaletteColor;     // Changes 'paletteColor' to the new color (used to keep track of current color).
        spriteRenderer.color = GameMaster.instance.colorPalette.GetColor(paletteColor);     // Changes 'spriteRenderer' color to the new color.
    }
    #endregion
}
