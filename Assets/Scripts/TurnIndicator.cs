using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TurnIndicator : MonoBehaviour
{
    public GameObject glow;

    public void Show(bool show)
    {
        glow.SetActive(show);
    }
}
