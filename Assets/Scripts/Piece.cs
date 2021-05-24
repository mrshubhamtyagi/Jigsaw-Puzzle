using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class Piece : MonoBehaviour
{
    public bool isPlaced = false;
    public Vector3 actualPosition;
    private SpriteRenderer spriteRenderer;


    private bool isDragging = false;
    private float distance;
    private Vector3 mouseStartPosition;
    private Vector3 spriteStartPosition;
    private Vector3 spriteDropPosition;


    private SortingGroup sortingGroup;
    private static int sortingOrder = 0;

    private void Awake()
    {
        sortingGroup = transform.GetComponent<SortingGroup>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GameManager.Instance.totalPieces++;
        GameManager.OnGameStart += Event_OnGameStart;
        GameManager.OnGameReset += Event_OnGameReset;
        GameManager.OnPieceSpread += Event_OnPieceSpread;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= Event_OnGameStart;
        GameManager.OnGameReset -= Event_OnGameReset;
        GameManager.OnPieceSpread -= Event_OnPieceSpread;
    }

    private void Event_OnGameStart(Texture2D _pic, bool _isReset)
    {
        if (!_isReset)
        {
            transform.localPosition = actualPosition;
        }

        isPlaced = false;
        GetComponent<BoxCollider2D>().enabled = false;
        distance = sortingOrder = sortingGroup.sortingOrder = 0;
        gameObject.name = $"Piece_{transform.GetSiblingIndex()}";
        spriteRenderer.sprite = Sprite.Create(_pic, new Rect(0.0f, 0.0f, _pic.width, _pic.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteRenderer.transform.localScale = GameManager.Instance.actualScale;
    }


    private void OnMouseDown()
    {
        if (isPlaced || !GameManager.Instance.hasGameStarted) return;

        isDragging = true;
        sortingGroup.sortingOrder = sortingOrder++;
        spriteStartPosition = transform.localPosition;
        mouseStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseStartPosition.z = 0;
    }

    private void OnMouseDrag()
    {
        if (isPlaced || !GameManager.Instance.hasGameStarted) return;


        if (isDragging)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseStartPosition;
            pos.z = 0;
            transform.localPosition = spriteStartPosition + pos;
        }
    }

    private void OnMouseUp()
    {
        if (isPlaced || !GameManager.Instance.hasGameStarted) return;


        isDragging = false;
        spriteDropPosition = transform.localPosition;

        if (CheckIfPlacedAtRightPosition())
        {
            isPlaced = true;
            transform.localPosition = actualPosition;
            sortingGroup.sortingOrder = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.piecesPlaced++;

            if (GameManager.Instance.piecesPlaced >= GameManager.Instance.totalPieces)
                UIManager.Instance.GameComplete();
        }
    }


    private bool CheckIfPlacedAtRightPosition()
    {
        distance = Vector3.Distance(spriteDropPosition, actualPosition);
        if (distance <= GameManager.Instance.snapRange)
            return true;
        else
            return false;
    }


    private void Event_OnGameReset()
    {
        transform.DOLocalMove(actualPosition, GameManager.Instance.animTime).SetEase(GameManager.Instance.easeType);
        GetComponent<BoxCollider2D>().enabled = false;
        distance = sortingOrder = sortingGroup.sortingOrder = 0;
        spriteRenderer.sprite = null;
        isPlaced = false;

    }

    private void Event_OnPieceSpread()
    {
        float _randX = Random.Range(GameManager.Instance.lowerBound.x, GameManager.Instance.upperBound.x);
        float _randY = Random.Range(GameManager.Instance.lowerBound.y, GameManager.Instance.upperBound.y);

        transform.DOLocalMove(new Vector3(_randX, _randY, 0), GameManager.Instance.animTime).SetEase(GameManager.Instance.easeType);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void PlacePieceToPosition()
    {
        isPlaced = true;
        transform.DOLocalMove(new Vector3(actualPosition.x, actualPosition.y, actualPosition.z), GameManager.Instance.animTime * 0.5f).SetEase(GameManager.Instance.easeType);
        sortingGroup.sortingOrder = 0;
        GetComponent<BoxCollider2D>().enabled = false;
        GameManager.Instance.piecesPlaced++;

        if (GameManager.Instance.piecesPlaced >= GameManager.Instance.totalPieces)
            UIManager.Instance.GameComplete();
    }
}
