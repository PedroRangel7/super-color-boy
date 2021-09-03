using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeColor", menuName = "Effect/ChangeColor")]    // Enables creating of a new 'ChangeColor' effect on Unity's Project window.
public class ChangeColor : Effect
{
    #region Variables
    [Header("Effect Settings")]     // Unity Inspector Header.
    [SerializeField] private PaletteColor newColor;     // Which color the entity should change to.
    private PaletteColor previousColor;                 // Entity's previous color.
    private ColorBehaviour colorBehaviour;              // Entity's ColorBehaviour reference.
    private ColorPalette colorPalette;                  // GameMaster's ColorPalette reference.
    #endregion

    #region Basic
    private void Awake() {
        colorPalette = GameMaster.instance.colorPalette;        // Set 'colorPallete' reference to the current used ColorPalette.
    }
    #endregion

    #region Apply
    public override void ApplyStartEffect() {
        colorBehaviour = entity.gameObject.GetComponent<ColorBehaviour>();      // Set 'colorBehaviour' reference to the entity's ColorBehaviour.
        previousColor = colorBehaviour.paletteColor;        // Set 'previousColor' to the entity's current color.
        colorBehaviour.ChangeColor(newColor);       // Set entity's color to 'newColor'.
    }

    protected override void ApplyEndEffect() {
        colorBehaviour.ChangeColor(previousColor);      // Set entity's color back to 'previousColor'.
    }
    #endregion
}
