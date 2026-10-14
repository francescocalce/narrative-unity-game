using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InterazioneQuadro : MonoBehaviour
{
    public GameObject testoInterazione;
    public Camera targetCamera;
    public float distanzaMassima = 5f;
    public GameObject puzzleCanvas;
    public GameObject quadroRisolto;
    public GameObject canvasIntro;
    
    [Header("Popup Anomalia e Narrazione")]
    public GameObject pannelloAnomaliaPuzzle;
    public Button bottoneProcediPuzzle;
    
    public GameObject[] pannelliNarrazione; // Array di 5 pannelli
    public Button[] bottoniAvanti; // Array di 5 bottoni
    
    [Header("Audio Narrazione")]
    public AudioClip[] audioNarrazione; // Array di 5 audio
    
    private AudioSource audioSource;
    private bool puzzleCompletato = false;
    private bool anomaliaMostrata = false;
    private int narrazioneCorrente = -1; // -1 = non iniziata

    [Header("Player")]
    public PlayerController playerController;

    void Start()
    {
        // Crea AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        // Collega bottone procedi puzzle
        if (bottoneProcediPuzzle != null)
            bottoneProcediPuzzle.onClick.AddListener(IniziaNarrazione);
        
        // Collega bottoni avanti (anche se useremo 'T')
        for (int i = 0; i < bottoniAvanti.Length; i++)
        {
            int index = i; // Cattura l'indice per il lambda
            if (bottoniAvanti[i] != null)
                bottoniAvanti[i].onClick.AddListener(() => AvantiNarrazione());
        }
    }

    void Update()
    {
        if (puzzleCompletato) return;
        
        // Raycast per interazione
        bool staGuardando = false;
        
        if (targetCamera != null && !anomaliaMostrata)
        {
            Ray ray = targetCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, distanzaMassima))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    staGuardando = true;
                }
            }
        }
        
        if (staGuardando && !anomaliaMostrata)
        {
            testoInterazione.SetActive(true);
            
            if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
            {
                MostraAnomaliaPuzzle();
            }
        }
        else if (!anomaliaMostrata)
        {
            testoInterazione.SetActive(false);
        }
        
        // Tasto 'T' per avanzare nella narrazione
        if (narrazioneCorrente >= 0 && narrazioneCorrente < 5)
        {
            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            {
                AvantiNarrazione();
            }
        }
    }
    
    void MostraAnomaliaPuzzle()
    {
        anomaliaMostrata = true;
        testoInterazione.SetActive(false);
        
        if (pannelloAnomaliaPuzzle != null)
            pannelloAnomaliaPuzzle.SetActive(true);
        
    // Disabilita movimento player
    if (playerController != null)
        playerController.enabled = false;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    void IniziaNarrazione()
    {
        // Nascondi popup anomalia
        if (pannelloAnomaliaPuzzle != null)
            pannelloAnomaliaPuzzle.SetActive(false);
        
        narrazioneCorrente = 0;
        MostraNarrazione(0);
    }
    
    void MostraNarrazione(int index)
    {
        // Nascondi tutti i pannelli
        foreach (GameObject pannello in pannelliNarrazione)
        {
            if (pannello != null)
                pannello.SetActive(false);
        }
        
        // Mostra il pannello corrente
        if (index >= 0 && index < pannelliNarrazione.Length)
        {
            if (pannelliNarrazione[index] != null)
                pannelliNarrazione[index].SetActive(true);
            
            // Fai partire l'audio
            if (index < audioNarrazione.Length && audioNarrazione[index] != null)
            {
                audioSource.PlayOneShot(audioNarrazione[index]);
                Debug.Log($"Audio narration-{index + 1} avviato");
            }
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    void AvantiNarrazione()
    {
        narrazioneCorrente++;
        
        if (narrazioneCorrente < 5)
        {
            MostraNarrazione(narrazioneCorrente);
        }
        else
        {
            // Finita la narrazione, avvia puzzle
            ApriPuzzle();
        }
    }
    
    void ApriPuzzle()
    {
        // Nascondi tutti i pannelli narrazione
        foreach (GameObject pannello in pannelliNarrazione)
        {
            if (pannello != null)
                pannello.SetActive(false);
        }
        
        puzzleCanvas.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Debug.Log("Puzzle aperto!");
    }
    
    public void PuzzleCompletato()
    {
        puzzleCompletato = true;
        gameObject.SetActive(false);
        
        if (quadroRisolto != null)
            quadroRisolto.SetActive(true);
        
        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(false);
        
        if (canvasIntro != null)
            canvasIntro.SetActive(true);
            
    // Riabilita movimento player
    if (playerController != null)
        playerController.enabled = true;
    
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Puzzle completato!");
    }
}