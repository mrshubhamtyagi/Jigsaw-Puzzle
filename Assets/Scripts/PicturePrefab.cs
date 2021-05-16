using UnityEngine;
using UnityEngine.UI;

public class PicturePrefab : MonoBehaviour
{
    [Header("-----References-----")]
    [SerializeField] private Image border;
    [SerializeField] private RawImage picture;


    private static Image lastSelcetedBorder;

    void OnEnable()
    {
        border.enabled = false;
    }

    public void PictureClick()
    {
        if (lastSelcetedBorder)
            lastSelcetedBorder.enabled = false;

        GameManager.Instance.selectedPicture = (Texture2D)picture.mainTexture;
        lastSelcetedBorder = border;
        border.enabled = true;
    }

}
