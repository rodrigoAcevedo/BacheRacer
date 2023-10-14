using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float Health = 100f;
    [SerializeField]
    private float TurnSpeed = 3f;
    
    private bool TouchingScreen = false;
    private bool CanMove = true;
    private bool Invulnerable = false; // We use this when we don't want to take damage, for example when we win the level sequence. Also a powerup maybe in a future?

    private Vector3 Target;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Events.OnDamage.Subscribe(OnDamage);
        Events.OnWinLevel.Subscribe(OnWinLevel);
    }

    private void OnDisable()
    {
        Events.OnDamage.Unsubscribe(OnDamage);
        Events.OnWinLevel.Unsubscribe(OnWinLevel);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputTouch();
    }

    private void HandleInputTouch()
    {
        if (CanMove)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    TouchingScreen = true;
                    Target = Camera.main.ScreenToWorldPoint(touch.position);
                    Target.y = transform.position.y;
                    Target.z = transform.position.z;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    TouchingScreen = false;
                }
            }

            if (TouchingScreen)
            {
                MovingToTarget();
            }
        }
        else if (Invulnerable) // For now only applies when wining levels.
        {
            MovingToTarget();
        }
    }

    private void MovingToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * TurnSpeed);
    }

    private void OnDamage(float damage)
    {
        if (!Invulnerable)
        {
            Health -= damage;
        }
        if (Health <= 0)
        {
            CanMove = false;
            Events.OnLoseGame.Dispatch();
        }
    }

    private void OnWinLevel()
    {
        Invulnerable = true;
        CanMove = false;
        Target.y = 6f;
    }
}
