using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickableSphere : MonoBehaviour
{
    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("Sphere hovered!");
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("Sphere clicked!");
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}
