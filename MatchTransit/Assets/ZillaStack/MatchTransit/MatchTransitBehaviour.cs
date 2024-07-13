using Assets.ZillaStack.MatchTransit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTransitBehaviour : MonoBehaviour
{
    void Start()
    {
        MatchTransitServices.ConfigureServices();
    }
}
