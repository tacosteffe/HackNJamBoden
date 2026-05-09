using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Fracture : MonoBehaviour
{
    public TriggerOptions triggerOptions;
    public FractureOptions fractureOptions;
    public RefractureOptions refractureOptions;
    public CallbackOptions callbackOptions;

    [Header("Fragment Layer")]
    public string fragmentLayerName = "Castle";

    [Header("Fragment Physics")]
    public bool fragmentsUseGravity = true;
    public bool fragmentsAreKinematic = false;
    public float fragmentMass = 1f;
    public float fragmentDrag = 0f;
    public float fragmentAngularDrag = 0.05f;

    [Header("Fragment Wake / Explosion")]
    public bool wakeFragmentsAfterFracture = true;
    public float explosionForce = 2f;
    public float explosionRadius = 3f;
    public float explosionUpwardsModifier = 0.2f;

    /// <summary>
    /// The number of times this fragment has been re-fractured.
    /// </summary>
    [HideInInspector]
    public int currentRefractureCount = 0;

    /// <summary>
    /// Collector object that stores the produced fragments.
    /// </summary>
    private GameObject fragmentRoot;

    private int FragmentLayer
    {
        get
        {
            int layer = LayerMask.NameToLayer(fragmentLayerName);

            if (layer == -1)
            {
                Debug.LogWarning(
                    $"Layer '{fragmentLayerName}' does not exist. Using current object layer instead.",
                    this
                );

                return gameObject.layer;
            }

            return layer;
        }
    }

    [ContextMenu("Print Mesh Info")]
    public void PrintMeshInfo()
    {
        var mesh = this.GetComponent<MeshFilter>().mesh;
        Debug.Log("Positions");

        var positions = mesh.vertices;
        var normals = mesh.normals;
        var uvs = mesh.uv;

        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log($"Vertex {i}");
            Debug.Log($"POS | X: {positions[i].x} Y: {positions[i].y} Z: {positions[i].z}");
            Debug.Log($"NRM | X: {normals[i].x} Y: {normals[i].y} Z: {normals[i].z} LEN: {normals[i].magnitude}");
            Debug.Log($"UV  | U: {uvs[i].x} V: {uvs[i].y}");
            Debug.Log("");
        }
    }

    public void CauseFracture()
    {
        callbackOptions.CallOnFracture(null, gameObject, transform.position);
        this.ComputeFracture();
    }

    void OnValidate()
    {
        if (this.transform.parent != null)
        {
            var scale = this.transform.parent.localScale;

            if ((scale.x != scale.y) || (scale.x != scale.z) || (scale.y != scale.z))
            {
                Debug.LogWarning(
                    "Warning: Parent transform of fractured object must be uniformly scaled in all axes or fragments will not render correctly.",
                    this.transform
                );
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (triggerOptions.triggerType == TriggerType.Collision)
        {
            if (collision.contactCount > 0)
            {
                var contact = collision.contacts[0];

                float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

                bool tagAllowed = triggerOptions.IsTagAllowed(contact.otherCollider.gameObject.tag);

                // If tag filtering is OFF, allow all tags.
                // If tag filtering is ON, only allow approved tags.
                bool allowedByTag =
                    !triggerOptions.filterCollisionsByTag ||
                    tagAllowed;

                if (collisionForce > triggerOptions.minimumCollisionForce && allowedByTag)
                {
                    callbackOptions.CallOnFracture(contact.otherCollider, gameObject, contact.point);
                    this.ComputeFracture();
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (triggerOptions.triggerType == TriggerType.Trigger)
        {
            bool tagAllowed = triggerOptions.IsTagAllowed(collider.gameObject.tag);

            // If tag filtering is OFF, allow all tags.
            // If tag filtering is ON, only allow approved tags.
            bool allowedByTag =
                !triggerOptions.filterCollisionsByTag ||
                tagAllowed;

            if (allowedByTag)
            {
                callbackOptions.CallOnFracture(collider, gameObject, transform.position);
                this.ComputeFracture();
            }
        }
    }

    void Update()
    {
        if (triggerOptions.triggerType == TriggerType.Keyboard)
        {
            if (Input.GetKeyDown(triggerOptions.triggerKey))
            {
                callbackOptions.CallOnFracture(null, gameObject, transform.position);
                this.ComputeFracture();
            }
        }
    }

    /// <summary>
    /// Compute the fracture and create the fragments.
    /// </summary>
    private void ComputeFracture()
    {
        var mesh = this.GetComponent<MeshFilter>().sharedMesh;

        if (mesh == null)
            return;

        int fragmentLayer = FragmentLayer;

        // If the fragment root object has not yet been created, create it now.
        if (this.fragmentRoot == null)
        {
            this.fragmentRoot = new GameObject($"{this.name}Fragments");

            this.fragmentRoot.layer = fragmentLayer;

            this.fragmentRoot.transform.SetParent(this.transform.parent);
            this.fragmentRoot.transform.position = this.transform.position;
            this.fragmentRoot.transform.rotation = this.transform.rotation;
            this.fragmentRoot.transform.localScale = Vector3.one;
        }

        var fragmentTemplate = CreateFragmentTemplate();

        // Make sure template is also on correct layer.
        fragmentTemplate.layer = fragmentLayer;

        if (fractureOptions.asynchronous)
        {
            StartCoroutine(Fragmenter.FractureAsync(
                this.gameObject,
                this.fractureOptions,
                fragmentTemplate,
                this.fragmentRoot.transform,
                () =>
                {
                    GameObject.Destroy(fragmentTemplate);

                    // Make extra sure all generated fragments are on Castle layer.
                    SetLayerRecursive(this.fragmentRoot.transform, fragmentLayer);

                    // Make sure fragments actually become physical objects.
                    PrepareFragmentsForPhysics();

                    if (wakeFragmentsAfterFracture)
                    {
                        WakeFragments(transform.position);
                    }

                    // Deactivate original object.
                    this.gameObject.SetActive(false);

                    // Fire the completion callback.
                    if ((this.currentRefractureCount == 0) ||
                        (this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
                    {
                        if (callbackOptions.onCompleted != null)
                        {
                            callbackOptions.onCompleted.Invoke();
                        }
                    }
                }
            ));
        }
        else
        {
            Fragmenter.Fracture(
                this.gameObject,
                this.fractureOptions,
                fragmentTemplate,
                this.fragmentRoot.transform
            );

            GameObject.Destroy(fragmentTemplate);

            // Make extra sure all generated fragments are on Castle layer.
            SetLayerRecursive(this.fragmentRoot.transform, fragmentLayer);

            // Make sure fragments actually become physical objects.
            PrepareFragmentsForPhysics();

            if (wakeFragmentsAfterFracture)
            {
                WakeFragments(transform.position);
            }

            // Deactivate original object.
            this.gameObject.SetActive(false);

            // Fire the completion callback.
            if ((this.currentRefractureCount == 0) ||
                (this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
            {
                if (callbackOptions.onCompleted != null)
                {
                    callbackOptions.onCompleted.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// Creates a template object which each fragment will derive from.
    /// </summary>
    private GameObject CreateFragmentTemplate()
    {
        GameObject obj = new GameObject();

        obj.name = "Fragment";
        obj.tag = this.tag;
        obj.layer = FragmentLayer;

        // Update mesh to the new sliced mesh.
        obj.AddComponent<MeshFilter>();

        // Add materials. Normal material goes in slot 1, cut material in slot 2.
        var meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = new Material[2]
        {
            this.GetComponent<MeshRenderer>().sharedMaterial,
            this.fractureOptions.insideMaterial
        };

        // Collider for fragment.
        var thisCollider = this.GetComponent<Collider>();

        var fragmentCollider = obj.AddComponent<MeshCollider>();
        fragmentCollider.convex = true;
        fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;

        // Viktigt:
        // Fragments ska vara fysiska bitar, inte triggers.
        fragmentCollider.isTrigger = false;

        // Rigidbody for fragment.
        var thisRigidBody = this.GetComponent<Rigidbody>();

        var fragmentRigidBody = obj.AddComponent<Rigidbody>();

        // Copy starting motion from original object.
        fragmentRigidBody.linearVelocity = thisRigidBody.linearVelocity;
        fragmentRigidBody.angularVelocity = thisRigidBody.angularVelocity;

        // Force fragment physics.
        fragmentRigidBody.mass = fragmentMass;
        fragmentRigidBody.linearDamping = fragmentDrag;
        fragmentRigidBody.angularDamping = fragmentAngularDrag;
        fragmentRigidBody.useGravity = fragmentsUseGravity;
        fragmentRigidBody.isKinematic = fragmentsAreKinematic;
        fragmentRigidBody.constraints = RigidbodyConstraints.None;

        // More stable for loose fragments.
        fragmentRigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        fragmentRigidBody.interpolation = RigidbodyInterpolation.Interpolate;

        fragmentRigidBody.WakeUp();

        // If refracturing is enabled, create a copy of this component and add it to the template fragment object.
        if (refractureOptions.enableRefracturing &&
            (this.currentRefractureCount < refractureOptions.maxRefractureCount))
        {
            CopyFractureComponent(obj);
        }

        return obj;
    }

    /// <summary>
    /// Copy this component to another component.
    /// </summary>
    private void CopyFractureComponent(GameObject obj)
    {
        var fractureComponent = obj.AddComponent<Fracture>();

        fractureComponent.triggerOptions = this.triggerOptions;
        fractureComponent.fractureOptions = this.fractureOptions;
        fractureComponent.refractureOptions = this.refractureOptions;
        fractureComponent.callbackOptions = this.callbackOptions;
        fractureComponent.currentRefractureCount = this.currentRefractureCount + 1;
        fractureComponent.fragmentRoot = this.fragmentRoot;

        // Keep same layer setting when fragments can refracture.
        fractureComponent.fragmentLayerName = this.fragmentLayerName;

        // Keep same physics settings when fragments can refracture.
        fractureComponent.fragmentsUseGravity = this.fragmentsUseGravity;
        fractureComponent.fragmentsAreKinematic = this.fragmentsAreKinematic;
        fractureComponent.fragmentMass = this.fragmentMass;
        fractureComponent.fragmentDrag = this.fragmentDrag;
        fractureComponent.fragmentAngularDrag = this.fragmentAngularDrag;

        fractureComponent.wakeFragmentsAfterFracture = this.wakeFragmentsAfterFracture;
        fractureComponent.explosionForce = this.explosionForce;
        fractureComponent.explosionRadius = this.explosionRadius;
        fractureComponent.explosionUpwardsModifier = this.explosionUpwardsModifier;
    }

    private void SetLayerRecursive(Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        foreach (Transform child in parent)
        {
            SetLayerRecursive(child, layer);
        }
    }

    private void PrepareFragmentsForPhysics()
    {
        if (fragmentRoot == null)
            return;

        Rigidbody[] bodies = fragmentRoot.GetComponentsInChildren<Rigidbody>(true);
        MeshCollider[] colliders = fragmentRoot.GetComponentsInChildren<MeshCollider>(true);

        foreach (MeshCollider col in colliders)
        {
            col.convex = true;
            col.isTrigger = false;
        }

        foreach (Rigidbody rb in bodies)
        {
            rb.mass = fragmentMass;
            rb.linearDamping = fragmentDrag;
            rb.angularDamping = fragmentAngularDrag;

            rb.useGravity = fragmentsUseGravity;
            rb.isKinematic = fragmentsAreKinematic;
            rb.constraints = RigidbodyConstraints.None;

            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            rb.WakeUp();
        }
    }

    private void WakeFragments(Vector3 explosionPosition)
    {
        if (fragmentRoot == null)
            return;

        Rigidbody[] bodies = fragmentRoot.GetComponentsInChildren<Rigidbody>(true);

        foreach (Rigidbody rb in bodies)
        {
            rb.useGravity = fragmentsUseGravity;
            rb.isKinematic = fragmentsAreKinematic;
            rb.constraints = RigidbodyConstraints.None;
            rb.WakeUp();

            rb.AddExplosionForce(
                explosionForce,
                explosionPosition,
                explosionRadius,
                explosionUpwardsModifier,
                ForceMode.Impulse
            );
        }
    }
}