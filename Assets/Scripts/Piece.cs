using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2 actualPosition;
    public Vector2 actualScale = Vector2.one;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        actualPosition = transform.localPosition;
        gameObject.name = $"Piece_{transform.GetSiblingIndex()}";
        spriteRenderer.sprite = GameManager.Instance.picture;
        spriteRenderer.transform.localScale = actualScale;
    }

    void Update()
    {

    }




}
