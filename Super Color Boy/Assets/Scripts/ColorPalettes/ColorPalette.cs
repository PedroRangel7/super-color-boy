using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColorPalette", menuName = "Color Palette")]    // Enables creating of a new 'ColorPalette' on Unity's Project window.
public class ColorPalette : ScriptableObject
{
    #region Variables
    [SerializeField] private Color black = Color.black;
    [SerializeField] private Color white = Color.white;
    [SerializeField] private Color red = Color.red;
    [SerializeField] private Color green = Color.green;
    [SerializeField] private Color blue = Color.blue;
    [SerializeField] private Color yellow = Color.yellow;
    [SerializeField] private Color pink = Color.magenta;
    #endregion

    #region Basic
    public Color GetColor(PaletteColor paletteColor) {      // Returns the color which corresponds to the 'PaletteColor' passed as argument.
        switch (paletteColor) {
            case PaletteColor.Black:
                return black;
                break;
            case PaletteColor.White:
                return white;
                break;
            case PaletteColor.Red:
                return red;
                break;
            case PaletteColor.Green:
                return green;
                break;
            case PaletteColor.Blue:
                return blue;
                break;
            case PaletteColor.Yellow:
                return yellow;
                break;
            case PaletteColor.Pink:
                return pink;
                break;
            default:
                return black;
                break;
        }
    }
    #endregion
}

public enum PaletteColor    // Predefines which colors exist.
{
    Black,
    White,
    Red,
    Green,
    Blue,
    Yellow,
    Pink
};