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

    public void SetPicture(Texture2D _avatar)
    {
        picture.texture = _avatar;
    }


    public void PictureClick()
    {
        if (lastSelcetedBorder)
            lastSelcetedBorder.enabled = false;

        UIManager.Instance.playBtn.SetActive(true);

        GameManager.Instance.selectedPicture = (Texture2D)picture.mainTexture;
        lastSelcetedBorder = border;
        border.enabled = true;
    }

}
