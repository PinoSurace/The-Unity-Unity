using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour {

    private void FixedUpdate()
    {
        Quaternion counter = this.transform.parent.rotation;
        counter.z = 0;
        this.gameObject.transform.rotation = counter;
    }
}
