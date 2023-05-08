using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Assignments.Project.G03.Scripts
{
    public class VideoInteractable : XRGrabInteractable
    {
        [SerializeField]
        private VideoPlayer m_VideoPlayer;

        public VideoPlayer videoPlayer
        {
            get => m_VideoPlayer;
            set => m_VideoPlayer = value;
        }
    }
}