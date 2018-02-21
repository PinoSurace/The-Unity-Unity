using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Rope generator script for level 1. The script generates ropes
 * having a random distance between each other.
 */
public class RopeGenerator : MonoBehaviour {
    public Transform ropePrefab;

    // Don't touch these two values. They are hard-coded to fit the existing rope prefab.
    private const float ropeY = 3.0f;
    private const float ropeParentOffset = 1.7f;

    // These three values together define the length of level 1
    /**
     * Defines the minimum distance between ropes
     */
    private const int ropeMinDistance = 7; // As a function of difficulty?

    /**
     * Defines the maximum distance between ropes
     */
    private const int ropeMaxDistance = 9; // As a function of difficulty?

    /**
     * Defines the number of ropes generated
     */
    private const int numberOfRopes = 10; // As a function of difficulty?

    /**
     * Defines the minimum speed for ropes
     */
    private const int ropeMinSpeed = 1;

    /**
     * Defines the maximum speed for ropes
     */
    private const int ropeMaxSpeed = 5;

    void Start()
    {
        var previousRopeX = ropeParentOffset;
        var ropeDistance = 0;
        for (int n = 0; n < numberOfRopes; n++)
        {
            var rope = Instantiate(ropePrefab, new Vector3(previousRopeX - ropeDistance, ropeY, 0), Quaternion.identity);

            // We also need to move the connected anchor of the first element's hinge joint to the correct place
            var firstElement = rope.Find("rope_1");
            var y = firstElement.gameObject.GetComponent<HingeJoint2D>().connectedAnchor.y;
            firstElement.gameObject.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(previousRopeX - ropeDistance - ropeParentOffset, y);

            // Set a random speed (gravity scale) for each rope
            var ropeSpeed = Random.Range(ropeMinSpeed, ropeMaxSpeed);
            for (int i = 0; i < rope.childCount; i++)
            {
                var ropeElement = rope.GetChild(i);
                ropeElement.gameObject.GetComponent<Rigidbody2D>().gravityScale = ropeSpeed;
            }

            previousRopeX -= ropeDistance;
            ropeDistance = Random.Range(ropeMinDistance, ropeMaxDistance);
        }
    }
}