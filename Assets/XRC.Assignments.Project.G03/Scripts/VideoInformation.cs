using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace XRC.Assignments.Project.G03.Scripts
{
    public class VideoInformation : MonoBehaviour
    {
        private VideoPlayer m_VideoPlayer;
        private string m_DebugString;

        public string debugString
        {
            get => m_DebugString;
            set => m_DebugString = value;
        }

        [SerializeField]
        private Text m_Text;
    
        // Start is called before the first frame update
        void Start()
        {
            m_VideoPlayer = GetComponent<VideoPlayer>();

            UpdateDebugString();
        }

        private void UpdateDebugString()
        {
            m_DebugString = $"VideoPlayer properties: \n" +
                            $"frame: {m_VideoPlayer.frame}\n" +
                            $"time: {m_VideoPlayer.time}\n" +
                            $"length: {m_VideoPlayer.length}\n" +
                            $"clockTime: {m_VideoPlayer.clockTime}\n" +
                            $"frameCount: {m_VideoPlayer.frameCount}\n" +
                            $"isLooping: {m_VideoPlayer.isLooping}\n" +
                            $"isPlaying: {m_VideoPlayer.isPlaying}\n" +
                            $"isPaused: {m_VideoPlayer.isPaused}\n" +
                            $"playbackSpeed: {m_VideoPlayer.playbackSpeed}\n";
        }

        void Update()
        {
            UpdateDebugString();
            m_Text.text = m_DebugString;
            m_Text.color = m_VideoPlayer.isPlaying ? Color.black : Color.red;
        }
    }
}