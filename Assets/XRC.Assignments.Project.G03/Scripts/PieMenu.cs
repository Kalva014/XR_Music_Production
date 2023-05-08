using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XRC.Assignments.Project.G03.Scripts;

public class PieMenu : MonoBehaviour
{
    [Header("Menu Button Mappings")]
    [SerializeField] private Button m_ANote;
    [SerializeField] private Button m_BNote;
    [SerializeField] private Button m_CNote;
    [SerializeField] private Button m_DNote;
    [SerializeField] private Button m_ENote;
    [SerializeField] private Button m_FNote;
    [SerializeField] private Button m_GNote;
    [SerializeField] private Button m_OtherOptions;
    [SerializeField] private Button m_ChangeInstrumentButton;
    [SerializeField] private GameObject Octave;
    
    [Header("Xylophone Notes")]
    [SerializeField] private GameObject m_XyloA5;
    [SerializeField] private GameObject m_XyloB5;
    [SerializeField] private GameObject m_XyloC5;
    [SerializeField] private GameObject m_XyloD5;
    [SerializeField] private GameObject m_XyloE5;
    [SerializeField] private GameObject m_XyloF5;
    [SerializeField] private GameObject m_XyloG5;
    [SerializeField] private GameObject m_Drums;
    
    [Header("Bass Notes")]
    [SerializeField] private GameObject m_BassA5;
    [SerializeField] private GameObject m_BassB5;
    [SerializeField] private GameObject m_BassC5;
    [SerializeField] private GameObject m_BassD5;
    [SerializeField] private GameObject m_BassE5;
    [SerializeField] private GameObject m_BassF5;
    [SerializeField] private GameObject m_BassG5;
    [SerializeField] private GameObject m_Harp;
    
    [Header("Guitar Notes")]
    [SerializeField] private GameObject m_GuitarA5;
    [SerializeField] private GameObject m_GuitarB5;
    [SerializeField] private GameObject m_GuitarC5;
    [SerializeField] private GameObject m_GuitarD5;
    [SerializeField] private GameObject m_GuitarE5;
    [SerializeField] private GameObject m_GuitarF5;
    [SerializeField] private GameObject m_GuitarG5; 
    [SerializeField] private GameObject m_GuitarPower; 

    
    [SerializeField] private GameObject m_SceneManager;
    private static String currentNote;
    private static int currentInstrument = 0;
    List<string> instrumentsNames = new List<string> {"Xylophone", "Bass", "Guitar"};

    // Start is called before the first frame update
    void Start()
    {
        m_ANote.onClick.AddListener(() => CurrentNote("A_Note"));
        m_BNote.onClick.AddListener(() => CurrentNote("B_Note"));
        m_CNote.onClick.AddListener(() => CurrentNote("C_Note"));
        m_DNote.onClick.AddListener(() => CurrentNote("D_Note"));
        m_ENote.onClick.AddListener(() => CurrentNote("E_Note"));
        m_FNote.onClick.AddListener(() => CurrentNote("F_Note"));
        m_GNote.onClick.AddListener(() => CurrentNote("G_Note"));
        m_OtherOptions.onClick.AddListener(() => CurrentNote("OtherOptions"));
        m_ChangeInstrumentButton.onClick.AddListener(() => UpdateInstrument());

    }
    
    // Based off which note is pressed on the pie menu instantiates the note object that can be placed
    void CurrentNote(String notePressed)
    {
        currentNote = notePressed;

        if (instrumentsNames[currentInstrument] == "Xylophone") {
            if (currentNote == "A_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_XyloA5;
            }
            else if (currentNote == "B_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_XyloB5;
            }
            else if (currentNote == "C_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_XyloC5;
            }
            else if (currentNote == "D_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_XyloD5;
            }
            else if (currentNote == "E_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_XyloE5;
            }
            else if (currentNote == "F_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_XyloF5;
            }
            else if (currentNote == "G_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_XyloG5;
            } 
            else if (currentNote == "OtherOptions")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_Drums;
            } 
        }
        else if (instrumentsNames[currentInstrument] == "Bass") {
            if (currentNote == "A_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_BassA5;
            }
            else if (currentNote == "B_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_BassB5;
            }
            else if (currentNote == "C_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_BassC5;
            }
            else if (currentNote == "D_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_BassD5;
            }
            else if (currentNote == "E_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_BassE5;
            }
            else if (currentNote == "F_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_BassF5;
            }
            else if (currentNote == "G_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_BassG5;
            } 
            else if (currentNote == "OtherOptions")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_Harp;
            } 
        }
        else if (instrumentsNames[currentInstrument] == "Guitar") {
            if (currentNote == "A_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarA5;
            }
            else if (currentNote == "B_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarB5;
            }
            else if (currentNote == "C_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarC5;
            }
            else if (currentNote == "D_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarD5;
            }
            else if (currentNote == "E_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarE5;
            }
            else if (currentNote == "F_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarF5;
            }
            else if (currentNote == "G_Note")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarG5;
            }
            else if (currentNote == "OtherOptions")
            {
                m_SceneManager.GetComponent<SceneManager>().notePrefab = m_GuitarPower;
            } 
        }
    }

    // Update current instrument
    void UpdateInstrument()
    {
        if (currentInstrument != 2)
        {
            currentInstrument += 1;
        }
        else {
            currentInstrument = 0;
        }

        Octave.GetComponent<TextMeshProUGUI>().text = instrumentsNames[currentInstrument];
    }

    // If users clicks anywhere using the right controller they place an object that end of the ray
    public static void PlaceNote(GameObject notePrefab, Vector3 position, Quaternion rotation, Transform audioInteractables)
    {
        GameObject newNote = Instantiate(notePrefab, audioInteractables);
        newNote.transform.position = position;
    }
}
