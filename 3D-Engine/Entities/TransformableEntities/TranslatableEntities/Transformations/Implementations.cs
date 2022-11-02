using Imagenic.Core.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

internal static partial class TranslatableEntityTransformations_Implementations
{
    internal static TTranslatableEntity TranslateX<TTranslatableEntity>(
        [DisallowNull][ThrowIfNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        return translatableEntity.Transform(e => { e.WorldOrigin += new Vector3D(distance, 0, 0); });
    }
}