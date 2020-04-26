using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FrogController : MonoBehaviour
{
    public bool Started;
    public float Speed;
    public float Size;
    public float Energy;
    public float Health;
    public Color32 Color;
    public bool ReproductionActive;
    public float ReproductionTimer;
    public float ReproductionCooldown = 30f;
    public float MutationRate = 0.15f;

    private Rigidbody2D _rb2d;
    private SpriteRenderer _renderer;

    private void Start()
    {
        if (!Started) {
            InitialStartActions();
        } else {
            MutationRoll();
        }
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.freezeRotation = true;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = Color;
        transform.localScale = new Vector3(Size, Size);
    }

    private void Update()
    {
        MoveFrog();
        ConsumeEnergy();
        CalculateHealth();
        UpdateStats();
        CreateOffspring();
        UpdateTimer();
    }

    private void InitialStartActions()
    {
        Size = Random.Range(1, 5) / 100f;
        Speed = Random.Range(1, 15) * (1f - Size);
        Energy = 50;
        Health = 100f * (1f + Size);
        Color = new Color32(
            (byte) Random.Range(0, 256),
            (byte) Random.Range(0, 256),
            (byte) Random.Range(0, 256),
            (byte) 255
        );
        Started = true;
    }

    private void MutationRoll()
    {
        if (Random.Range(0, 100) / 100f < MutationRate) {
            Size = Random.Range(1, 20) / 100f;
            Speed = Random.Range(1, 10) * (1f - Size);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Food")
        {
            Destroy(other.gameObject);
            Energy += 5;
        }
    }

    private void UpdateTimer()
    {
        if(ReproductionActive)
        {
            ReproductionTimer -= Time.deltaTime;
        }
        if(ReproductionTimer < 0)
        {
            ReproductionTimer = 0;
        }
    }

    private void MoveFrog()
    {
        var horizontalMovement = Random.Range(-1f, 1f);
        var verticalMovement = Random.Range(-1f, 1f);
        var movementVector = new Vector3(horizontalMovement, verticalMovement);
        _rb2d.AddForce(movementVector * Speed);
    }

    private void ConsumeEnergy()
    {
        Energy -= Size * 1f;
    }

    private void CalculateHealth()
    {
        if (Energy > 0 && Health < 100)
        {
            Health += 1f;
        }
        else if (Energy < 0 && Health > 0)
        {
            Health -= 5f;
        } else if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateStats()
    {

    }

    private void CreateOffspring()
    {
        if (ReproductionTimer > 0)
        {
            return;
        }
        else {
            ReproductionActive = false;
            if (Energy > 100 && !ReproductionActive) {
                Energy = Energy / (2f * (1f + Size));
                Instantiate(gameObject, transform.position, Quaternion.identity);
                ReproductionTimer = ReproductionCooldown;
                ReproductionActive = true;
            }
        }
    }
}
