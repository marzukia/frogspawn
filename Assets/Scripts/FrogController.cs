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
    public float ReproductionCooldown;
    public float MutationRate = 0.15f;
    public float Metabolism;
    public float Intelligence;

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
        transform.localScale = new Vector3(Size / 100, Size / 100);
        CalculateMetabolism();
    }

    private void Update()
    {
        MoveFrog();
        ConsumeEnergy();
        CalculateHealth();
        CreateOffspring();
        UpdateTimer();
    }

    private void InitialStartActions()
    {
        Size = Random.Range(1, 15);
        Speed = Random.Range(1, 5);
        Intelligence = Random.Range(1, 100);
        ReproductionCooldown = Random.Range(1, 10);
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
            Size = Random.Range(1, 15) / (Size / 3f);
            Speed = Random.Range(1, 10) - (Size / 3f);
            ReproductionCooldown = Random.Range(1, 10);
            Intelligence = Random.Range(1, 100);
            Color = new Color32(
                (byte) Random.Range(0, 256),
                (byte) Random.Range(0, 256),
                (byte) Random.Range(0, 256),
                (byte) 255
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Food")
        {
            Destroy(other.gameObject);
            Energy += 3f;
        }
    }

    private void CalculateMetabolism()
    {
        Metabolism = (Size * 0.5f) + (Speed * 0.5f) + (Intelligence / 20 * 0.5f);
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
        if (Random.Range(0, 100) < Intelligence)
        {
            var movementVector = FindClosestFood();
            _rb2d.AddForce(movementVector * Speed);
        }
        else
        {
            var horizontalMovement = Random.Range(-1f, 1f);
            var verticalMovement = Random.Range(-1f, 1f);
            var movementVector = new Vector3(horizontalMovement, verticalMovement);
        }
    }

    private Vector3 FindClosestFood()
    {
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 closestDirectionVector = transform.position;
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");

        foreach (GameObject food in allFood)
        {
            var directionVector = food.transform.position - transform.position;
            var distanceSqr = directionVector.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr) {
                closestDistanceSqr = distanceSqr;
                closestDirectionVector = directionVector;
            }
        }

        return closestDirectionVector;
    }

    private void ConsumeEnergy()
    {
        Energy -= Metabolism * Time.deltaTime;
    }

    private void CalculateHealth()
    {
        if (Energy > 0 && Health < 100)
        {
            Health += 50f;
        }
        else if (Energy < 0 && Health > 0)
        {
            Health -= 50f;
        } else if (Health <= 0)
        {
            Destroy(gameObject);
        }
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
