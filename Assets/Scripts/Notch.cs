using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Notch : XRSocketInteractor
{
    private Puller puller = null;
    private Arrow currentArrow = null;

    protected override void Awake()
    {
        base.Awake();
        puller = GetComponent<Puller>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        puller.onSelectExited.AddListener(TryToReleaseArrow);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        puller.onSelectExited.RemoveListener(TryToReleaseArrow); //onSelectEntered
    }

    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        //base.OnSelectEntering(interactable);
        base.OnSelectEntered(interactable);
        StoreArrow(interactable);
    }

    private void StoreArrow(XRBaseInteractable interactable)
    {
        if (interactable is Arrow arrow)
            currentArrow = arrow;
    }

    private void TryToReleaseArrow(XRBaseInteractor interactor)
    {
        if (currentArrow)
        {
            ForceDeselect();
            ReleaseArrow();
        }
    }

    private void ForceDeselect()
    {

        base.OnSelectExiting(currentArrow);
        base.OnSelectExited(currentArrow);

        currentArrow.OnSelectExiting(this);
        currentArrow.OnSelectExited(this);

        // base.OnSelectExited(currentArrow);
        //currentArrow.OnSelectExited(this);
    }

    private void ReleaseArrow()
    {
        currentArrow.Release(puller.PullAmount);
        currentArrow = null;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }
}
