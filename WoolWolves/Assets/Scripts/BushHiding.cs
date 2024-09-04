using UnityEngine;

public class BushHiding : MonoBehaviour
{
    private Material bushMaterial;
    private bool isHidden = false;

    private void Start()
    {
        // Ambil material dari bush
        bushMaterial = GetComponent<Renderer>().material;
    }

    public void ToggleHide(bool hide)
    {
        isHidden = hide;
        SetBushTransparency(isHidden);
        Debug.Log(isHidden ? "Player is now hiding in bush" : "Player is no longer hiding in bush");
    }

    private void SetBushTransparency(bool isHidden)
    {
        if (bushMaterial != null)
        {
            if (isHidden)
            {
                bushMaterial.SetFloat("_Surface", 1); // Transparent
                bushMaterial.SetOverrideTag("RenderType", "Transparent");
                bushMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                Color color = bushMaterial.color;
                color.a = 0.3f; // Set transparansi
                bushMaterial.color = color;
            }
            else
            {
                bushMaterial.SetFloat("_Surface", 0); // Opaque
                bushMaterial.SetOverrideTag("RenderType", "Opaque");
                bushMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                Color color = bushMaterial.color;
                color.a = 1f; // Kembalikan ke tidak transparan
                bushMaterial.color = color;
            }
        }
        else
        {
            Debug.LogError("Bush material is not assigned!");
        }
    }
}
