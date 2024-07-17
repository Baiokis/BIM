using UnityEngine;
using UnityEngine.UI;

public class OpacityController : MonoBehaviour
{
    public Slider opacitySlider;
    public Renderer objectRenderer;

    private Material objectMaterial;

    void Start()
    {
        // Certifique-se de que o Slider e o Renderer estão atribuídos
        if (opacitySlider != null && objectRenderer != null)
        {
            objectMaterial = objectRenderer.material;
            opacitySlider.onValueChanged.AddListener(UpdateOpacity);
        }
    }

    void UpdateOpacity(float value)
    {
        if (objectMaterial != null)
        {
            Color color = objectMaterial.color;
            color.a = value;
            objectMaterial.color = color;
        }
    }
}
