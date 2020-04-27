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

    public InputField FoodValue;
    public InputField SizeCost;
    public InputField SpeedCost;
    public InputField IntelligenceCost;

    private Rigidbody2D _rb2d;
    private SpriteRenderer _renderer;
    private float _foodValue;
    private float _sizeCost;
    private float _speedCost;
    private float _intelligenceCost;

    private void Start()
    {
        UpdateEnvironmentValues();
        if (!Started) {
            InitialStartActions();
        } else {
            MutationRoll();
        }
        LoadPrivateVariables();
        CalculateMetabolism();

         transform.localScale = new Vector3(Size / 100, Size / 100);
    }

    private void Update()
    {
        MoveFrog();
        ConsumeEnergy();
        CalculateHealth();
        CreateOffspring();
        UpdateTimer();
        UpdateEnvironmentValues();
    }

    private void LoadPrivateVariables()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.freezeRotation = true;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = Color;
    }

    private void UpdateEnvironmentValues()
    {
        FoodValue = (InputField) GameObject.Find("FoodValue").GetComponent<InputField>();
        SizeCost = (InputField) GameObject.Find("SizeCost").GetComponent<InputField>();
        SpeedCost = (InputField) GameObject.Find("SpeedCost").GetComponent<InputField>();
        IntelligenceCost = (InputField) GameObject.Find("IntelligenceCost").GetComponent<InputField>();
        _foodValue = float.Parse(FoodValue.text);
        _sizeCost = float.Parse(SizeCost.text);
        _speedCost = float.Parse(SpeedCost.text);
        _intelligenceCost = float.Parse(IntelligenceCost.text);
    }

    private void InitialStartActions()
    {
        RollBaseTraits();
        Energy = 50;
        Health = 100f * (1f + Size);
        Started = true;
    }

    private void MutationRoll()
    {
        if (Random.Range(0, 100) / 100f < MutationRate) {
            RollBaseTraits();
        }
    }

    private void RollBaseTraits(){
        Size = Random.Range(1, 20);
        Speed = Random.Range(1, 10) - (Size / 3f);
        if (Speed < 1f) {
            Speed = 1f;
        }
        Intelligence = Random.Range(0, 10);
        ReproductionCooldown = Random.Range(1, 10);
        Color = new Color32(
            (byte) Random.Range(0, 256),
            (byte) Random.Range(0, 256),
            (byte) Random.Range(0, 256),
            (byte) 255
        );
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Food")
        {
            Destroy(other.gameObject);
            Energy += _foodValue;
        }
    }

    private void CalculateMetabolism()
    {
        Metabolism = (Size * _sizeCost)
            + (Speed * _speedCost)
            + (Intelligence * _intelligenceCost);
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
        if (Random.Range(0, Intelligence) < Intelligence)
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
