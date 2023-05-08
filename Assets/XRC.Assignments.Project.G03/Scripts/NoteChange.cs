using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NoteChange : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public float[] scaleYValues = { 0.0625f, 0.125f, 0.25f, 0.5f, 1f, 2f };
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectExited.AddListener(UpdateYScaleAndAudioClip);
    }

    private void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(UpdateYScaleAndAudioClip);
    }

    private void UpdateYScaleAndAudioClip(SelectExitEventArgs args)
    {
        UpdateYScale();
        UpdateAudioClip();
    }

    private void UpdateYScale()
    {
        float currentYScale = transform.localScale.y;
        float closestScaleY = scaleYValues[0];
        float minDistance = Mathf.Abs(currentYScale - closestScaleY);

        for (int i = 1; i < scaleYValues.Length; i++)
        {
            float distance = Mathf.Abs(currentYScale - scaleYValues[i]);
            if (distance < minDistance)
            {
                closestScaleY = scaleYValues[i];
                minDistance = distance;
            }
        }

        transform.localScale = new Vector3(transform.localScale.x, closestScaleY, transform.localScale.z);
    }

    private void UpdateAudioClip()
    {
        float newYScale = transform.localScale.y;
        int index = System.Array.IndexOf(scaleYValues, newYScale);

        string currentClipName = audioSource.clip.name;
        int currentNote = int.Parse(currentClipName.Substring(0, 1));
        string noteName = currentClipName.Substring(1);

        int newNote = Mathf.Clamp(currentNote + index - 2, 2, 7);
        string newClipName = newNote.ToString() + noteName;

        AudioClip newClip = audioClips.Find(clip => clip.name == newClipName);
    
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.pitch = Mathf.Pow(2, index - (newNote - currentNote));
        }
    }
}