﻿using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject playBtn;

    [Header("-----Screens-----")]
    public GameObject homeScreen;
    public GameObject gameScreen;
    public GameObject gameplayScreen;
    public GameObject gameCompleteScreen;

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
        LoadHomeScreen();
    }


    private void LoadHomeScreen()
    {
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
        click.Play();
        NativeShare nativeShare = new NativeShare();
        nativeShare.SetTitle("Download <b>Virtue Dasavatharam Puzzle</b> game from the fiven link.").SetText(GameManager.Instance.playstoreLink).Share();
    }


    public void StartGamePlay()
    {
        click.Play();
        homeScreen.SetActive(false);
        gameScreen.SetActive(true);
        pictureBlueprint.SetActive(false);
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
        click.Play();
        GameManager.Instance.Event_OnGameReset();
        GameManager.Instance.StartGame(true);
    }


    public void Home_Click()
    {
        click.Play();
        GameManager.Instance.selectedPicture = null;
        LoadHomeScreen();
        GameManager.totalPieces = 0;

        if (isMusicOn)
            music.Play();
    }

    public void EyeClick()
    {
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
    }
}
