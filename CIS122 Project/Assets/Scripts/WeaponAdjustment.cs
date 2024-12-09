// written by jason blankenship

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdjustment : MonoBehaviour
{

    public Vector3 positionOffset;

    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = positionOffset;
    }

    
}
