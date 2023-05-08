using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRC.Assignments.Project.G03.Scripts
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractor m_LeftRayInteractor;
        [SerializeField] private XRBaseInteractor m_RightRayInteractor;

        
        [SerializeField] private InputActionAsset m_XRIActions;
        [SerializeField] private InputActionAsset m_SystemControlActions;
        
        [SerializeField] private InputActionProperty m_ToggleUI;
        [SerializeField] private InputActionProperty m_PlaceNote;
        [SerializeField] private InputActionProperty m_PitchBimanual;
        [SerializeField] private InputActionProperty m_VolumeBimanual;
        [SerializeField] private InputActionProperty m_ConnectSequentialNotes;
        [SerializeField] private InputActionProperty m_PlayNotes;
        [SerializeField] private InputActionProperty m_RemoveSequentialNote;
        [SerializeField] private InputActionProperty m_DeleteNote;
        [SerializeField] private InputActionProperty m_MoveNote;
        [SerializeField] private InputActionProperty m_ToggleHelp;
        
        
        [SerializeField] private GameObject m_XROrigin;
        [SerializeField] private GameObject m_PieMenu;
        [SerializeField] private GameObject m_AudioInteractables;
        [SerializeField] private GameObject m_BimanualLine;
        [SerializeField] private GameObject m_SequentialLine;
        [SerializeField] private GameObject m_NotePrefab;
        [SerializeField] private GameObject m_HelpMenu;
        public GameObject notePrefab
        {
            get => m_NotePrefab;
            set => m_NotePrefab = value;
        }

        // Action maps used to bulk-enable/disable actions
        private InputActionMap m_AudioActionMap;
        private InputActionMap m_GeneralActionMap;
        private InputActionMap m_VideoActionMap;

        /// <summary>
        /// The audio source component on a currently hovered audio interactable
        /// </summary>
        private AudioSource m_AudioSource;
        
        /// <summary>
        /// The currently hovered interactable
        /// </summary>
        private IXRHoverInteractable m_Interactable;
        
        /// <summary>
        /// Variables for keeping track of data for specific actions
        /// </summary>
        private Vector3 m_BimanualStartDistance;
        private LineRenderer m_BimanualLineRenderer;
        private LineRenderer m_SequentialLineRenderer;
        private OrderedSet<IXRHoverInteractable> m_SequentialNotes;
        private Vector3[] m_SequentialNotePositions;



        private void Awake()
        {
            m_SequentialNotes = new OrderedSet<IXRHoverInteractable>();
            InitializeInputActionMaps();
            SetupInteractorEvents();
            SetupCallbacks();
            SetupLineRenderers();
        }

        private void OnEnable()
        {
            m_AudioActionMap.Enable();
            m_GeneralActionMap.Enable();
            SetupInteractorEvents();
        }

        private void OnDisable()
        { 
            m_AudioActionMap.Disable();
            m_GeneralActionMap.Disable();
            TeardownInteractorEvents();
        }

        private void SetupCallbacks()
        {
            m_ToggleUI.reference.action.performed += _ => Project.G03.Scripts.SystemControl.ToggleMenu(m_PieMenu);
            m_ToggleHelp.reference.action.performed += _ => Project.G03.Scripts.SystemControl.ToggleMenu(m_HelpMenu);
            
            
            m_PlaceNote.reference.action.performed += _ =>
            {
                Debug.Log("reaches here");
                Transform attachTransform  = m_RightRayInteractor.attachTransform;
                PieMenu.PlaceNote(m_NotePrefab, attachTransform.position, attachTransform.rotation, m_AudioInteractables.transform);
            };
            
            m_PitchBimanual.reference.action.started += _ =>
            {
                m_RightRayInteractor.enabled = false;
                m_LeftRayInteractor.enabled = false;
                m_BimanualLine.SetActive(true);
                m_BimanualLineRenderer.startColor = Color.green;
                m_BimanualLineRenderer.endColor = Color.green;
                m_BimanualLineRenderer.material.color = Color.green;
                m_BimanualStartDistance = SystemControl.BimanualDistance(m_XRIActions, m_XROrigin, m_BimanualLineRenderer);
            };
            m_PitchBimanual.reference.action.performed += _ =>
            {
                Vector3 bimanualCurrentDistance = SystemControl.BimanualDistance(m_XRIActions, m_XROrigin, m_BimanualLineRenderer);
                Vector3 differenceDistance = m_BimanualStartDistance - bimanualCurrentDistance;
                
                float modifier = Mathf.Round(differenceDistance.x * 2);
                if (Mathf.Abs(modifier) >= 1)
                {
                    m_BimanualStartDistance = bimanualCurrentDistance;
                }
                float newX = m_Interactable.transform.localScale.x;
                if ((newX >= 2.0f && newX <= 4.0f) || (newX < 2.0f && modifier > 0) || (newX > 4.0f && modifier < 0))
                {
                    newX += modifier / 2;
                    m_AudioSource.pitch *= Mathf.Pow(2, -1 * modifier);
                }
                m_Interactable.transform.localScale = new Vector3(
                    newX, 
                    m_Interactable.transform.localScale.y, 
                    m_Interactable.transform.localScale.z);
            };
            m_PitchBimanual.reference.action.canceled += _ =>
            {
                m_RightRayInteractor.enabled = true;
                m_LeftRayInteractor.enabled = true;
                m_BimanualLine.SetActive(false);
            };
            
            m_VolumeBimanual.reference.action.started += _ =>
            {
                m_RightRayInteractor.enabled = false;
                m_LeftRayInteractor.enabled = false;
                m_BimanualLine.SetActive(true);
                m_BimanualLineRenderer.startColor = Color.magenta;
                m_BimanualLineRenderer.endColor = Color.magenta;
                m_BimanualLineRenderer.material.color = Color.magenta;
            };
            m_VolumeBimanual.reference.action.performed += _ =>
            {
                Vector3 bimanualCurrentDistance = SystemControl.BimanualDistance(m_XRIActions, m_XROrigin, m_BimanualLineRenderer);
                Vector3 differenceDistance = m_BimanualStartDistance - bimanualCurrentDistance;
                
                float modifier = Mathf.Round(differenceDistance.y * 2);
                if (Mathf.Abs(modifier) >= 1)
                {
                    m_BimanualStartDistance = bimanualCurrentDistance;
                }
                float newZ = m_Interactable.transform.localScale.z;
                if ((newZ >= 2.0f && newZ <= 4.0f) || (newZ < 2.0f && modifier > 0) || (newZ > 4.0f && modifier < 0))
                {
                    newZ += modifier / 2;
                    m_AudioSource.volume += modifier / 4;
                }
                
                m_Interactable.transform.localScale = new Vector3(
                    m_Interactable.transform.localScale.x,
                    m_Interactable.transform.localScale.y, 
                    newZ);
            };
            m_VolumeBimanual.reference.action.canceled += _ =>
            {
                m_RightRayInteractor.enabled = true;
                m_LeftRayInteractor.enabled = true;
                m_BimanualLine.SetActive(false);
            };
            
            m_ConnectSequentialNotes.reference.action.started += _ =>
            {
                m_LeftRayInteractor.enabled = false;
                m_SequentialLine.SetActive(true);
                m_SequentialLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                float alpha = 1.0f;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.yellow, 0.2f), 
                        new GradientColorKey(Color.green, 0.4f), new GradientColorKey(Color.cyan, 0.6f), 
                        new GradientColorKey(Color.blue, 0.8f), new GradientColorKey(Color.magenta, 1.0f)},
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
                m_SequentialLineRenderer.colorGradient = gradient;
            };
            m_ConnectSequentialNotes.reference.action.performed += _ =>
            {
                m_SequentialNotes.Add(m_Interactable);
            };
            m_ConnectSequentialNotes.reference.action.canceled += _ =>
            {
                m_SequentialNotePositions = SystemControl.SetNotePositions(m_SequentialNotes);
                m_SequentialLineRenderer.positionCount = m_SequentialNotePositions.Length;
                m_SequentialLineRenderer.SetPositions(m_SequentialNotePositions);
                m_LeftRayInteractor.enabled = true;
            };

            m_PlayNotes.reference.action.performed += _ =>
            {
                if (m_SequentialNotes.Contains(m_Interactable))
                {
                    StartCoroutine(PlayWaiter(m_SequentialNotes, m_SequentialNotePositions));
                }
                else
                {
                    AudioSource currentNoteAudio = (m_Interactable as AudioInteractable).audioSource;
                    SystemControl.ToggleAudio(currentNoteAudio, m_Interactable);
                }
            }; 
            
            m_RemoveSequentialNote.reference.action.performed += _ =>
            {
                if (m_SequentialNotes.Contains(m_Interactable))
                {
                    m_SequentialNotes.Remove(m_Interactable);
                    m_SequentialNotePositions = SystemControl.SetNotePositions(m_SequentialNotes);
                    m_SequentialLineRenderer.positionCount = m_SequentialNotePositions.Length;
                    m_SequentialLineRenderer.SetPositions(m_SequentialNotePositions);
                }
            };
            
            m_DeleteNote.reference.action.performed += _ =>
            {
                if (m_SequentialNotes.Contains(m_Interactable))
                {
                    m_SequentialNotes.Remove(m_Interactable);
                    m_SequentialNotePositions = SystemControl.SetNotePositions(m_SequentialNotes);
                    m_SequentialLineRenderer.positionCount = m_SequentialNotePositions.Length;
                    m_SequentialLineRenderer.SetPositions(m_SequentialNotePositions);
                }
                
                Destroy(m_Interactable.transform.gameObject);
            };
            
            m_MoveNote.reference.action.started += _ =>
            {
                if (m_Interactable != null)
                {
                    m_SequentialNotePositions = SystemControl.SetNotePositions(m_SequentialNotes);
                    if (m_SequentialNotes.Contains(m_Interactable))
                    {
                        m_SequentialNotePositions[m_SequentialNotes.ToList().IndexOf(m_Interactable)] =
                            m_Interactable.transform.position;
                    }
                    m_SequentialLineRenderer.positionCount = m_SequentialNotePositions.Length;
                    m_SequentialLineRenderer.SetPositions(m_SequentialNotePositions);
                }
            };
            m_MoveNote.reference.action.performed += _ =>
            {
                if (m_Interactable != null)
                {
                    m_SequentialNotePositions = SystemControl.SetNotePositions(m_SequentialNotes);
                    if (m_SequentialNotes.Contains(m_Interactable))
                    {
                        m_SequentialNotePositions[m_SequentialNotes.ToList().IndexOf(m_Interactable)] =
                            m_Interactable.transform.position;
                    }
                    m_SequentialLineRenderer.positionCount = m_SequentialNotePositions.Length;
                    m_SequentialLineRenderer.SetPositions(m_SequentialNotePositions);
                }
            };
            m_MoveNote.reference.action.canceled += _ =>
            {
                if (m_Interactable != null)
                {
                    m_SequentialNotePositions = SystemControl.SetNotePositions(m_SequentialNotes);
                    if (m_SequentialNotes.Contains(m_Interactable))
                    {
                        m_SequentialNotePositions[m_SequentialNotes.ToList().IndexOf(m_Interactable)] =
                            m_Interactable.transform.position;
                    }
                    m_SequentialLineRenderer.positionCount = m_SequentialNotePositions.Length;
                    m_SequentialLineRenderer.SetPositions(m_SequentialNotePositions);
                }
            };

        }
        
        private IEnumerator PlayWaiter(OrderedSet<IXRHoverInteractable> notes, Vector3[] notePositions)
        {
            int i = 0;
            foreach (IXRHoverInteractable note in notes)
            {
                AudioSource noteAudio = (note as AudioInteractable).audioSource;
                SystemControl.ToggleAudio(noteAudio, note);
                float waitTime = 0.0f;
                if (i < notes.Count - 1)
                {
                    float distance = Vector3.Distance(notePositions[i], notePositions[i + 1]);
                    float unitLength = 0.3f;
                    float timePerUnit = 0.1f;

                    waitTime = distance / unitLength * timePerUnit;
                }
                i++;
                yield return new WaitForSeconds(waitTime);
            }
        }


        private void OnSelectEntered(SelectEnterEventArgs arg0)
        {
            m_XRIActions.FindActionMap("XRI LeftHand Locomotion").Disable();
            m_XRIActions.FindActionMap("XRI RightHand Locomotion").Disable();
        }

        private void OnSelectExited(SelectExitEventArgs arg0)
        {
            m_XRIActions.FindActionMap("XRI LeftHand Locomotion").Enable();
            m_XRIActions.FindActionMap("XRI RightHand Locomotion").Enable();
        }

      
        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            m_Interactable = args.interactableObject;
            if (m_Interactable == null)
            {
                return;
            }

            if (m_Interactable is AudioInteractable)
            {
                m_AudioSource = (m_Interactable as AudioInteractable).audioSource;
                m_AudioActionMap.Enable(); 
            }
        }

        private void OnHoverExited(HoverExitEventArgs arg0)
        {
            if (m_PitchBimanual.reference.action.inProgress)
            {
                return;
            }

            if (m_VolumeBimanual.reference.action.inProgress)
            {
                return;
            }

            if (m_Interactable is AudioInteractable)
            {
                m_SystemControlActions.FindActionMap("Audio").Disable();
            }
        }
        
        private void InitializeInputActionMaps()
        {
            m_AudioActionMap = m_SystemControlActions.FindActionMap("Audio");
            m_GeneralActionMap = m_SystemControlActions.FindActionMap("General");
            
            m_AudioActionMap.Disable();
            m_GeneralActionMap.Enable();
        }


        private void SetupInteractorEvents()
        {
            if (m_LeftRayInteractor != null)
            {
                m_LeftRayInteractor.hoverEntered.AddListener(OnHoverEntered);
                m_LeftRayInteractor.hoverExited.AddListener(OnHoverExited);
                m_LeftRayInteractor.selectEntered.AddListener(OnSelectEntered);
                m_LeftRayInteractor.selectExited.AddListener(OnSelectExited);
            }

            if (m_RightRayInteractor != null)
            {
                m_RightRayInteractor.hoverEntered.AddListener(OnHoverEntered);
                m_RightRayInteractor.hoverExited.AddListener(OnHoverExited);
                m_RightRayInteractor.selectEntered.AddListener(OnSelectEntered);
                m_RightRayInteractor.selectExited.AddListener(OnSelectExited);
            }
        }

        private void TeardownInteractorEvents()
        {
            if (m_LeftRayInteractor != null)
            {
                m_LeftRayInteractor.hoverEntered.RemoveListener(OnHoverEntered);
                m_LeftRayInteractor.hoverExited.RemoveListener(OnHoverExited);
                m_LeftRayInteractor.selectEntered.RemoveListener(OnSelectEntered);
                m_LeftRayInteractor.selectExited.RemoveListener(OnSelectExited);
            }

            if (m_RightRayInteractor != null)
            {
                m_RightRayInteractor.hoverEntered.RemoveListener(OnHoverEntered);
                m_RightRayInteractor.hoverExited.RemoveListener(OnHoverExited);
                m_RightRayInteractor.selectEntered.RemoveListener(OnSelectEntered);
                m_RightRayInteractor.selectExited.RemoveListener(OnSelectExited);
            }
        }

        private void SetupLineRenderers()
        {
            m_BimanualLineRenderer = m_BimanualLine.GetComponent<LineRenderer>();
            m_BimanualLineRenderer.startWidth = 0.02f;
            m_BimanualLineRenderer.endWidth = 0.02f;
            m_BimanualLineRenderer.positionCount = 2;
            m_BimanualLineRenderer.useWorldSpace = true;
            
            m_SequentialLineRenderer = m_SequentialLine.GetComponent<LineRenderer>();
            m_SequentialLineRenderer.startWidth = 0.1f;
            m_SequentialLineRenderer.endWidth = 0.01f;
            m_SequentialLineRenderer.positionCount = 2;
            m_SequentialLineRenderer.useWorldSpace = true;
        }
    }
}