using Imagenic.Core.Enums;

namespace Imagenic.Core.Renderers.Animations;

public class AnimationOptions
{
    #region Fields and Properties

    public float Duration { get; set; }
    public float FrameRate { get; set; }

    public Repeat RepeatType { get; set; }
    public int RepeatCount { get; set; }

    #endregion

    #region Constructors

    public AnimationOptions(float duration)
    {
        Duration = duration;
    }

    #endregion
}