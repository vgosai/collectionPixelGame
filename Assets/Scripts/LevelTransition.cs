using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string triggerTag = "Player";
    public string playerSpawnTransformName = "NOT SET";
    public float enterSpeed = 1f;
    public SceneAsset sceneToLoad;
    public GameObject fadeAnimation;
    private Canvas canvas;
    private Animator transitionAnimator;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        if (sceneToLoad == null)
        {
            throw new MissingReferenceException(name + "has no sceneToLoad set");
        }
        if (fadeAnimation == null)
        {
            throw new MissingReferenceException(name + "has no fadeAnimation set for the transition");
        }
    }

    void Update()
    {
        if (transitionAnimator != null)
        {
            if (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                LevelEvents.levelExit.Invoke(sceneToLoad, playerSpawnTransformName);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == triggerTag)
        {
            Damageable playerDamageable = collider.gameObject.GetComponent<Damageable>();
            if (playerDamageable != null)
            {
                playerDamageable.Invincible = true;
            }
            Rigidbody2D playerBody = collider.gameObject.GetComponent<Rigidbody2D>();
            playerBody.bodyType = RigidbodyType2D.Kinematic;
            Vector2 entranceDirection = (transform.position - playerBody.transform.position).normalized;
            playerBody.velocity = entranceDirection * enterSpeed;
            transitionAnimator = Instantiate(fadeAnimation, canvas.transform).GetComponent<Animator>();
        }
    }
}
