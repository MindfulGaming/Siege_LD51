using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using TMPro;
using System.Linq;

public class FadeInRenderGroup : UIAnimation
{
    private List<ShapeRenderer> shapes;
    private List<SpriteRenderer> sprites;
    private List<TextMeshPro> texts;

    private List<float> shapeEndOpacity;
    private List<float> spriteEndOpacity;
    private List<float> textEndOpacity;

    public EasingFunctions.EasingFunction easingFunction;
    public GameEvent eventOnAnimationEnd;
    public bool startAtZeroOpacity; //can be set to false in case an object is fading out, then fading in

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
        shapeEndOpacity = new List<float>();
        spriteEndOpacity = new List<float>();
        textEndOpacity = new List<float>();

        foreach(ShapeRenderer s in shapes)
        {
            shapeEndOpacity.Add(s.Color.a);
        }

        foreach(SpriteRenderer s in sprites)
        {
            spriteEndOpacity.Add(s.color.a);
        }

        foreach(TextMeshPro t in texts)
        {
            textEndOpacity.Add(t.color.a);
        }
    }

    public override void StartAnimation(float duration)
    {
        SetOpacitiesToZero();
        StartCoroutine(Animation(duration));
    }

    void Awake()
    {
        FindRenderersInChildren();
        RecordStartOpacities();
        if(startAtZeroOpacity) SetOpacitiesToZero(); //should this be optional? Are there fade in groups that start with their opacity not at zero?
    }

    void OnEnable()
    {
        if(startAtZeroOpacity) SetOpacitiesToZero(); //should this be optional? Are there fade in groups that start with their opacity not at zero?
    }

    void SetToStartOpacities()
    {
            for(int i = 0; i < shapes.Count; i++)
            {
                float a = shapeEndOpacity[i];
                SetShapeAlpha(shapes[i], a);
            }

            for(int i = 0; i < sprites.Count; i++)
            {
                float a = spriteEndOpacity[i];
                sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, a);
            }

            for(int i = 0; i < texts.Count; i++)
            {
                float a = textEndOpacity[i];
                texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, a);
            }

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
                float a = Mathf.Lerp(0f, shapeEndOpacity[i], t);
                SetShapeAlpha(shapes[i], a);
            }

            //fade in sprites
            for(int i = 0; i < sprites.Count; i++)
            {
                float a = Mathf.Lerp(0f, spriteEndOpacity[i], t);
                sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, a);
            }

            //fade in texts
            for(int i = 0; i < texts.Count; i++)
            {
                float a = Mathf.Lerp(0f, textEndOpacity[i], t);
                texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, a);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        SetToStartOpacities();
        if(eventOnAnimationEnd != null) eventOnAnimationEnd.Raise();
    }



    void SetShapeAlpha(ShapeRenderer shape, float a)
    {
            shape.Color = new Color(shape.Color.r, shape.Color.g, shape.Color.b, a);
            shape.UpdateMaterialProperties();  
    }

}
