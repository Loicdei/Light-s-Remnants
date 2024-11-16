using UnityEngine;

[ExecuteAlways]
public class PlatformResizer : MonoBehaviour
{
    public enum PlatformSize { Small = 16, Medium = 32, Large = 48 }
    public PlatformSize size = PlatformSize.Small;

    public Sprite smallSprite;
    public Sprite mediumSprite;
    public Sprite largeSprite;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();

        ResizePlatform();
    }

    private void ResizePlatform()
    {
        if (spriteRenderer == null || boxCollider == null) return;

        // Définir la texture en fonction de la taille
        switch (size)
        {
            case PlatformSize.Small:
                spriteRenderer.sprite = smallSprite;
                break;
            case PlatformSize.Medium:
                spriteRenderer.sprite = mediumSprite;
                break;
            case PlatformSize.Large:
                spriteRenderer.sprite = largeSprite;
                break;
        }

        // Ajuster la largeur en fonction de la taille
        float width = (float)size / 16;

        // Ajuster le BoxCollider
        boxCollider.size = new Vector2(width, 0.5f); // 0.5f pour 8px de hauteur
        boxCollider.offset = Vector2.zero; // centre sur le transform
    }
}
