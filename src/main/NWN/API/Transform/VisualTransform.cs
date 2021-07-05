using System;
using System.Numerics;
using NWN.Core;

namespace NWN.API
{
  public sealed class VisualTransform
  {
    private readonly NwGameObject gameObject;
    private VisualTransformLerpSettings activeLerpSettings;

    public float Scale
    {
      get => GetValue(VisualTransformProperty.ObjectVisualTransformScale);
      set => SetValue(VisualTransformProperty.ObjectVisualTransformScale, value);
    }

    public float AnimSpeed
    {
      get => GetValue(VisualTransformProperty.ObjectVisualTransformAnimationSpeed);
      set => SetValue(VisualTransformProperty.ObjectVisualTransformAnimationSpeed, value);
    }

    public Vector3 Translation
    {
      get
      {
        float x = GetValue(VisualTransformProperty.ObjectVisualTransformTranslateX);
        float y = GetValue(VisualTransformProperty.ObjectVisualTransformTranslateY);
        float z = GetValue(VisualTransformProperty.ObjectVisualTransformTranslateZ);

        return new Vector3(x, y, z);
      }
      set
      {
        SetValue(VisualTransformProperty.ObjectVisualTransformTranslateX, value.X);
        SetValue(VisualTransformProperty.ObjectVisualTransformTranslateY, value.Y);
        SetValue(VisualTransformProperty.ObjectVisualTransformTranslateZ, value.Z);
      }
    }

    public Vector3 Rotation
    {
      get
      {
        float x = GetValue(VisualTransformProperty.ObjectVisualTransformRotateX);
        float y = GetValue(VisualTransformProperty.ObjectVisualTransformRotateY);
        float z = GetValue(VisualTransformProperty.ObjectVisualTransformRotateZ);

        return new Vector3(x, y, z);
      }
      set
      {
        SetValue(VisualTransformProperty.ObjectVisualTransformRotateX, value.X);
        SetValue(VisualTransformProperty.ObjectVisualTransformRotateY, value.Y);
        SetValue(VisualTransformProperty.ObjectVisualTransformRotateZ, value.Z);
      }
    }

    internal VisualTransform(NwGameObject gameObject)
    {
      this.gameObject = gameObject;
    }

    /// <summary>
    /// Lerps the specified transform changes using the specified settings.
    /// </summary>
    /// <param name="settings">The lerp settings to use when changing this visual transform.</param>
    /// <param name="transforms">An action containing the transform changes to be lerped.</param>
    public void Lerp(VisualTransformLerpSettings settings, Action<VisualTransform> transforms)
    {
      activeLerpSettings = settings;

      try
      {
        transforms?.Invoke(this);
      }
      finally
      {
        activeLerpSettings = null;
      }
    }

    /// <summary>
    /// Updates the transform data of this visual transform by copying another.
    /// </summary>
    /// <param name="other">The visual transform to copy.</param>
    public void Copy(VisualTransform other)
    {
      Scale = other.Scale;
      AnimSpeed = other.AnimSpeed;
      Translation = other.Translation;
      Rotation = other.Rotation;
    }

    private float GetValue(VisualTransformProperty property)
    {
      if (activeLerpSettings != null)
      {
        return NWScript.GetObjectVisualTransform(gameObject, (int)property, (!activeLerpSettings.ReturnDestinationTransform).ToInt());
      }

      return NWScript.GetObjectVisualTransform(gameObject, (int)property, false.ToInt());
    }

    private void SetValue(VisualTransformProperty property, float value)
    {
      if (activeLerpSettings != null)
      {
        NWScript.SetObjectVisualTransform(gameObject, (int)property, value, (int)activeLerpSettings.LerpType, (float)activeLerpSettings.Duration.TotalSeconds, activeLerpSettings.PauseWithGame.ToInt());
      }
      else
      {
        NWScript.SetObjectVisualTransform(gameObject, (int)property, value);
      }
    }

    private enum VisualTransformProperty
    {
      ObjectVisualTransformScale = NWScript.OBJECT_VISUAL_TRANSFORM_SCALE,
      ObjectVisualTransformRotateX = NWScript.OBJECT_VISUAL_TRANSFORM_ROTATE_X,
      ObjectVisualTransformRotateY = NWScript.OBJECT_VISUAL_TRANSFORM_ROTATE_Y,
      ObjectVisualTransformRotateZ = NWScript.OBJECT_VISUAL_TRANSFORM_ROTATE_Z,
      ObjectVisualTransformTranslateX = NWScript.OBJECT_VISUAL_TRANSFORM_TRANSLATE_X,
      ObjectVisualTransformTranslateY = NWScript.OBJECT_VISUAL_TRANSFORM_TRANSLATE_Y,
      ObjectVisualTransformTranslateZ = NWScript.OBJECT_VISUAL_TRANSFORM_TRANSLATE_Z,
      ObjectVisualTransformAnimationSpeed = NWScript.OBJECT_VISUAL_TRANSFORM_ANIMATION_SPEED,
    }
  }
}
