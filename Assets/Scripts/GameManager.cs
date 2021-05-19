using System;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public bool hint = false;

    [Header("-----Settings-----")]
    public string playstoreLink = "www.google.com";
    public float startDelay = 2f;
    public float animTime = 1f;
    public float snapRange = 0.4f;
    public Ease easeType = Ease.InExpo;
    [Space(20)]
    public Texture2D selectedPicture;
    public Vector2 actualScale = Vector2.one;
    public Vector2 lowerBound;
    public Vector2 upperBound;

    [Header("-----References-----")]
    public GameObject puzzleParent;
    public Sprite picture;
    public SpriteRenderer picBlueprint;

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

    private void Update()
    {
        if (hint)
        {
            hint = false;
            UIManager.Instance.HintClick();
        }
    }


    public void StartGame(bool _isReset)
    {
        piecesPlaced = 0;
        puzzleParent.SetActive(true);
        picBlueprint.sprite = Sprite.Create(selectedPicture, new Rect(0.0f, 0.0f, selectedPicture.width, selectedPicture.height), new Vector2(0.5f, 0.5f), 100.0f);
        Event_OnGameStart(selectedPicture, _isReset);

        Invoke("Event_OnPieceSpred", startDelay);
    }


    public void RestartGame()
    {
        Event_OnGameReset();
        Invoke("StartGame", 2f);
    }

}
