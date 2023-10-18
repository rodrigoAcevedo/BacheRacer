using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float Health;

    private float TurnSpeed => GameManager.Instance.BaseParameters.BaseTurnSpeed;

    private bool TouchingScreen = false;
    private bool CanMove = true;
    private bool Invulnerable = false;

    private Vector3 Target;

    [SerializeField] private GameObject NitroFlare;
    [SerializeField] private SpriteRenderer CarModel;

    private void Start()
    {
        Health = UserDataUtility.GetPlayerHealth();
    }

    private void OnEnable()
    {
        Events.OnDamage.Subscribe(OnDamage);
        Events.OnNitroActivated.Subscribe(OnNitroActivated);
        Events.OnWinLevel.Subscribe(OnWinLevel);
    }

    private void OnDisable()
    {
        Events.OnDamage.Unsubscribe(OnDamage);
        Events.OnNitroActivated.Unsubscribe(OnNitroActivated);
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
        else if (Invulnerable)
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
            StartCoroutine(BlinkEffect());
        }
        if (Health <= 0)
        {
            CanMove = false;
            Events.OnLoseGame.Dispatch();
            UserDataUtility.SetPlayerHealth(Health);
        }
        Events.OnUpdatePlayerHealth.Dispatch(Health.ToString());
    }

    private IEnumerator BlinkEffect()
    {
        CarModel.enabled = false;
        yield return new WaitForSeconds(0.1f);
        CarModel.enabled = true;
        yield return new WaitForSeconds(0.1f);
        CarModel.enabled = false;
        yield return new WaitForSeconds(0.1f);
        CarModel.enabled = true;
        yield return new WaitForSeconds(0.1f);
    }

    private void OnNitroActivated(bool isActive)
    {
        Invulnerable = isActive;
        NitroFlare.SetActive(isActive);
        
        if(isActive)
            Events.OnMessage.Dispatch(MessageType.Principal, "Nitro active! Invulnerable");
    }

    private void OnWinLevel()
    {
        Invulnerable = true;
        CanMove = false;
        Target.y = 6f;
        UserDataUtility.SetPlayerHealth(Health);
    }
}
