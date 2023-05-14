using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Light : MonoBehaviour
{
    public float currentTimeOfDay = 0.9f;
    public float dayCycleSpeed = 0.01f;

    private bool isOneCycleCompleted = false;

    void Start()
    {
        // Set the initial time of day and cycle speed
        currentTimeOfDay = 0.9f;
        dayCycleSpeed = 0.01f;
    }

    void Update()
    {
        if (!isOneCycleCompleted)
        {
            // Increase the time of day based on the cycle speed
            currentTimeOfDay += dayCycleSpeed * Time.deltaTime;

            // If the time of day is greater than or equal to 1.75, reset it to 0.9 and mark one cycle as completed
            if (currentTimeOfDay >= 1.75f)
            {
                currentTimeOfDay = 0.9f;
                isOneCycleCompleted = true;
            }

            // Calculate the rotation angle of the directional light based on the current time of day
            float angle = currentTimeOfDay * 360.0f;
            transform.rotation = Quaternion.Euler(angle, 45.0f, 0.0f);
        }
    }
}
