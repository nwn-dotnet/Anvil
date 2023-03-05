using System;
using System.Numerics;
using NWN.Core;

namespace Anvil.API
{
  public sealed class VisualTransform
  {
    private readonly NwGameObject gameObject;
    private readonly ObjectVisualTransformDataScope scope;
    private VisualTransformLerpSettings? activeLerpSettings;

    internal VisualTransform(NwGameObject gameObject, ObjectVisualTransformDataScope scope)
    {
      this.gameObject = gameObject;
      this.scope = scope;
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

    public float AnimSpeed
    {
      get => GetValue(VisualTransformProperty.ObjectVisualTransformAnimationSpeed);
      set => SetValue(VisualTransformProperty.ObjectVisualTransformAnimationSpeed, value);
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

    public float Scale
    {
      get => GetValue(VisualTransformProperty.ObjectVisualTransformScale);
      set => SetValue(VisualTransformProperty.ObjectVisualTransformScale, value);
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
        transforms.Invoke(this);
      }
      finally
      {
        activeLerpSettings = null;
      }
    }

    private float GetValue(VisualTransformProperty property)
    {
      if (activeLerpSettings != null)
      {
        return NWScript.GetObjectVisualTransform(gameObject, (int)property, (!activeLerpSettings.ReturnDestinationTransform).ToInt(), (int)scope);
      }

      return NWScript.GetObjectVisualTransform(gameObject, (int)property, false.ToInt(), (int)scope);
    }

    private void SetValue(VisualTransformProperty property, float value)
    {
      if (activeLerpSettings != null)
      {
        NWScript.SetObjectVisualTransform(gameObject, (int)property, value, (int)activeLerpSettings.LerpType, (float)activeLerpSettings.Duration.TotalSeconds, activeLerpSettings.PauseWithGame.ToInt(), (int)scope);
      }
      else
      {
        NWScript.SetObjectVisualTransform(gameObject, (int)property, value, (int)scope);
      }
    }
  }
}
