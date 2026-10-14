using UnityEngine;

public class AperturaWebView : MonoBehaviour
{
    private WebViewObject webViewObject;
    public InterazioneQuadro interazioneQuadro;

    void Start()
    {
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init(
            cb: (msg) => {
                Debug.Log($"Messaggio da WebView: {msg}");
                if (msg.Contains("PuzzleCompletato") && interazioneQuadro != null)
                {
                    interazioneQuadro.PuzzleCompletato();
                }
            },
            err: (msg) => {
                Debug.LogError($"Errore WebView: {msg}");
            },
            transparent: false,
            zoom: true,
            ua: ""
        );
        
        // Imposta margini per occupare tutto lo schermo
        webViewObject.SetMargins(0, 0, 0, 0);
        webViewObject.SetVisibility(false);
        
        Debug.Log("WebView inizializzato!");
    }

    public void ApriPuzzle()
    {
        if (webViewObject == null)
        {
            Debug.LogError("WebViewObject è null!");
            return;
        }
        
        string htmlPath = System.IO.Path.Combine(Application.streamingAssetsPath, "puzzle.html");
        Debug.Log("Path completo: " + htmlPath);
        
        // Verifica che il file esista
        if (!System.IO.File.Exists(htmlPath))
        {
            Debug.LogError("File puzzle.html NON trovato in: " + htmlPath);
            return;
        }
        
        webViewObject.LoadURL("file:///" + htmlPath.Replace("\\", "/"));
        webViewObject.SetVisibility(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Debug.Log("WebView visibilità impostata su TRUE");
    }

    public void ChiudiPuzzle()
    {
        if (webViewObject != null)
        {
            webViewObject.SetVisibility(false);
        }
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        Debug.Log("Puzzle chiuso!");
    }
}