using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float splashScreenHoldTime = 2f;
    public GameObject playBtn;
    public Transform container;
    public GameObject prefab;

    [Header("-----Avatars-----")]
    public List<Texture2D> avatarList;

    [Header("-----Hint-----")]
    public int hintCount = 2;
    public Button hintBtn;
    public bool showHint = false;


    [Header("-----Screens-----")]
    public GameObject splashScreen;
    public GameObject homeScreen;
    public GameObject gameScreen;
    public GameObject gameplayScreen;
    public GameObject gameCompleteScreen;
    public GameObject exitScreen;

    [Header("-----Sounds-----")]
    public Image musicImageHomeScreen;
    public Image musicImageGameScreen;
    public Sprite musicONSprite;
    public Sprite musicOFFSprite;
    public AudioSource music;
    public AudioSource click;
    public AudioSource happy;


    [Header("-----Eye-----")]
    public GameObject pictureBlueprint;
    public Image eyeImage;
    public Sprite eyeON;
    public Sprite eyeOFF;


    private bool isMusicOn = true;

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
        exitScreen.SetActive(false);
        splashScreen.SetActive(true);

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
            exitScreen.SetActive(true);
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


    public void StartGamePlay()
    {
        click.Play();
        homeScreen.SetActive(false);
        gameScreen.SetActive(true);
        pictureBlueprint.SetActive(false);
        hintCount = 2;
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

        if (isMusicOn)
            music.Play();
    }

    public void EyeClick()
    {
        if (!GameManager.Instance.hasGameStarted)
            return;

        click.Play();
        pictureBlueprint.SetActive(!pictureBlueprint.activeInHierarchy);
        eyeImage.sprite = pictureBlueprint.activeInHierarchy ? eyeOFF : eyeON;
    }

    public void GameComplete()
    {
        print("Game Complete");
        gameCompleteScreen.SetActive(true);
        music.Stop();
        happy.Play();
        GameManager.Instance.piecesPlaced = 0;
    }

    public void HintClick()
    {
        if (!GameManager.Instance.hasGameStarted)
            return;

        if (--hintCount <= 0)
            hintBtn.interactable = false;

        if (GameManager.Instance.piecesPlaced <= GameManager.Instance.totalPieces)
        {
            Transform _puzzle = gameplayScreen.transform.GetChild(3);

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
}
