using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class PlatformManager : MonoBehaviour
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
        // Évitez d'exécuter toute logique complexe ici
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();

        EditorApplication.delayCall += () =>
        {
            if (this != null) ResizePlatform(); // S'assure que la logique est appelée en dehors du pipeline de validation
        };
    }

    private void ResizePlatform()
    {
        if (spriteRenderer == null || boxCollider == null) return;

        // Ajuster la texture en fonction de la taille
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
        boxCollider.offset = Vector2.zero; // Le BoxCollider est centré sur le transform
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlatformManager))]
    public class PlatformManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PlatformManager platformManager = (PlatformManager)target;
            if (GUILayout.Button("Resize Platform"))
            {
                platformManager.ResizePlatform();
            }
        }
    }
#endif
}
