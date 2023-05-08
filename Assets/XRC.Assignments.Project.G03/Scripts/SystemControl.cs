using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Assignments.Project.G03.Scripts
{
    public static class SystemControl
    {
        /// <summary>
        /// Returns the distance in 3D space between startPosition and currentPosition, as percentage of maxDistance.
        /// This can be used to get values from a sweeping gesture, to be passed along as a value between 0 and 1 
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="currentPosition"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public static float GetSweepPercentage(Vector3 startPosition, Vector3 currentPosition, float maxDistance = 1.0f)
        {
            var percentage = Mathf.Clamp(Vector3.Distance(currentPosition, startPosition), 0, maxDistance)/maxDistance;
            return percentage;
        }

        public static void ToggleAudio(AudioSource audioSource, IXRHoverInteractable hoverInteractable)
        {
            hoverInteractable.transform.gameObject.GetComponent<ParticleSystem>().Play();
            audioSource.Play();
        }
        

        public static void ToggleMenu(GameObject menu)
        {
            Debug.Log("toggling menu");
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
            else
            {
                menu.SetActive(true);
            }
        }
        
        public static Vector3 BimanualDistance(InputActionAsset defaultActions, GameObject xrOriginObject, LineRenderer lineRenderer)
        {
            InputAction leftHandPositionAction = defaultActions.FindActionMap("XRI LeftHand").FindAction("Position");
            InputAction rightHandPositionAction = defaultActions.FindActionMap("XRI RightHand").FindAction("Position");

            Vector3 leftHandPosition = xrOriginObject.transform.TransformPoint(leftHandPositionAction.ReadValue<Vector3>());
            Vector3 rightHandPosition = xrOriginObject.transform.TransformPoint(rightHandPositionAction.ReadValue<Vector3>());
            
            lineRenderer.SetPosition(0, leftHandPosition);
            lineRenderer.SetPosition(1, rightHandPosition);
            Vector3 differencePositions = leftHandPosition - rightHandPosition;

            return differencePositions;
        }

        public static Vector3[] SetNotePositions(OrderedSet<IXRHoverInteractable> notes)
        {
            Vector3[] notePositions = new Vector3[notes.Count];
            int i = 0;
            foreach (IXRHoverInteractable note in notes)
            {
                notePositions[i] = note.transform.position;
                i++;
            }

            return notePositions;
        }
    }
}