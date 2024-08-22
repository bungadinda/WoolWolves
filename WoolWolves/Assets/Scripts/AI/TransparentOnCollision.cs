using UnityEngine;

public class TransparentOnCollision : MonoBehaviour
{
    private Material originalMaterial;
    private Color originalColor;
    private string originalRenderQueue;

    void Start()
    {
        // Mendapatkan material asli dari GameObject
        originalMaterial = GetComponent<Renderer>().material;
        originalColor = originalMaterial.color;
        originalRenderQueue = originalMaterial.GetTag("RenderType", true);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Memeriksa apakah objek yang bertabrakan memiliki tag "Player"
        if (collision.gameObject.tag == "Player")
        {
            // Mengubah material menjadi transparan 25%
            SetMaterialTransparency(true, 0.25f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Mengembalikan material ke kondisi aslinya ketika tabrakan berakhir
        if (collision.gameObject.tag == "Player")
        {
            SetMaterialTransparency(false, originalColor.a);
        }
    }

    private void SetMaterialTransparency(bool transparent, float alpha)
    {
        if (transparent)
        {
            originalMaterial.SetOverrideTag("RenderType", "Transparent");
            originalMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            originalMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            originalMaterial.SetInt("_ZWrite", 0);
            originalMaterial.DisableKeyword("_ALPHATEST_ON");
            originalMaterial.EnableKeyword("_ALPHABLEND_ON");
            originalMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            originalMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        else
        {
            originalMaterial.SetOverrideTag("RenderType", originalRenderQueue);
            originalMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            originalMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            originalMaterial.SetInt("_ZWrite", 1);
            originalMaterial.DisableKeyword("_ALPHABLEND_ON");
            originalMaterial.renderQueue = -1; // Reset to default render queue
        }

        Color newColor = originalMaterial.color;
        newColor.a = alpha;
        originalMaterial.color = newColor;
    }
}
