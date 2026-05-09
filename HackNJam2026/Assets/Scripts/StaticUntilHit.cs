using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public sealed class StaticUntilHit : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] private LayerMask activationLayers = ~0;
    [SerializeField] private string activationTag = "";
    [SerializeField] private bool allowChainReaction = true;
    [SerializeField] private float minimumRelativeSpeed = 0.1f;
    [SerializeField, Range(0f, 1f)] private float chainReactionDamping = 0.35f;
    [SerializeField, Min(0f)] private float minimumChainReactionEnergy = 0.2f;

    [Header("Physics")]
    [SerializeField] private bool startLocked = true;
    [SerializeField] private bool enableGravityOnActivation = true;
    [SerializeField] private float transferredImpulseMultiplier = 0.2f;

    private Rigidbody rb;
    private bool activated;
    private float activationEnergy = 1f;

    public bool IsActivated => activated;

    private void Awake()
    {
        EnsureRigidbody();

        if (startLocked)
        {
            Lock();
        }
        else
        {
            activated = !rb.isKinematic;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (activated || collision.relativeVelocity.magnitude < minimumRelativeSpeed)
        {
            return;
        }

        if (TryGetActivationEnergy(collision.collider, out float energy))
        {
            Activate(collision.relativeVelocity, energy);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && TryGetActivationEnergy(other, out float energy))
        {
            Activate(Vector3.zero, energy);
        }
    }

    [ContextMenu("Activate Physics")]
    public void Activate()
    {
        Activate(Vector3.zero, 1f);
    }

    [ContextMenu("Lock Physics")]
    public void Lock()
    {
        EnsureRigidbody();

        activated = false;
        activationEnergy = 1f;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.Sleep();
    }

    private void Activate(Vector3 relativeVelocity, float energy)
    {
        EnsureRigidbody();

        activated = true;
        activationEnergy = Mathf.Max(energy, minimumChainReactionEnergy);
        rb.isKinematic = false;
        rb.useGravity = enableGravityOnActivation;
        rb.WakeUp();

        if (relativeVelocity.sqrMagnitude > 0f && transferredImpulseMultiplier > 0f)
        {
            rb.AddForce(relativeVelocity * transferredImpulseMultiplier * activationEnergy, ForceMode.Impulse);
        }
    }

    private bool TryGetActivationEnergy(Collider other, out float energy)
    {
        energy = 1f;

        if (((1 << other.gameObject.layer) & activationLayers.value) == 0)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(activationTag) && !other.CompareTag(activationTag))
        {
            return false;
        }

        StaticUntilHit source = other.GetComponentInParent<StaticUntilHit>();

        if (source == null)
        {
            return true;
        }

        if (!allowChainReaction)
        {
            return false;
        }

        energy = source.activationEnergy * chainReactionDamping;
        return energy >= minimumChainReactionEnergy;
    }

    private void EnsureRigidbody()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (rb == null)
        {
            Debug.LogError($"{nameof(StaticUntilHit)} requires a Rigidbody.", this);
            enabled = false;
        }
    }
}
