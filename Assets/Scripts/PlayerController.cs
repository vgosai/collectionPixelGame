using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

/*
Notes:

Pivot Point: the pivot point is the origin point (0,0) of a GameObject or Sprite — it determines:

Where the object rotates and scales from

How the object is positioned relative to its Transform

Where other objects will attach if made children

How Pivot Affects Sprites

For 2D Sprites:

The pivot is defined in the Sprite Editor (when using a sprite atlas or texture).

It determines how the sprite is positioned in world space.

For example, if your sprite has a bottom-center pivot:

    Its Y position will align to its bottom.

    It will rotate around the base.
So in this case with a top down kind of character we are concerned with where the feet are for rotations like if our character is in front of some grass or not is dependent on the feets location so we set the pivot point there

The difference with send messages and invoke unity events is that it will only check for the implemented response function on components attached to the parent object, for invoke events you can manually assign the functions

Gravity set to zero because always on the ground and finally freeze z axis constraint

With the capsule collider we want the collider to only be around objects that matter physics wise so thats the feet cause position solely matters on feet position for determining what we are walking into and what we can interact with.

for capsule colliders the vertical/horizontal setting determins which direction of the collider can be bigger than the other. 

Enter play mode setting will change if you load domain or scene at the start of the game. Basically when you have these on the game view is that of the final build.    

When setting the number of samples you are setting the number of frame changes you want per second thus if you want a short number of key frames to play fast you use a high sample rate and put the key frames at the start and then turn off loop no exit time and no transition so when the next input comes you are not waitng for all that down time to be done at the end of the animation that you left.

The idea here is that we are going to first decide our direction then based on that direction we will find the key state that corresponds with that direction then we will get the value which gives us the direction animation in that direction.

The whole idea behind using these state data scriptable objects is other characters can use this same coded statemachine setup that is controlled by this dictionairy where we go into the walk state machine find the facing direction then play the corresponding animations in that state that we are in.

When we create the tilemap collider keep in mind a collider is just a perimeter. So we use a composite collider across the whole ground surface to create only one perimeter edge that blocks the player

When making the tilepallate make sure to drag and drop the composite object so your pallete doesnt mix everything up and also you can use the random brush which if select pick random tiles it will choose a random tile from the pool of selected random tiles which can be controlled by turning on pick random tiles then selecting the tiles you want in your random tile pool.

Now with objects on the ground we want them to show in front of the player when the player is above the tile and behind the player when the player is below the tile and we want this to be relative to the feet because the ground stuff rendering is based on the position of the players feet thus to do this we go to the universal render pipeline the renderer 2d then set the transparency sort mode to custom axis and set only y. And we set the mode on the tilemap renderer to individual instead of chunck so we sort based on the individual objects instead of the average of a group. And finally change the sprite pivot point to pivot not center in the player 

You might just want some objects to always be beneath the player so you will put them on a non sortable layer aka just a in a different layer behind the player. Overall all this sorting is just for the sake of showing that this object is not directly on the ground sometimes its in front sometimes its behind the player.

Refer to particle system but basically we changed the start size turned on inherit velocity so they get the velocity of the player multiplied it by a negative to go in the opposite direction of the player then turned simulation space from local to world so the particles do not share the transform of the player to they would get their own and branch off and changed the shape for the emission location to a circle. And changed from rate over to time to rate over distance which instead of emitting the designated number of particles per second you emit the designated number of particles per unit distance. And color over time allows you to set a gradient for the alpha value and color over the life time of the particle so for a fadeout effect just turn the alpha at the end of the gradient down. Can add extra notches to have different rates of change over the lifetime
*/
public class PlayerController : MonoBehaviour
{
    private Vector2 _axisInput = Vector2.zero;
    private Animator animator;
    private Rigidbody2D rb;
    private CharacterState _currentState; //keep track of what the active set is based off of inputs. Go into that set for the specific direction then go into the animator and render
    private AnimationClip _currentClip;
    private Vector2 _facingDir;
    private float _timeToEndAnimation = 0f;

    [field: SerializeField] public float MoveForce { get; private set; } = 5f; //With a property we can make it so setting the value is private and can only be done in the class but we can make the getter public so anyone can read but no one can set note as well that properties are not exposed to the expector even if they are made public unlike with fields who are exposed when public so we need to put the field:Serialized Field attribute to make it be both a field to the inspector but also a property to the inspector 
    [field: SerializeField] public CharacterState Idle { get; private set; } //now cause we are using scriptable object our player controller knows what the idle animations are going to be as they are all wrapped into one data object of code. Now we need to feed our facing direction into the animation set and for it to spit out the correct clip. 
    [field: SerializeField] public CharacterState Walk { get; private set; } //now cause we are using scriptable object our player controller knows what the walk animations are going to be as they are all wrapped into one data object of code. Now we need to feed our facing direction into the animation set and for it to spit out the correct clip. 
    [field: SerializeField] public CharacterState Use { get; private set; } //now cause we are using scriptable object our player controller knows what the use animations are going to be as they are all wrapped into one data object of code. Now we need to feed our facing direction into the animation set and for it to spit out the correct clip. 
    [field: SerializeField] public StateAnimationSetDictionary StateAnimations { get; private set; } //Now we need to make sure that we have a function that will take the current state then go thorugh the dictionairy get the current direction and then apply get facing direction
    [field: SerializeField] public float WalkVelocityThreshold { get; private set; } = 0.05f; //5 pixels per second per time game is running at 100 pixels per unit
    public CharacterState CurrentState
    {
        get
        {
            return _currentState;
        }
        private set
        {
            if (_currentState != value) //This check prevents you from entering a state that you are already in this is important because for the use state you want to complete the use state before going again. 
            {
                _currentState = value;
                ChangeClip();
                _timeToEndAnimation = _currentClip.length;
            }
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentState = Idle;
    }

    private void Update() //Not for physics based things sprite animation and so on would go here
    {
        _timeToEndAnimation = Mathf.Max(_timeToEndAnimation - Time.deltaTime, 0);
        if (_currentState.CanExitWhilePlaying || _timeToEndAnimation <= 0) //check if we are allowed to change clip basically we need to not change when in use function
        {
            if (_axisInput != Vector2.zero && rb.velocity.magnitude > WalkVelocityThreshold) //two conditions for movement input or _axisinput with this one if something external is pushing you are not walking and velocity greater than zero now we have it so we change directions but we are locked into the idle scriptable object we need to 
            {
                CurrentState = Walk;
            }
            else
            {
                CurrentState = Idle;
            }
            ChangeClip(); // we use this when using the use action
        }

    }

    private void ChangeClip()
    {
        //We want to get the correct current clip from the directional animation set based on our facing direction
        AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(_currentState, _facingDir);

        if (_currentClip == null || _currentClip != expectedClip)
        {
            animator.Play(expectedClip.name); //the caveat to doing like this is the animation clip in the animator will have to share the same name.
            _currentClip = expectedClip;
        }
    }



    private void FixedUpdate() //On fixed update unity assigns a force to the velocity of the rigid body physics based functions in fixed update
    {
        if (_currentState.CanMove)
        {
            Vector2 moveVector = _axisInput * MoveForce * Time.fixedDeltaTime; //fixed delta time updates consistently and you can change this update interval in the project setting but overall consistent independent of framerate
            rb.AddForce(moveVector);
        }
    }
    private void OnMove(InputValue value) // we need input here keep in mind for invoke unity events we need to use callback.context which they take but for send messages they take the input value
    {
        //Caveat to implementation is that whenever a player is not moving we need to make sure that we are not changing the animation direction even though we are still running fixed update continuously so we initialize another variable that has the facing direction that only changes when input vector is not zero or in other words we are getting an input
        _axisInput = value.Get<Vector2>();
        if (_axisInput != Vector2.zero)
        {
            _facingDir = _axisInput;
        }
    }

    private void OnUse(InputValue value) //swinging the pickaxe we dont need the input and use this to change state and animation
    {
        CurrentState = Use;
    }

}
