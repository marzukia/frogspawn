using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalFoodController : MonoBehaviour
{
    public int MaxFood;
    public GameObject FoodPrefab;
    public SpriteRenderer BackgroundSpriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < MaxFood; i++)
        {
            for (int j = 0; j < Random.Range(0, 10); j++ )
            {
                Instantiate(
                    FoodPrefab,
                    GenerateValidVector(BackgroundSpriteRenderer),
                    Quaternion.identity
                );
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var currentFood = GameObject.FindGameObjectsWithTag("Food").Length;
        if (currentFood < MaxFood) {
            Instantiate(
                FoodPrefab,
                GenerateValidVector(BackgroundSpriteRenderer),
                Quaternion.identity
            );
        }
    }

    private Vector3 GenerateValidVector(SpriteRenderer BackgroundSpriteRenderer)
    {
        var bounds = BackgroundSpriteRenderer.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
