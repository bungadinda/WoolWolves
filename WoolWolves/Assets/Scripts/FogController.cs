using UnityEngine;

public class FogController : MonoBehaviour
{
    public Transform playerTransform; // Drag player here
    public ParticleSystem fogParticleSystem; // Drag your Particle System here
    private Material fogMaterial;

    void Start()
    {
        // Ambil material dari Particle System
        fogMaterial = fogParticleSystem.GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Update posisi pemain dan radius di shader
        if (fogMaterial != null)
        {
            fogMaterial.SetVector("_PlayerPos", playerTransform.position);
            fogMaterial.SetFloat("_Radius", 10.0f); // Set radius sesuai kebutuhan
        }
    }
}
