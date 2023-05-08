using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    
    private Slider m_Slider;


    // Start is called before the first frame update
    void Start()
    {
        m_Slider = gameObject.GetComponent<Slider>();
        m_Slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float val)
    {
        m_AudioSource.volume = val;
        transform.GetChild(2).GetComponent<Text>().text = val.ToString();
    }
}
