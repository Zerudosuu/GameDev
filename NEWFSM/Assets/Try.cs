using System.Collections.Generic;
using UnityEngine;

public class Try : MonoBehaviour
{
    public GameObject myExplosion;
    public GameObject prefabExplosion;
    private ParticleSystem particleS;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        particleS = FindAnyObjectByType<ParticleSystem>();
        var mainModule = particleS.main;
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        particleS.GetCollisionEvents(other, collisionEvents);

        foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
        {
            Vector3 collisionHitLoc = collisionEvent.intersection;

            Instantiate(prefabExplosion, collisionHitLoc, Quaternion.identity);
        }

        collisionEvents.Clear();
    }
}
