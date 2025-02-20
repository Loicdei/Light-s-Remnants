using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VirtualBeaconButton : MonoBehaviour
{
    public static bool isBeaconPressed = false; // Permet d'être accessible partout

    public void OnButtonPress()
    {
        isBeaconPressed = true;
        StartCoroutine(ResetButton());
    }

    private IEnumerator ResetButton()
    {
        yield return null; yield return null;
        isBeaconPressed = false;
    }
}
