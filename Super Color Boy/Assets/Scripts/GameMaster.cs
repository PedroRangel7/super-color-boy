using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    #region Variables
    public static GameMaster instance = null;   // GameMaster unique instance (singleton).
    public ColorPalette colorPalette;           // Current ColorPalette.
    #endregion

    #region Basic
    private void Awake() {
        Setup();        // Setups GameMaster.
    }

    private void Setup() {
        if (instance == null)       // If no GameMaster instance exists...
            instance = this;        // ... Set this instance to the unique instance.
        else                        // ... Else (an instance already exists)...
            Destroy(gameObject);    // ... Delete the Game Object to which this instance is attached.
    }
    #endregion
}
