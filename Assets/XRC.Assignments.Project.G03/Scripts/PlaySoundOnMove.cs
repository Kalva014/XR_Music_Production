using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaySoundOnMove : MonoBehaviour
{
    public float threshold = 0.5f;
    public AudioSource audioSource;
    public GameObject xrRig;

    private Vector2 primary2DAxis;

    void Start()
    {
        // Ensure the AudioSource component is assigned
        if (audioSource == null)
        {
            audioSource = xrRig.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Read the primary 2D axis (joystick) value from the locomotion controller
        primary2DAxis = Gamepad.current.leftStick.ReadValue();

        // Check if the forward movement on the joystick exceeds the threshold
        if (primary2DAxis.y > threshold)
        {
            // If the AudioSource is not playing, start playing the sound
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // If the AudioSource is playing and the joystick is below the threshold, stop the sound
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}