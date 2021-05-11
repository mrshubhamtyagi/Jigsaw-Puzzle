using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Sprite picture;


    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {

    }

    void Update()
    {

    }
}
