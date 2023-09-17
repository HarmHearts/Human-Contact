using HC.Data;
using HC.ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : SerializedMonoBehaviour
{
    [OdinSerialize] public Human Human;
    public SpriteRenderer BodySprite;
    public GameObject DynamicAnchor;

    private List<GameObject> bodies = new List<GameObject>();

    private World world = World.Instance;
    
    // Start is called before the first frame update
    void Start()
    {
        Human.Initialize();
        BuildBody(Human, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Human.WorldPosition.x * 0.5f, Human.WorldPosition.y * -0.5f);
    }

    void RedrawBody()
    {

    }

    void BuildBody(IBody body, Transform parent)
    {
        BodyPart[,] grid = body.CurrentPlan.BodyPartGrid;

        for (int j = 0; j < grid.GetLength(1); j++)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                if (grid[i, j] != null)
                {
                    if (grid[i,j] is DynamicLimbBodyPart)
                    {
                        Transform t = Instantiate(
                            DynamicAnchor,
                            new Vector2(i * 0.5f, j * -0.5f) - (new Vector2(body.Anchor.x, -body.Anchor.y) * 0.5f),
                            Quaternion.identity,
                            parent).transform;

                        bodies.Add(t.gameObject);
                        BuildBody((DynamicLimbBodyPart)grid[i, j], t);
                    } else
                    {
                        SpriteRenderer r = Instantiate(
                            BodySprite, 
                            new Vector2(i * 0.5f, j * -0.5f) - (new Vector2(body.Anchor.x, -body.Anchor.y) * 0.5f - (Vector2)parent.position), 
                            Quaternion.identity,
                            parent);
                        if (grid[i, j].Sprite) r.sprite = (grid[i, j].Sprite);
                        bodies.Add(r.gameObject);
                    }

                }
            }
        }
    }

    void DestroyBody()
    {

    }
}
