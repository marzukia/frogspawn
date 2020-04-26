using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogsController : MonoBehaviour
{
    public int StartingFrogs;
    public GameObject FrogPrefab;
    public SpriteRenderer BackgroundSpriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < StartingFrogs; i++)
        {
            Instantiate(
                FrogPrefab,
                GenerateValidVector(BackgroundSpriteRenderer),
                Quaternion.identity
            );
        }
    }

    // Update is called once per frame
    void Update()
    {

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
