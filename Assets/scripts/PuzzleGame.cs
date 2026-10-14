using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PuzzleGame : MonoBehaviour
{
    [Header("Impostazioni Puzzle")]
    public Texture2D puzzleImage;
    public int rows = 4;
    public int cols = 4;
    
    [Header("UI References")]
    public RectTransform puzzleBoard;
    public RectTransform piecesPanel;
    public Image referenceImage;
    public TextMeshProUGUI piecesCountText;
    public TextMeshProUGUI movesCountText;
    public GameObject winPanel;
    public TextMeshProUGUI finalMovesText;
    public Button avantiButton;
    public ProgressManager progressManager;
    
    [Header("Suoni")]
    public AudioClip snapSound;
    public AudioClip wrongSound;
    
    [Header("Riferimenti")]
    public InterazioneQuadro interazioneQuadro;
    
    private AudioSource audioSource;
    private GameObject[,] boardCells;
    private List<PuzzlePiece> pieces;
    private int[] board;
    private int moves = 0;
    private int placedPieces = 0;
    
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        board = new int[rows * cols];
        for (int i = 0; i < board.Length; i++) board[i] = -1;
        
        InitializePuzzle();
    }
    
    void InitializePuzzle()
    {
        // Riferimento
        referenceImage.sprite = Sprite.Create(
            puzzleImage,
            new Rect(0, 0, puzzleImage.width, puzzleImage.height),
            new Vector2(0.5f, 0.5f)
        );
        
        // Crea board
        CreateBoard();
        
        // Crea pezzi
        CreatePieces();
        
        // Nasconde win panel
        winPanel.SetActive(false);
        
        UpdateUI();
    }
    
    void CreateBoard()
    {
        boardCells = new GameObject[rows, cols];
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject cell = new GameObject($"Cell_{row}_{col}");
                cell.transform.SetParent(puzzleBoard, false);
                
                Image img = cell.AddComponent<Image>();
                img.color = new Color(0.04f, 0.12f, 0.24f, 0.3f);
                
                // Drop zone component
                PuzzleDropZone dropZone = cell.AddComponent<PuzzleDropZone>();
                dropZone.cellIndex = row * cols + col;
                dropZone.puzzleGame = this;
                
                boardCells[row, col] = cell;
            }
        }
    }
    
    void CreatePieces()
    {
        pieces = new List<PuzzlePiece>();
        int pieceW = puzzleImage.width / cols;
        int pieceH = puzzleImage.height / rows;
        
        // Crea lista indici e mischia
        List<int> indices = Enumerable.Range(0, rows * cols).ToList();
        ShuffleList(indices);
        
        foreach (int pieceIndex in indices)
        {
            int row = pieceIndex / cols;
            int col = pieceIndex % cols;
            
            // Crea sprite del pezzo
            Rect pieceRect = new Rect(col * pieceW, (rows - 1 - row) * pieceH, pieceW, pieceH);
            Sprite pieceSprite = Sprite.Create(puzzleImage, pieceRect, new Vector2(0.5f, 0.5f));
            
            // Crea GameObject
            GameObject pieceObj = new GameObject($"Piece_{pieceIndex}");
            pieceObj.transform.SetParent(piecesPanel, false);
            
            Image img = pieceObj.AddComponent<Image>();
            img.sprite = pieceSprite;
            
            PuzzlePiece piece = pieceObj.AddComponent<PuzzlePiece>();
            piece.pieceIndex = pieceIndex;
            piece.puzzleGame = this;
            
            pieces.Add(piece);
        }
    }
    
public void PlacePiece(int pieceIndex, int cellIndex)
{
    // Verifica se cella occupata
    if (board[cellIndex] != -1) return;
    
    // Verifica se pezzo corretto
    if (pieceIndex != cellIndex)
    {
        // Pezzo sbagliato
        PlaySound(wrongSound);
        FlashCell(cellIndex, Color.red);
        return;
    }
    
    // Pezzo corretto!
    board[cellIndex] = pieceIndex;
    moves++;
    placedPieces++;
    
    PlaySound(snapSound);
    
    // Visualizza pezzo nella board
    int row = cellIndex / cols;
    int col = cellIndex % cols;
    GameObject cell = boardCells[row, col];
    
    // Trova il pezzo e RICREA lo sprite per la griglia
    PuzzlePiece piece = pieces.Find(p => p.pieceIndex == pieceIndex);
    if (piece != null)
    {
        // RICREA lo sprite invece di riutilizzare (usa nomi diversi)
        int pieceRow = pieceIndex / cols;
        int pieceCol = pieceIndex % cols;
        int pieceW = puzzleImage.width / cols;
        int pieceH = puzzleImage.height / rows;
        
        Rect pieceRect = new Rect(pieceCol * pieceW, (rows - 1 - pieceRow) * pieceH, pieceW, pieceH);
        Sprite newSprite = Sprite.Create(puzzleImage, pieceRect, new Vector2(0.5f, 0.5f));
        
        Image cellImg = cell.GetComponent<Image>();
        cellImg.sprite = newSprite;
        cellImg.color = Color.white; // Bianco
        cellImg.material = null; // Nessun materiale
        cellImg.preserveAspect = true;
        cellImg.raycastTarget = false;
        
        // DEBUG
        Debug.Log($"Pezzo {pieceIndex} inserito. Sprite: {cellImg.sprite != null}, Color: {cellImg.color}, Active: {cell.activeSelf}");

        piece.MarkAsUsed(); 
    }
    
// DEBUG prima di UpdateUI
Debug.Log($"PRIMA di UpdateUI - Color: {cell.GetComponent<Image>().color}");

UpdateUI();

// DEBUG dopo UpdateUI  
Debug.Log($"DOPO UpdateUI - Color: {cell.GetComponent<Image>().color}");
    
    if (placedPieces == rows * cols)
    {
        Win();
    }
}
    
    void FlashCell(int cellIndex, Color color)
    {
        int row = cellIndex / cols;
        int col = cellIndex % cols;
        GameObject cell = boardCells[row, col];
        Image img = cell.GetComponent<Image>();
        
        Color original = img.color;
        img.color = color;
        
        Invoke(nameof(ResetCellColor), 0.4f);
        
        void ResetCellColor()
        {
            img.color = original;
        }
    }
    
    void UpdateUI()
    {
        piecesCountText.text = $"Pezzi: <b>{placedPieces}</b> / {rows * cols}";
        movesCountText.text = $"Mosse: <b>{moves}</b>";
    }
    
    void Win()
    {
        finalMovesText.text = $"Hai ricomposto il dipinto in {moves} mosse.";
        winPanel.SetActive(true);
        
        // Cambia testo bottone in "Avanti"
        avantiButton.GetComponentInChildren<TextMeshProUGUI>().text = "AVANTI";
        avantiButton.onClick.RemoveAllListeners();
        avantiButton.onClick.AddListener(PuzzleCompletato);
    }
    
    void PuzzleCompletato()
    {
        if (interazioneQuadro != null)
        {
            interazioneQuadro.PuzzleCompletato();
        }
        
        // AGGIUNGI QUESTA PARTE:
        // Completa la barra di progresso
        ProgressManager pm = FindObjectOfType<ProgressManager>();
        if (pm != null)
        {
            pm.CompleteStep("Puzzle completato");
        }
        
        // Chiude il puzzle
        gameObject.SetActive(false);
    }
    
    public void Shuffle()
    {
        // Reset
        Reset();
    }
    
    public void Solve()
    {
        for (int i = 0; i < rows * cols; i++)
        {
            if (board[i] == -1)
            {
                PlacePiece(i, i);
            }
        }
    }
    
    void Reset()
    {
        moves = 0;
        placedPieces = 0;
        for (int i = 0; i < board.Length; i++) board[i] = -1;
        
        // Pulisci board
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Image img = boardCells[row, col].GetComponent<Image>();
                img.sprite = null;
                img.color = new Color(0.04f, 0.12f, 0.24f, 0.3f);
            }
        }
        
        // Reset pezzi
        foreach (var piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();
        
        CreatePieces();
        UpdateUI();
        winPanel.SetActive(false);
    }
    
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}