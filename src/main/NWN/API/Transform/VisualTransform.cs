using System.Numerics;
using NWN.Core;

namespace NWN.API
{
  public class VisualTransform
  {
    public readonly VisualTransform Default = new VisualTransform(Vector3.One, Vector3.One);

    public float Scale;
    public Vector3 Translation;
    public Vector3 Rotation;
    public float AnimSpeed;

    public VisualTransform() {}

    public VisualTransform(Vector3 translation, Vector3 rotation, float scale = 1f, float animSpeed = 1f)
    {
      Scale = scale;
      Translation = translation;
      Rotation = rotation;
      AnimSpeed = animSpeed;
    }

    internal VisualTransform(NwGameObject gameObject)
    {
      Scale = GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformScale);
      Translation = new Vector3(GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformTranslateX),
        GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformTranslateY),
        GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformTranslateZ));
      Rotation = new Vector3(GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformRotateX),
        GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformRotateY),
        GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformRotateZ));
      AnimSpeed = GetValue(gameObject, VisualTransformProperty.ObjectVisualTransformAnimationSpeed);
    }

    internal void Apply(NwGameObject gameObject)
    {
      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformScale, Scale);

      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformTranslateX, Translation.X);
      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformTranslateY, Translation.Y);
      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformTranslateZ, Translation.Z);

      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformRotateX, Rotation.X);
      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformRotateY, Rotation.Y);
      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformRotateZ, Rotation.Z);

      SetValue(gameObject, VisualTransformProperty.ObjectVisualTransformAnimationSpeed, AnimSpeed);
    }

    private float GetValue(NwGameObject obj, VisualTransformProperty prop)
    {
      return NWScript.GetObjectVisualTransform(obj, (int) prop);
    }

    private void SetValue(NwGameObject obj, VisualTransformProperty prop, float value)
    {
      NWScript.SetObjectVisualTransform(obj, (int) prop, value);
    }

    private enum VisualTransformProperty
    {
      ObjectVisualTransformScale = 10,
      ObjectVisualTransformRotateX = 21,
      ObjectVisualTransformRotateY = 22,
      ObjectVisualTransformRotateZ = 23,
      ObjectVisualTransformTranslateX = 31,
      ObjectVisualTransformTranslateY = 32,
      ObjectVisualTransformTranslateZ = 33,
      ObjectVisualTransformAnimationSpeed = 40
    }
  }
}