using UnityEngine;
using Shapes;

public abstract class UIButton : MonoBehaviour
{
    public enum detectionType{Rectangle, Disc, Collider2D}
    [Tooltip("How to check if this button was selected")]
    public detectionType type;
    protected internal Vector2 touchLocation = new Vector2();
    public bool selectable = true;
    public abstract void OnSelected();
    public abstract void OnDeselected();
    
    public virtual bool CheckIfSelected(Vector2 atPosition)
    {
        if(!selectable) return false;

        switch(type)
        {
            case detectionType.Rectangle:
            {
                Rectangle rectangle = GetComponent<Rectangle>();
                if(rectangle.GetBounds().Contains(atPosition - (Vector2)transform.position)) return true;
                else return false;
            }
            case detectionType.Disc:
            {
                Disc disc = GetComponent<Disc>();
                if(Vector2.Distance(atPosition - (Vector2)transform.position, this.transform.position) < disc.Radius) return true;
                else return false;
            }
            case detectionType.Collider2D:
            {
                Collider2D coll = GetComponent<Collider2D>();
                if(coll.OverlapPoint(atPosition)) return true;
                else return false;
            }
            default: return false;
        }  
    }

    public virtual void RecordTouchLocation(Vector2 position)
    {
        touchLocation = position;
    }

}
