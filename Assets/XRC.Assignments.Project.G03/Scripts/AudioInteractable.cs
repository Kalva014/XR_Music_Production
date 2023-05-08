using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Assignments.Project.G03.Scripts
{
    public class AudioInteractable : XRGrabInteractable
    {
        
        
        [SerializeField]
        private AudioSource m_AudioSource;

        public AudioSource audioSource
        {
            get => m_AudioSource;
            set => m_AudioSource = value;
        }

        private bool m_DebugCanvasOn;

        public bool debugCanvasOn
        {
            get => m_DebugCanvasOn;
            set => m_DebugCanvasOn = value;
        }

        private void Awake()
        {
            base.Awake();
            AssignInteractionManager();
        }
        
        private void AssignInteractionManager()
        {
            GameObject xrOrigin = GameObject.Find("XR Origin");
            if (xrOrigin != null)
            {
                XRInteractionManager xrInteractionManager = xrOrigin.GetComponent<XRInteractionManager>();
                if (xrInteractionManager != null)
                {
                    interactionManager = xrInteractionManager;
                }
                else
                {
                    Debug.LogWarning("No XRInteractionManager found on XR Origin.");
                }
            }
            else
            {
                Debug.LogWarning("No XR Origin found in the scene.");
            }
        }
    }
}