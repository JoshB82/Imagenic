using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

public class TextureStyle : FaceStyle
{
    #region Fields and Properties

    // Appearance
    public Texture DisplayTexture { get; set; }

    // Texture space values
    public Vector3D T1 { get; set; }
    public Vector3D T2 { get; set; }
    public Vector3D T3 { get; set; }

    #endregion

    #region Constructors

    public TextureStyle([DisallowNull] Texture texture,
                        Vector3D t1,
                        Vector3D t2,
                        Vector3D t3)
    {
        ThrowIfNull(texture);
        DisplayTexture = texture;

        ThrowIfNull(t1, t2, t3);
        T1 = t1;
        T2 = t2;
        T3 = t3;
    }

    #endregion
}