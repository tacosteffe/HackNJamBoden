using UnityEditor;
using UnityEngine;

public static class StaticUntilHitSetup
{
    [MenuItem("Tools/JAMRF/Physics/Add Static Until Hit To Selection")]
    private static void AddStaticUntilHitToSelection()
    {
        int changed = 0;

        foreach (GameObject selected in Selection.gameObjects)
        {
            foreach (Rigidbody rigidbody in selected.GetComponentsInChildren<Rigidbody>(true))
            {
                Undo.RecordObject(rigidbody, "Configure Static Until Hit Rigidbody");
                rigidbody.isKinematic = false;
                rigidbody.linearVelocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;

                if (!rigidbody.TryGetComponent(out StaticUntilHit _))
                {
                    Undo.AddComponent<StaticUntilHit>(rigidbody.gameObject);
                    changed++;
                }
            }
        }

        Debug.Log($"StaticUntilHit added/configured on {changed} Rigidbody object(s).");
    }

    [MenuItem("Tools/JAMRF/Physics/Add Static Until Hit To Selection", true)]
    private static bool CanAddStaticUntilHitToSelection()
    {
        return Selection.gameObjects.Length > 0;
    }
}
