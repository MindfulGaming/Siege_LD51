using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using TMPro;
using System.Linq;

public class FadeOutRenderGroup : UIAnimation
{
    private List<ShapeRenderer> shapes;
    private List<SpriteRenderer> sprites;
    private List<TextMeshPro> texts;

    private List<float> shapeStartOpacity;
    private List<float> spriteStartOpacity;
    private List<float> textStartOpacity;

    public EasingFunctions.EasingFunction easingFunction;
    public bool disableOnAnimationEnd;


    void SetOpacitiesToZero()
    {
        foreach(ShapeRenderer s in shapes)
        {
            SetShapeAlpha(s, 0f);
        }

        foreach(SpriteRenderer s in sprites)
        {
            s.color = new Color(s.color.r, s.color.g, s.color.b, 0f);
        }

        foreach(TextMeshPro t in texts)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, 0f);
        }
    }

    void FindRenderersInChildren()
    {
        shapes = GetComponentsInChildren<ShapeRenderer>().ToList();
        sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        texts = GetComponentsInChildren<TextMeshPro>().ToList();
    }

    void RecordStartOpacities()
    {
        shapeStartOpacity = new List<float>();
        spriteStartOpacity = new List<float>();
        textStartOpacity = new List<float>();

        foreach(ShapeRenderer s in shapes)
        {
            shapeStartOpacity.Add(s.Color.a);
        }

        foreach(SpriteRenderer s in sprites)
        {
            spriteStartOpacity.Add(s.color.a);
        }

        foreach(TextMeshPro t in texts)
        {
            textStartOpacity.Add(t.color.a);
        }
    }

    public override void StartAnimation(float duration)
    {
        FindRenderersInChildren();
        RecordStartOpacities();
        StartCoroutine(Animation(duration));
    }


    private IEnumerator Animation(float duration)
    {
        float timeElapsed = 0;
        float t;
        while (timeElapsed < duration)
        {
            t = timeElapsed / duration;
            t = EasingFunctions.DoEase(easingFunction, t);

            //fade in shapes
            for(int i = 0; i < shapes.Count; i++)
            {
                float a = Mathf.Lerp(shapeStartOpacity[i], 0f, t);
                SetShapeAlpha(shapes[i], a);
                //shapes[i].Color = new Color(shapes[i].Color.r, shapes[i].Color.g, shapes[i].Color.b, a);
            }

            //fade in sprites
            for(int i = 0; i < sprites.Count; i++)
            {
                float a = Mathf.Lerp(spriteStartOpacity[i], 0f, t);
                sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, a);
            }

            //fade in texts
            for(int i = 0; i < texts.Count; i++)
            {
                float a = Mathf.Lerp(textStartOpacity[i], 0f, t);
                texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, a);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetOpacitiesToZero();

        if(disableOnAnimationEnd) this.gameObject.SetActive(false);

    }

    void SetShapeAlpha(ShapeRenderer shape, float a)
    {
            shape.Color = new Color(shape.Color.r, shape.Color.g, shape.Color.b, a);
            shape.UpdateMaterialProperties();
    }


}
