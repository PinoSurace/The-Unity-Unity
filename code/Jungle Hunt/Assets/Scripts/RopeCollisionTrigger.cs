// Uncomment this line to enable debug prints for the class
//#define DEBUG_ROPE_COLLISION_TRIGGER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Rope collision trigger class for detecting the player
 * colliding with the rope.
 */
public class RopeCollisionTrigger : MonoBehaviour {

    void OnTriggerEnter2D()
    {
#if (DEBUG_ROPE_COLLISION_TRIGGER)
        print(Time.time.ToString() + ": Another collider entered " + this.name + "!");
#endif
    }

    void OnTriggerExit2D()
    {
#if (DEBUG_ROPE_COLLISION_TRIGGER)
        print(Time.time.ToString() + ": Another collider exit " + this.name + "!");
#endif
    }
}