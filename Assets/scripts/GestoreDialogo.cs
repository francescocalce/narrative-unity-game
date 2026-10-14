using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GestoreDialogo : MonoBehaviour
{
    [Header("UI")]
    public GameObject pannelloDialogo;
    public TextMeshProUGUI testoDomanda1;
    public TextMeshProUGUI testoDomanda2;
    public Button bottone1;
    public Button bottone2;
    public Button bottone3;
    public GameObject oggettoQuadro; 
    
    [Header("UI Conseguenze")]
    public GameObject pannelloConseguenze;
    public TextMeshProUGUI testoConseguenze;
    public Button bottoneTornaEsplorazione;
    
    [Header("Sfera")]
    public GameObject sfera;
    
    [Header("Audio Conseguenze")]
    public AudioClip consequence1;
    public AudioClip consequence2;
    public AudioClip consequence3;

    [Header("Player")]
    public PlayerController playerController;
    
    private AudioSource audioSource;
    
    // Variabile per salvare la scelta
    public int ultimaRispostaScelta { get; private set; } = -1;
    public string ultimoTestoRisposta { get; private set; } = "";
    
    // Testi delle conseguenze
    private string[] testiConseguenze = new string[]
    {
        "", // Index 0 non usato
        "Il monastero diventa presidio e punto di riferimento nel territorio. Nel corso dei secoli la sua presenza consoliderà i confini e rafforzerà i legami con il potere civile.",
        "La comunità locale trova nella chiesa un luogo di speranza e protezione. Le monache divengono custodi non solo della fede, ma anche della memoria storica del luogo.",
        "Ogni pietra posa un mattone nella fondazione dell'identità locale. Il monastero diventa simbolo eterno, testimone silenzioso dei secoli che passano."
    };

    void Start()
    {
        // Crea AudioSource se non esiste
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        bottone1.onClick.AddListener(() => RispostaScelta(1));
        bottone2.onClick.AddListener(() => RispostaScelta(2));
        bottone3.onClick.AddListener(() => RispostaScelta(3));
        
        // Collega bottone conseguenze
        if (bottoneTornaEsplorazione != null)
            bottoneTornaEsplorazione.onClick.AddListener(ChiudiConseguenze);
    }

    public void MostraDialogo(string domanda1, string domanda2, string risp1, string risp2, string risp3)
    {
        sfera.SetActive(false);
        pannelloDialogo.SetActive(true);
        testoDomanda1.text = domanda1;
        testoDomanda2.text = domanda2;
        bottone1.GetComponentInChildren<TextMeshProUGUI>().text = risp1;
        bottone2.GetComponentInChildren<TextMeshProUGUI>().text = risp2;
        bottone3.GetComponentInChildren<TextMeshProUGUI>().text = risp3;
        
        // Disabilita movimento player
        if (playerController != null)
            playerController.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void RispostaScelta(int numeroRisposta)
    {
        ultimaRispostaScelta = numeroRisposta;
        
        if (numeroRisposta == 1)
            ultimoTestoRisposta = bottone1.GetComponentInChildren<TextMeshProUGUI>().text;
        else if (numeroRisposta == 2)
            ultimoTestoRisposta = bottone2.GetComponentInChildren<TextMeshProUGUI>().text;
        else if (numeroRisposta == 3)
            ultimoTestoRisposta = bottone3.GetComponentInChildren<TextMeshProUGUI>().text;
        
        Debug.Log($"Risposta scelta: {numeroRisposta} - Testo: '{ultimoTestoRisposta}'");
        
        // Fai partire l'audio corrispondente
        PlayConsequenceAudio(numeroRisposta);
        
        // Nascondi pannello dialogo
        pannelloDialogo.SetActive(false);
        
        // Mostra popup conseguenze
        MostraConseguenze(numeroRisposta);
        
        // NON chiudiamo cursore e NON gestiamo scelta ancora
        // Lo faremo quando chiude il popup
    }
    
    void PlayConsequenceAudio(int scelta)
    {
        AudioClip clip = null;
        
        switch(scelta)
        {
            case 1:
                clip = consequence1;
                break;
            case 2:
                clip = consequence2;
                break;
            case 3:
                clip = consequence3;
                break;
        }
        
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
            Debug.Log($"Audio consequence-{scelta} avviato");
        }
    }
    
    void MostraConseguenze(int scelta)
    {
        if (pannelloConseguenze != null && testoConseguenze != null)
        {
            pannelloConseguenze.SetActive(true);
            
            // Imposta il testo in base alla scelta
            if (scelta >= 1 && scelta <= 3)
                testoConseguenze.text = testiConseguenze[scelta];
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    void ChiudiConseguenze()
    {
        if (pannelloConseguenze != null)
            pannelloConseguenze.SetActive(false);
        
        // Riabilita movimento player
        if (playerController != null)
            playerController.enabled = true;

        // Ora blocca cursore e gestisci la scelta
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        GestisciScelta(ultimaRispostaScelta);
    }

    void GestisciScelta(int scelta)
    {
        switch(scelta)
        {
            case 1:
                Debug.Log("Appare l'oggetto blu!");
                oggettoQuadro.SetActive(true);
                break;
            case 2:
                Debug.Log("Scelta 2");
                oggettoQuadro.SetActive(true);
                break;
            case 3:
                Debug.Log("Scelta 3");
                oggettoQuadro.SetActive(true);
                break;
        }
    }
    
    public int OttieniUltimaScelta()
    {
        return ultimaRispostaScelta;
    }
    
    public void MostraDialogoConTestiAttuali()
    {
        sfera.SetActive(false);
        pannelloDialogo.SetActive(true);
        
        // Disabilita movimento player
        if (playerController != null)
            playerController.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}