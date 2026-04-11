using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer start; 
    public AnimatedSpriteRenderer middle; 
    public AnimatedSpriteRenderer end;

    public void SetActiveRenderer ( AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    
}

