﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A circular conveyer belt tracks parts around a semi-circle.
/// </summary>
public class CircularConveyerBelt : ConveyerBelt
{
    [Header("Circular Anchors")]
    public float radius;
    public bool inverted;

    private SpriteRenderer sushiBelt;
    private Dictionary<GameObject, float> angleDictionary = new Dictionary<GameObject, float>();

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        sushiBelt = transform.Find("Sushi Belt").GetComponent<SpriteRenderer>();

        if(inverted)
        {
            sushiBelt.flipX = true;
        }
    }

    /// <summary>
    /// Spawns the instance of a part so that the conveyer can move it on it's own accord.
    /// </summary>
    public override void SpawnPart(PartInstance part)
    {
        Vector2 targetStartingPoint = transform.position;
        if (!inverted)
        {
            targetStartingPoint = new Vector2(transform.position.x - radius, transform.position.y);
        }
        else
        {
            targetStartingPoint = new Vector2(transform.position.x + radius, transform.position.y);
        }

        PartInstance partInstance = Instantiate(part, targetStartingPoint, Quaternion.identity);
        partInstance.transform.SetParent(spawnedPartsTransform);

        partsOnBelt.Add(partInstance);
        angleDictionary.Add(partInstance.gameObject, 0);

        if(!inverted)
        {
            if(!invertedDirection)
            {
                angleDictionary[partInstance.gameObject] = -1.2f;
            }
            else
            {
                angleDictionary[partInstance.gameObject] = 4.4f;
            }
        }
        else
        {
            if (!invertedDirection)
            {
                angleDictionary[partInstance.gameObject] = 1.2f;
            }
            else
            {
                angleDictionary[partInstance.gameObject] = -4.4f;
            }
        }
    }

    /// <summary>
    /// Animates the conveyer belt.
    /// </summary>
    public override void AnimateConveyer()
    {
        if(!invertedDirection)
        {
            if(sushiBelt.flipY)
            {
                sushiBelt.flipY = false;
            }

            sushiBelt.transform.Rotate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            if (!sushiBelt.flipY)
            {
                sushiBelt.flipY = true;
            }

            sushiBelt.transform.Rotate(0, 0, -speed * Time.deltaTime);
        }
    }


    /// <summary>
    /// Moves a part along the belt.
    /// </summary>
    public override void MovePart(PartInstance part)
    {
        Vector2 offset = Vector2.zero;

        if (invertedDirection)
        {
            if (!inverted)
            {
                angleDictionary[part.gameObject] -= speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x - radius, transform.position.y), part.transform.position) <= 0.1f)
                {
                    DestroyConveyerPart(part);
                }
            }
            else
            {
                angleDictionary[part.gameObject] += speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x + radius, transform.position.y), part.transform.position) <= 0.1f)
                {
                    DestroyConveyerPart(part);
                }
            }
        }
        else
        {
            if (!inverted)
            {
                angleDictionary[part.gameObject] += speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x - radius, transform.position.y), part.transform.position) <= 0.1f)
                {
                    DestroyConveyerPart(part);
                }
            }
            else
            {
                angleDictionary[part.gameObject] -= speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x + radius, transform.position.y), part.transform.position) <= 0.1f)
                {
                    DestroyConveyerPart(part);
                }
            }
        }
    }

    /// <summary>
    /// Removes a part from the conveyer belt.
    /// </summary>
    public override void RemoveConveyerPart(PartInstance part)
    {
        partsOnBelt.Remove(part);
        angleDictionary.Remove(part.gameObject);
    }

    /// <summary>
    /// Destroys a part from the conveyer belt. In the circular conveyer belt, also removes it from angle calculations.
    /// </summary>
    public override void DestroyConveyerPart(PartInstance part)
    {
        base.DestroyConveyerPart(part);
    }

    /// <summary>
    /// Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        if(!inverted)
        {
            Gizmos.DrawSphere(new Vector2(transform.position.x - radius, transform.position.y), 0.2f);
        }
        else
        {
            Gizmos.DrawSphere(new Vector2(transform.position.x + radius, transform.position.y), 0.2f);
        }

    }


}
