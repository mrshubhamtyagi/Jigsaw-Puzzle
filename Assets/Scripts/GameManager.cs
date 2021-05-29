using System;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public bool hasGameStarted = false;


    [Header("-----Settings-----")]
    public string playstoreLink = "www.google.com";
    public string websiteLink = "www.srivaishnavagurukulam.com";
    public float startDelay = 2f;
    public float animTime = 1f;
    public float snapRange = 0.4f;
    public Ease easeType = Ease.InExpo;
    [Space(20)]
    public Texture2D selectedPicture;
    //public Vector2 actualScale = Vector2.one;
    public Vector2 lowerBound_4x4;
    public Vector2 upperBound_4x4;
    [Space(20)]
    public Vector2 lowerBound_6x6;
    public Vector2 upperBound_6x6;

    [Header("-----References-----")]
    public GameObject puzzleParent;
    public GameObject puzzleParent_4x4;
    public GameObject puzzleParent_6x6;
    public SpriteRenderer picBlueprint_4x4;
    public SpriteRenderer picBlueprint_6x6;

    public int totalPieces;
    public int piecesPlaced;

    #region Events
    public static event Action<Texture2D, bool> OnGameStart;
    public void Event_OnGameStart(Texture2D _pic, bool _isReset)
    {
        OnGameStart?.Invoke(_pic, _isReset);
    }

    public static event Action OnGameReset;
    public void Event_OnGameReset()
    {
        OnGameReset?.Invoke();
    }

    public static event Action OnPieceSpread;
    public void Event_OnPieceSpred()
    {
        OnPieceSpread?.Invoke();
    }
    #endregion


    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {

    }

    public void StartGame(bool _isReset)
    {
        piecesPlaced = 0;

        puzzleParent.SetActive(true);
        if (UIManager.Instance.difficulty == UIManager.Difficulty.Easy)
        {
            puzzleParent_6x6.SetActive(false);
            puzzleParent_4x4.SetActive(true);
            picBlueprint_4x4.sprite = Sprite.Create(selectedPicture, new Rect(0.0f, 0.0f, selectedPicture.width, selectedPicture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else
        {
            puzzleParent_4x4.SetActive(false);
            puzzleParent_6x6.SetActive(true);
            picBlueprint_6x6.sprite = Sprite.Create(selectedPicture, new Rect(0.0f, 0.0f, selectedPicture.width, selectedPicture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        Event_OnGameStart(selectedPicture, _isReset);


        Invoke("Event_OnPieceSpred", startDelay);

        hasGameStarted = false;
        Invoke("MakeGameStarted", startDelay + 1f);

    }

    private void MakeGameStarted()
    {
        hasGameStarted = true;
    }


    public void RestartGame()
    {
        Event_OnGameReset();
        Invoke("StartGame", 2f);
    }

}
