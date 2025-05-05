using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public Sprite hiddenIcon;
    public Sprite icon;

    public bool isClicked;

    public GameManager gameManager;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnCardClicked()
    {
        gameManager.SetSelected(this);
    }

    public void SetIconSprite(Sprite sp)
    {
        icon = sp;
    }

    public void Show()
    {
        iconImage.sprite = icon;
        isClicked = true;
    }

    public void Hide()
    {
        iconImage.sprite = hiddenIcon;
        isClicked = false;
    }
}
