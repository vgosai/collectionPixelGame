using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Particle System Event Notes
| Method                                  | When it’s called                                         | Requirements                                                                     |
| --------------------------------------- | -------------------------------------------------------- | -------------------------------------------------------------------------------- |
| `OnParticleCollision(GameObject other)` | When a particle with collision enabled hits a Collider.  | Particle System **Collision module** enabled, `Send Collision Messages` checked. |
| `OnParticleTrigger()`                   | When particles enter/exit/stay in trigger volumes.       | Particle System **Trigger module** enabled, `Send Trigger Messages` checked.     |
| `OnParticleSystemStopped()`             | When a Particle System finishes emitting and is stopped. | Usually needs `Stop Action = Callback`.                                          |
| `OnParticleSystemPaused()`              | When a Particle System is paused.                        | Rarely used manually.                                                            |
| `OnParticleSystemPlayed()`              | When a Particle System starts playing.                   | Rarely used manually.                                                            |

*/
public class GameObjectEmitter : MonoBehaviour
{
    [field: SerializeField] public GameObject ObjectPrefab { get; private set; }

    private ParticleSystem _ps;
    private List<ParticleSystem.Particle> exitParticles = new();

    // Start is called before the first frame update
    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger() //Looking at the particle system there are some unity events where the we can invoke a unity event that looks for the function OnParticleTrigger when the particle is (inside, outside, exit, or enters the indicated collider)

    {
        _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitParticles); //This gets all the particles that are triggering the exit callback

        foreach (ParticleSystem.Particle p in exitParticles) //This loop for each particle we detect in exiting the particle system that triggers the callback that we add to the list we create a gameobject
        {
            GameObject spawnedObject = Instantiate(ObjectPrefab);
            spawnedObject.transform.position = p.position; //Copies the position of the particle we just replaced also we change the harvestable object particle system simulation space to world since its will make it so each particle has its own trnasform in the world.
        }
    }
}