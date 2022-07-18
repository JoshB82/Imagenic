using Imagenic.Core.Entities.TransformableEntities.TranslatableEntities;
using Imagenic.Core.Entities.TransformableEntities.TranslatableEntities.OrientatedEntities.PhysicalEntities;
using Imagenic.Core.Utilities.Node;
using System;

namespace Imagenic.Core.Entities.TransformableEntities;

//public delegate TTranslatableEntity Translate<TTranslatableEntity>(Vector3D displacement);

public enum TransformationType
{
    Orientation,
    Reflection,
    Scaling,
    Translation
}

public class TransformationNoInputNoOutputNode<TTransformableEntity> : Node<Action<TTransformableEntity>>
    where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public Action<TTransformableEntity> Transformation { get; }

    #endregion

    #region Constructors

    public TransformationNoInputNoOutputNode(Action<TTransformableEntity> transformation)
    {
        Transformation = transformation;
    }

    #endregion
}

public class TransformationOneInputNoOutputNode<TTransformableEntity, TInput> : Node<Action<TTransformableEntity, TInput>>
    where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public Action<TTransformableEntity, TInput> Transformation { get; }

    #endregion

    #region Constructors

    public TransformationOneInputNoOutputNode(Action<TTransformableEntity, TInput> transformation)
    {
        Transformation = transformation;
    }

    #endregion
}

public class TransformationCascadeNode<TTransformableEntity, TInput> : Node<Func<TTransformableEntity, TInput>>
    where TTransformableEntity : TransformableEntity
{
    #region Constructors

    public TransformationCascadeNode(Func<TTransformableEntity, TInput> cascadeFunction) : base(cascadeFunction) { }

    #endregion
}

public sealed class TranslationNode<TTranslatableEntity> : TransformationCascadeNode<TTranslatableEntity, Vector3D>
    where TTranslatableEntity : TranslatableEntity
{
    #region Fields and Properties

    public Vector3D Displacement { get; }

    #endregion

    #region Constructors

    public TranslationNode(Vector3D displacement) : base(e => e.WorldOrigin += displacement)
    {
        Displacement = displacement;
    }

    #endregion
}

public sealed class OrientationNode<TOrientatedEntity> : TransformationCascadeNode<TOrientatedEntity, Orientation>
    where TOrientatedEntity : OrientatedEntity
{
    #region Fields and Properties

    public Orientation Orientation { get; }

    #endregion

    #region Constructors

    public OrientationNode(Orientation orientation) : base(e => e.WorldOrientation = orientation)
    {
        Orientation = orientation;
    }

    #endregion
}

public sealed class ScalingNode<TPhysicalEntity> : TransformationCascadeNode<TPhysicalEntity, Vector3D>
    where TPhysicalEntity : PhysicalEntity
{
    #region Fields and Properties

    public Vector3D ScaleFactor { get; }

    #endregion

    #region Constructors

    public ScalingNode(Vector3D scaleFactor) : base(e => e.Scaling = scaleFactor)
    {
        ScaleFactor = scaleFactor;
    }

    #endregion
}

public sealed class ReflectionNode<TOrientatedEntity> : TransformationCascadeNode<TOrientatedEntity, Vector3D>
    where TOrientatedEntity : OrientatedEntity
{
    #region Fields and Properties

    public Vector3D ReflectionAxis { get; }

    #endregion

    #region Constructors

    public ReflectionNode(Vector3D reflectionAxis) : base(e => e)
    {
        ReflectionAxis = reflectionAxis;
    }

    #endregion
}