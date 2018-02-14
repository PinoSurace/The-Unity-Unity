// Uncomment this line to enable debug prints for the class
//#define DEBUG_OSCILLATION_MAINTAINER

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Oscillation maintainer class for maintaining the oscillation
 * amplitude of the swinging ropes. The maintainer works by
 * applying gravity boost to the rope elements in specific
 * positions and directions of rope movement.
 */
public class OscillationMaintainer : MonoBehaviour {

    /**
     * Gravity boost region limit as degrees from center
     */
    private const float boostRegionLimit = 20.0f;
    
    /**
     * Gravity boost coefficient defines how big the gravity boost is
     */
    private const float gravityBoost = 5.0f;

    /**
     * Side is used to define on which side the rope is currently
     * to define the gravity boost direction.
     */
    private enum Side
    {
        LEFT,
        RIGHT
    }
    
    private float originalGravityScale = 0.0f;
    private HingeJoint2D hingeJoint2D;

    /**
    * @brief Boosts the gravity according to where the rope is and where it is going.
    *
    * @param[in] side The side on which the rope currently is - left or right - to define
    *                 which direction to boost the gravity
    */
    private void BoostGravity(Side side)
    {
        float boostedGravityScale = originalGravityScale * gravityBoost;

        if (hingeJoint2D.jointSpeed > 0.0f)
        {
            if (side == Side.RIGHT)
            {
                GetComponent<Rigidbody2D>().gravityScale = boostedGravityScale;
#if (DEBUG_OSCILLATION_MAINTAINER)
                print(Time.time.ToString() + " Gravity boost direction: DOWN");
#endif
            }
            else if (side == Side.LEFT)
            {
                GetComponent<Rigidbody2D>().gravityScale = -boostedGravityScale;
#if (DEBUG_OSCILLATION_MAINTAINER)
                print(Time.time.ToString() + " Gravity boost direction: UP");
#endif
            }
        }
        else if (hingeJoint2D.jointSpeed <= 0.0f)
        {
            if (side == Side.RIGHT)
            {
                GetComponent<Rigidbody2D>().gravityScale = -boostedGravityScale;
#if (DEBUG_OSCILLATION_MAINTAINER)
                print(Time.time.ToString() + " Gravity boost direction: UP");
#endif
            }
            else if (side == Side.LEFT)
            {
                GetComponent<Rigidbody2D>().gravityScale = boostedGravityScale;
#if (DEBUG_OSCILLATION_MAINTAINER)
                print(Time.time.ToString() + " Gravity boost direction: DOWN");
#endif
            }
        }
    }

    void Start()
    {
        // Get the original gravity scale
        originalGravityScale = GetComponent<Rigidbody2D>().gravityScale;
    }
    
    void Update()
    {
        hingeJoint2D = GetComponent<HingeJoint2D>();

        if (hingeJoint2D.jointAngle >= -boostRegionLimit &&
            hingeJoint2D.jointAngle <= 0)
        {
            // We are within the right side of the gravity boost region
            BoostGravity(Side.RIGHT);
        }
        else if (hingeJoint2D.jointAngle > 0 &&
                 hingeJoint2D.jointAngle <= boostRegionLimit)
        {
            // We are within the left side of the gravity boost region
            BoostGravity(Side.LEFT);
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = originalGravityScale;
        }
    }
}
