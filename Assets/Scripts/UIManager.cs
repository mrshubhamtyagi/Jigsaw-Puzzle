using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float splashScreenHoldTime = 2f;
    public GameObject playBtn;
    public Transform container;
    public GameObject prefab;

    [Header("-----Boards-----")]
    public GameObject parent_4x4;
    public GameObject picBlueprint_4x4;
    public GameObject parent_6x6;
    public GameObject picBlueprint_6x6;

    [Header("-----Hint-----")]
    public int hintCountEasy = 1;
    public int hintCountHard = 3;
    public Button hintBtn;
    public bool showHint = true;


    [Header("-----Screens & Popups-----")]
    public GameObject splashScreen;
    public GameObject homeScreen;
    public GameObject gameScreen;
    public GameObject gameplayScreen;
    public GameObject gameCompletePopup;
    public GameObject exitPopup;

    [Header("-----Sounds-----")]
    public Image musicImageHomeScreen;
    public Image musicImageGameScreen;
    public Sprite musicONSprite;
    public Sprite musicOFFSprite;
    public AudioSource music;
    public AudioSource click;
    public AudioSource happy;


    [Header("-----Eye-----")]
    public Image eyeImage;
    public Sprite eyeON;
    public Sprite eyeOFF;


    [Header("-----Avatars-----")]
    public List<Texture2D> avatarList;

    private bool isMusicOn = true;



    public enum Difficulty
    {
        Easy,
        Hard
    }
    public Difficulty difficulty = Difficulty.Easy;



    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!showHint)
        {
            hintBtn.interactable = false;
            hintBtn.gameObject.SetActive(false);
        }
        exitPopup.SetActive(false);
        splashScreen.SetActive(true);

        parent_4x4.SetActive(false);
        parent_6x6.SetActive(false);

        Init();
        Invoke("LoadHomeScreen", splashScreenHoldTime);
    }


    private void Init()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);


        foreach (var avatar in avatarList)
        {
            Instantiate(prefab, container).GetComponent<PicturePrefab>().SetPicture(avatar);
        }
    }



    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && homeScreen.activeInHierarchy && !gameScreen.activeInHierarchy)
            exitPopup.SetActive(true);
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    private void LoadHomeScreen()
    {
        splashScreen.SetActive(false);

        if (isMusicOn)
            music.Play();

        playBtn.SetActive(false);
        homeScreen.SetActive(true);
        gameScreen.SetActive(false);
        gameplayScreen.SetActive(false);
    }

    public void Music_Click()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            musicImageHomeScreen.sprite = musicONSprite;
            musicImageGameScreen.sprite = musicONSprite;
            if (music)
                music.Play();
        }
        else
        {
            musicImageHomeScreen.sprite = musicOFFSprite;
            musicImageGameScreen.sprite = musicOFFSprite;
            if (music)
                music.Stop();
        }
    }

    public void Share_Click()
    {
        if (!GameManager.Instance.hasGameStarted)
            return;

        click.Play();
        NativeShare nativeShare = new NativeShare();
        nativeShare.SetTitle("Virtue Dasavatharam Puzzle Game").SetText($"Download Virtue Dasavatharam Puzzle game from the link below/n/n{GameManager.Instance.playstoreLink}").Share();
    }


    public void StartGamePlay(int _difficulty)
    {
        difficulty = (Difficulty)_difficulty;

        click.Play();
        homeScreen.SetActive(false);
        gameScreen.SetActive(true);
        picBlueprint_4x4.SetActive(false);
        picBlueprint_6x6.SetActive(false);
        hintCountEasy = 1;
        hintCountHard = 3;
        if (showHint)
        {
            hintBtn.interactable = true;
            hintBtn.gameObject.SetActive(true);
        }
        eyeImage.sprite = eyeON;
        GameManager.Instance.StartGame(false);
    }


    public void PlayClickSound()
    {
        if (click)
            click.Play();
    }

    public void Restart_Click()
    {
        if (!GameManager.Instance.hasGameStarted)
            return;

        click.Play();
        GameManager.Instance.Event_OnGameReset();
        GameManager.Instance.StartGame(true);
    }


    public void Home_Click()
    {
        click.Play();
        GameManager.Instance.selectedPicture = null;
        LoadHomeScreen();
        GameManager.Instance.totalPieces = 0;

        parent_4x4.SetActive(false);
        parent_6x6.SetActive(false);

        if (isMusicOn)
            music.Play();
    }

    public void EyeClick()
    {
        if (!GameManager.Instance.hasGameStarted)
            return;

        click.Play();

        GameObject pictureBlueprint = difficulty == Difficulty.Easy ? picBlueprint_4x4 : picBlueprint_6x6;
        pictureBlueprint.SetActive(!pictureBlueprint.activeInHierarchy);
        eyeImage.sprite = pictureBlueprint.activeInHierarchy ? eyeOFF : eyeON;
    }

    public void GameComplete()
    {
        print("Game Complete");
        gameCompletePopup.SetActive(true);
        music.Stop();
        happy.Play();
        GameManager.Instance.piecesPlaced = 0;
    }

    public void HintClick()
    {
        if (!GameManager.Instance.hasGameStarted)
            return;

        Transform _puzzle = null;

        if (difficulty == Difficulty.Easy)
        {
            _puzzle = parent_4x4.transform.GetChild(2);
            if (--hintCountEasy <= 0)
                hintBtn.interactable = false;
        }
        else
        {
            _puzzle = parent_6x6.transform.GetChild(2);
            if (--hintCountHard <= 0)
                hintBtn.interactable = false;
        }


        if (GameManager.Instance.piecesPlaced <= GameManager.Instance.totalPieces)
        {
            for (int i = 1000; i > 0; i--)
            {
                int _childIndex = Random.Range(0, _puzzle.childCount - 1);
                if (!_puzzle.GetChild(_childIndex).GetComponent<Piece>().isPlaced)
                {
                    _puzzle.GetChild(_childIndex).GetComponent<Piece>().PlacePieceToPosition();
                    return;
                }
            }
        }
    }


    public void OpenURL()
    {
        Application.OpenURL(GameManager.Instance.websiteLink);
    }
}
