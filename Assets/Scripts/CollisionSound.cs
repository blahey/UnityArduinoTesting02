using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] collisionSounds;

    [SerializeField] private float minimumVelocity = 0.5f;
    [SerializeField] private float maximumVelocity = 10f;

    [SerializeField] private float minimumVolume = 0.1f;
    [SerializeField] private float maximumVolume = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity < minimumVelocity)
            return;

        float volume = Mathf.InverseLerp(
            minimumVelocity,
            maximumVelocity,
            impactVelocity
        );

        volume = Mathf.Lerp(minimumVolume, maximumVolume, volume);

        AudioClip clip =
            collisionSounds[Random.Range(0, collisionSounds.Length)];

        audioSource.PlayOneShot(clip, volume);
    }
}