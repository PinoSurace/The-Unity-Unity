using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Level generator script for level 1. The script generates ropes,
 * background, leaves and grass.
 */
public class LevelGenerator : MonoBehaviour {
    public Transform ropePrefab;

    /**
     * Don't touch these two values. They are hard-coded values to define
     * the length of the level start (tree etc.) and the level end (water etc.)
     */
    private const float levelStartLength = 3.0f;
    private const float levelEndLength = 5.0f;

    private float levelTotalLength = 0.0f;

    void Start()
    {
        // Y coordinate of the ropes
        const float ropeY = 4.0f; // Should this be constant or also vary between some limits?

        // These following three values together define the length of level 1
        // Minimum distance between ropes
        const int ropeMinDistance = 7; // As a function of difficulty?

        // Maximum distance between ropes
        const int ropeMaxDistance = 9; // As a function of difficulty?

        // Number of ropes generated
        const int numberOfRopes = 10; // As a function of difficulty?

        // Minimum speed for ropes
        const int ropeMinSpeed = 1;

        // Maximum speed for ropes
        const int ropeMaxSpeed = 5;

        GenerateRopes(numberOfRopes,
                      ropeMinDistance,
                      ropeMaxDistance,
                      ropeMinSpeed,
                      ropeMaxSpeed,
                      ropeY);
        GenerateBackground();
        GenerateFoliage();
    }

    /**
     * @brief Generates ropes having a random distance between each other.
     * 
     * @param numberOfRopes number of ropes to generate
     * @param ropeMinDistance minimum distance between ropes
     * @param ropeMaxDistance maximum distance between ropes
     * @param ropeMinSpeed minimum rope speed
     * @param ropeMaxSpeed maximum rope speed
     * @param ropeY y coordinate for the ropes
     */
    private void GenerateRopes(int numberOfRopes,
                               int ropeMinDistance,
                               int ropeMaxDistance,
                               int ropeMinSpeed,
                               int ropeMaxSpeed,
                               float ropeY)
    {
        // Don't touch these two values. They are hard-coded to fit the existing rope prefab.
        const float ropeJointXOffset = 1.7f;
        const float ropeJointYOffset = 1.0f;

        var previousRopeX = ropeJointXOffset;
        var ropeDistance = 0;
        for (int n = 0; n < numberOfRopes; n++)
        {
            var rope = Instantiate(ropePrefab, new Vector3(previousRopeX - ropeDistance, ropeY, 0), Quaternion.identity);

            // We also need to move the connected anchor of the first element's hinge joint to the correct place
            var firstElement = rope.Find("rope_1");
            firstElement.gameObject.GetComponent<HingeJoint2D>().connectedAnchor
                = new Vector2(previousRopeX - ropeDistance - ropeJointXOffset, ropeY + ropeJointYOffset);

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

        // The level length is defined by where the last rope is placed + some offset to cover
        // the beginning and the end of the level
        levelTotalLength = -previousRopeX + levelStartLength + levelEndLength;
    }

    void GenerateBackground()
    {
        // Generate background
    }

    void GenerateFoliage()
    {
        // Generate top screen leaves and bottom screen grass
    }
}