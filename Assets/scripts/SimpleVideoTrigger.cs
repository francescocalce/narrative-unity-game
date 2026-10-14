using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SimplVideoTrigger : MonoBehaviour
{
    public GameObject testoInterazione;
    public VideoPlayer videoPlayer;
    public Camera targetCamera;
    public float distanzaMassima = 5f;
    public GestoreDialogo gestoreDialogo;
    public GameObject canvasIntro;
    public GameObject videoDisplay;
    public ProgressManager progressManager;
    
    [Header("UI Nodo e Anomalia")]
    public GameObject pannelloNodo;
    public GameObject pannelloAnomalia;
    public Button bottoneProcedi;
    
    private bool videoAttivo = false;
    private float tempoAvvio = 0f;
    private bool nodoMostrato = false;
    private bool anomaliaMostrata = false;

    [Header("Player")]
    public PlayerController playerController;

    void Start()
    {
        if (targetCamera != null)
            videoPlayer.targetCamera = targetCamera;
        
        // Collega il bottone
        if (bottoneProcedi != null)
            bottoneProcedi.onClick.AddListener(AvviaVideo);
    }

    void Update()
    {
        bool staGuardandoSfera = false;
        if (targetCamera != null && !videoAttivo && !nodoMostrato)
        {
            Ray ray = targetCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanzaMassima))
                if (hit.collider.gameObject == gameObject)
                    staGuardandoSfera = true;
        }
        
        if (staGuardandoSfera && !videoAttivo && !nodoMostrato)
        {
            testoInterazione.SetActive(true);
            if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
            {
                MostraNodo();
            }
        }
        else if (!nodoMostrato)
            testoInterazione.SetActive(false);
        
        // Aspetta SPAZIO per passare al popup anomalia
        if (nodoMostrato && !anomaliaMostrata && Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            MostraAnomalia();
        }
        
        // Gestione fine video
        if (videoAttivo && !videoPlayer.isPlaying && videoPlayer.time > 0)
        {
            TerminaVideo();
        }

        // Skip video con X
        if (videoAttivo && Time.time - tempoAvvio > 1f && Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            TerminaVideo();
        }
    }
    
    void MostraNodo()
    {
        nodoMostrato = true;
        testoInterazione.SetActive(false);
        
        if (pannelloNodo != null)
            pannelloNodo.SetActive(true);

                
        // Disabilita movimento player
        if (playerController != null)
            playerController.enabled = false;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void MostraAnomalia()
    {
        anomaliaMostrata = true;
        
        if (pannelloNodo != null)
            pannelloNodo.SetActive(false);
        
        if (pannelloAnomalia != null)
            pannelloAnomalia.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    void AvviaVideo()
    {
        videoAttivo = true;
        tempoAvvio = Time.time;
        
        if (pannelloAnomalia != null)
            pannelloAnomalia.SetActive(false);
        
        if (canvasIntro != null)
            canvasIntro.SetActive(false);
        
        if (videoDisplay != null)
            videoDisplay.SetActive(true);
        
        videoPlayer.Play();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void TerminaVideo()
    {
        videoAttivo = false;
        videoPlayer.Stop();
        
        if (videoDisplay != null)
            videoDisplay.SetActive(false);
        
        if (canvasIntro != null)
            canvasIntro.SetActive(true);
        
        if (progressManager != null)
            progressManager.CompleteStep("Video completato");
        
        gestoreDialogo.MostraDialogoConTestiAttuali();
    }
}