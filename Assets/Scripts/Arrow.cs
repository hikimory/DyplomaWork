using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : XRGrabInteractable
{
    public float speed = 1000.0f;

    public Transform tip = null;

    private bool inAir = false;

    private Vector3 lastPosition = Vector3.zero;

    private Rigidbody rigidBody = null;

    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (inAir)
        {
            CheckForCollision();
            lastPosition = tip.position;
        }
    }

    private void CheckForCollision()
    {
        if (Physics.Linecast(lastPosition, tip.position))
            Stop();
    }

    private void Stop()
    {
        inAir = false;
        SetPhysics(false);
        Destroy(gameObject, 2f);
    }

    public void Release(float pullValue)
    {
        inAir = true;
        SetPhysics(true);

        MaskAndFire(pullValue);
        StartCoroutine(RotateWithVelocity());

        lastPosition = tip.position;
    }

    private void SetPhysics(bool usePhysics)
    {
        rigidBody.isKinematic = !usePhysics;
        rigidBody.useGravity = usePhysics;
    }

    private void MaskAndFire(float power)
    {
        // colliders[0].enabled = false;
        interactionLayerMask = 1 << LayerMask.NameToLayer("Ignore");

        Vector3 force = transform.forward * (power * speed);
        rigidBody.AddForce(force);
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    public new void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
    }

    public new void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
    }

    public new void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
    }
}
