using System;

namespace _3D_Engine.Constants
{
    internal static class Exceptions
    {
        // Format:
        // <struct/class name that uses the exception><brief description of message content>

        #region Exceptions

        internal static readonly ArgumentException Vector2DAngle = new("Cannot calculate angle with one or more zero vectors.");
        internal static readonly ArgumentException Vector2DNormalise = new("Cannot normalise a zero vector.");

        #endregion

        #region Messages

        internal const string Vector2DParameterLength = "Parameter \"elements\" must at least be of length 2.";

        #endregion
    }
}
