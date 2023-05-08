using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using LineRenderer = UnityEngine.LineRenderer;
 
public class TwoHandGrabMove : ConstrainedMoveProvider
{
 [SerializeField]
 [Tooltip("The left hand grab move instance which will be used as one half of two-handed locomotion.")]
 GrabMoveProvider m_LeftGrabMoveProvider;
 /// <summary>
 /// The left hand grab move instance which will be used as one half of two-handed locomotion.
 /// </summary>
 public GrabMoveProvider leftGrabMoveProvider
 {
     get => m_LeftGrabMoveProvider;
     set => m_LeftGrabMoveProvider = value;
 }

 [SerializeField]
 [Tooltip("The right hand grab move instance which will be used as one half of two-handed locomotion.")]
 GrabMoveProvider m_RightGrabMoveProvider;
 /// <summary>
 /// The right hand grab move instance which will be used as one half of two-handed locomotion.
 /// </summary>
 public GrabMoveProvider rightGrabMoveProvider
 {
     get => m_RightGrabMoveProvider;
     set => m_RightGrabMoveProvider = value;
 }

 [SerializeField]
 [Tooltip("Controls whether to enable yaw rotation of the user.")]
 bool m_EnableRotation = true;
 /// <summary>
 /// Controls whether to enable yaw rotation of the user.
 /// </summary>
 public bool enableRotation
 {
     get => m_EnableRotation;
     set => m_EnableRotation = value;
 }

 [SerializeField]
 [Tooltip("Controls whether to enable uniform scaling of the user.")]
 bool m_EnableScaling;
 /// <summary>
 /// Controls whether to enable uniform scaling of the user.
 /// </summary>
 public bool enableScaling
 {
     get => m_EnableScaling;
     set => m_EnableScaling = value;
 }

 [SerializeField]
 [Tooltip("The minimum user scale allowed.")]
 float m_MinimumScale = 0.2f;
 /// <summary>
 /// The minimum user scale allowed.
 /// </summary>
 public float minimumScale
 {
     get => m_MinimumScale;
     set => m_MinimumScale = value;
 }

 [SerializeField]
 [Tooltip("The maximum user scale allowed.")]
 float m_MaximumScale = 100f;
 /// <summary>
 /// The maximum user scale allowed.
 /// </summary>
 public float maximumScale
 {
     get => m_MaximumScale;
     set => m_MaximumScale = value;
 }

 public bool m_IsMoving;

 Vector3 m_PreviousMidpointBetweenControllers;
 float m_InitialOriginYaw;
 Vector3 m_PreviousLeftToRightDirection;
 Vector3 m_PreviousForwardAverage;
 Vector3 m_InitialLeftToRightOrthogonal;

 float m_PreviousOriginScale;
 float m_PreviousDistanceBetweenHands;

 Vector3 m_InitialPosition;
 Quaternion m_InitialRotation;

 /// <summary>
 /// See <see cref="MonoBehaviour"/>.
 /// </summary>
 protected void OnEnable()
 {
     if (m_LeftGrabMoveProvider == null || m_RightGrabMoveProvider == null)
     {
         Debug.LogError("Left or Right Grab Move Provider is not set or has been destroyed.", this);
         enabled = false;
     }
     m_LeftGrabMoveProvider.canMove = false;
     m_RightGrabMoveProvider.canMove = true;
 }

 /// <summary>
 /// See <see cref="MonoBehaviour"/>.
 /// </summary>
 protected void OnDisable()
 {
     if (m_LeftGrabMoveProvider != null)
         m_LeftGrabMoveProvider.canMove = true;
     if (m_RightGrabMoveProvider != null)
         m_RightGrabMoveProvider.canMove = true;
 }

 /// <inheritdoc/>
 protected override Vector3 ComputeDesiredMove(out bool attemptingMove)
     {
         attemptingMove = false;
         var wasMoving = m_IsMoving;
         var xrOrigin = system.xrOrigin;
         var originTransform = xrOrigin.transform;
         var leftHandLocalPosition = m_LeftGrabMoveProvider.controllerTransform.localPosition;
         var rightHandLocalPosition = m_RightGrabMoveProvider.controllerTransform.localPosition;
         var midpointLocalPosition = (leftHandLocalPosition + rightHandLocalPosition) * 0.5f;
         var leftForward = m_LeftGrabMoveProvider.controllerTransform.localRotation * Vector3.forward;
         var rightForward = m_RightGrabMoveProvider.controllerTransform.localRotation * Vector3.forward;
         
         //Debug.Log("Right local: " + rightHandLocalPosition + " Right Forward: " + rightForward + " Combined: " + (rightForward + rightHandLocalPosition));
       
         m_IsMoving = m_LeftGrabMoveProvider.IsGrabbing() && m_RightGrabMoveProvider.IsGrabbing() && xrOrigin != null;
         if (!m_IsMoving)
         {
             m_LeftGrabMoveProvider.canMove = false;
             m_RightGrabMoveProvider.canMove = true;
             
             return Vector3.zero;
         }

         // Prevent individual grab locomotion since we perform our own translation
         m_LeftGrabMoveProvider.canMove = false;
         m_RightGrabMoveProvider.canMove = false;

        
         if (!wasMoving && m_IsMoving) // Cannot simply check locomotionPhase because it might always be in moving state, due to gravity application mode
         {
             m_PreviousLeftToRightDirection = rightHandLocalPosition - leftHandLocalPosition;
             m_PreviousForwardAverage = (leftForward + rightForward) * 0.5f;

             m_PreviousOriginScale = originTransform.localScale.x;
             m_PreviousDistanceBetweenHands = Vector3.Distance(leftHandLocalPosition, rightHandLocalPosition);
             
             m_PreviousMidpointBetweenControllers = midpointLocalPosition;

             // Do not move the first frame of grab
             return Vector3.zero;
         }
         attemptingMove = true;
         var midpointWorldPosition = originTransform.TransformPoint(midpointLocalPosition);

         // --- Movement ---
         
         var move = originTransform.TransformVector(m_PreviousMidpointBetweenControllers - midpointLocalPosition);
         originTransform.position += move;
         m_PreviousMidpointBetweenControllers = midpointLocalPosition;
         
         // --- Scale --- 

         var c_DistanceBetweenHands = Vector3.Distance(leftHandLocalPosition, rightHandLocalPosition);
         var targetScale = c_DistanceBetweenHands != 0f
             ? m_PreviousOriginScale * (m_PreviousDistanceBetweenHands / c_DistanceBetweenHands)
             : originTransform.localScale.x;
         targetScale = Mathf.Clamp(targetScale, m_MinimumScale, m_MaximumScale);
         originTransform.localScale = Vector3.one * targetScale;

         // compensatory move
         var distMidpointToRig = originTransform.position - midpointWorldPosition;
         originTransform.position = midpointWorldPosition +
                                    distMidpointToRig * (targetScale / m_PreviousOriginScale);
         
         m_PreviousOriginScale = originTransform.localScale.x;
         m_PreviousDistanceBetweenHands = c_DistanceBetweenHands;

         
         /*
         // --- Rotation 1 ---
         
         var c_LeftToRightDirection = rightHandLocalPosition - leftHandLocalPosition;
         var rot1Axis = Vector3.Cross(c_LeftToRightDirection, m_PreviousLeftToRightDirection);
         originTransform.RotateAround(midpointWorldPosition, originTransform.TransformVector(rot1Axis), 
             Vector3.Angle(c_LeftToRightDirection, m_PreviousLeftToRightDirection));

         m_PreviousLeftToRightDirection = c_LeftToRightDirection;
         
         
         // --- Rotation 2 ---
         var c_ForwardAverage = (leftForward + rightForward) * 0.5f;
         var rot2Axis = c_LeftToRightDirection;
         var oldForwardProject = Vector3.ProjectOnPlane(m_PreviousForwardAverage, rot2Axis);
         var curForwardProject = Vector3.ProjectOnPlane(c_ForwardAverage, rot2Axis);
         originTransform.RotateAround(midpointWorldPosition, originTransform.TransformVector(rot2Axis),
         Vector3.SignedAngle(curForwardProject, oldForwardProject, rot2Axis));

         m_PreviousForwardAverage = c_ForwardAverage;*/

         return Vector3.zero;
     } 
}
