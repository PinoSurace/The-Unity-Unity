using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpeedAdjuster : MonoBehaviour {
    
    private const float boostRegionLimit = 10.0f; // Gravity boost region limit as degrees from center
    private const float gravityBoost = 5.0f; // Gravity boost coefficient
    
    enum Side
    {
        LEFT = 0,
        RIGHT = 1
    }
    
    private float originalGravityScale = 0.0f;
    private HingeJoint2D hingeJoint2D;

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

    void BoostGravity(Side side)
    {
        float boostedGravityScale = originalGravityScale * gravityBoost;

        if (hingeJoint2D.jointSpeed > 0.0f)
        {
            if (side == Side.RIGHT)
            {
                GetComponent<Rigidbody2D>().gravityScale = boostedGravityScale;
            }
            else if (side == Side.LEFT)
            {
                GetComponent<Rigidbody2D>().gravityScale = -boostedGravityScale;
            }
        }
        else if (hingeJoint2D.jointSpeed <= 0.0f)
        {
            if (side == Side.RIGHT)
            {
                GetComponent<Rigidbody2D>().gravityScale = -boostedGravityScale;
            }
            else if (side == Side.LEFT)
            {
                GetComponent<Rigidbody2D>().gravityScale = boostedGravityScale;
            }
        }
    }
}
