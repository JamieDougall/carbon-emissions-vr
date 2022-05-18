using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSelectiveSocket : XRSocketInteractor
{
    public SelectMode mode = SelectMode.Tag;
    public string targetTag = string.Empty;
    public List<GameObject> targetInteractables;

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        switch (mode)
        {
            case SelectMode.Tag:
                return base.CanHover(interactable) && MatchUsingTag(interactable);
            case SelectMode.Interactables:
                return base.CanHover(interactable) && MatchUsingInteractables(interactable);
            default:
                return false;
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {

        switch (mode)
        {
            case SelectMode.Tag:
                return base.CanSelect(interactable) && MatchUsingTag(interactable);
            case SelectMode.Interactables:
                return base.CanSelect(interactable) && MatchUsingInteractables(interactable);
            default:
                return false;
        }
    }

    private bool MatchUsingTag(IXRInteractable interactable)
    {
        return interactable.transform.CompareTag(targetTag);
    }

    private bool MatchUsingInteractables(IXRInteractable interactable)
    {
        return targetInteractables.Contains(interactable.transform.gameObject);
        //return interactable == targetInteractable.GetComponent<IXRInteractable>();
    }

    [System.Serializable]
    public enum SelectMode
    {
        Tag,
        Interactables
    }
}
