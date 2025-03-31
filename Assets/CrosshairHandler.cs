using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class CrosshairHandler : MonoBehaviour
{
    public VisualElement ui;
    public VisualElement crosshair;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        crosshair = ui.Q<VisualElement>("Crosshair");
        crosshair.RemoveFromClassList("filled");
    }

    public void ActivaeIndicator(bool activate)
    {
        if (activate)
        {
            crosshair.AddToClassList("filled");
        }
        else
        {
            crosshair.RemoveFromClassList("filled");
        }
    }
}
