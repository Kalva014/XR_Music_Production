using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class AudioInformation : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private string m_DebugString;
    [SerializeField]
    private XRGrabInteractable m_Interactable;

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
        m_AudioSource = GetComponent<AudioSource>();

        UpdateDebugString();
    }

    private void UpdateDebugString()
    {
        float scaleX = m_Interactable.transform.localScale.x;
        float baseSize = 0.0625f;
        int power = Mathf.RoundToInt(scaleX / baseSize / 2.0f);
        // Debug.Log(power);
        
        switch (power)
        {
            case 5:
                m_DebugString = m_AudioSource.clip.name + " Whole Note";
                break;
            case 4:
                m_DebugString = m_AudioSource.clip.name + " Half Note";
                break;
            case 3:
                m_DebugString = m_AudioSource.clip.name + " Quarter Note";
                break;
            case 2:
                m_DebugString = m_AudioSource.clip.name + " Eighth Note";
                break;
            case 1:
                m_DebugString = m_AudioSource.clip.name + " Sixteenth Note";
                break;
            default:
                m_DebugString = $"Invalid Note Size: {scaleX}";
                break;
        }
    }
    
    void Update()
    {
        // UpdateDebugString();
        m_Text.text = m_DebugString;
        m_Text.color = m_AudioSource.isPlaying ? Color.black : Color.red;
    }
}