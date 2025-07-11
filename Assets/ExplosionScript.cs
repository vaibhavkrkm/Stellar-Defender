using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    void ExplosionDone()
    {
        // 1. Get the Animator component
        Animator animator = GetComponent<Animator>();

        if (animator == null) return; // return if no animator

        // 2. Disable the animator component
        animator.enabled = false;

        // 3. Finally, destroy the GameObject
        Destroy(gameObject);
    }
}
