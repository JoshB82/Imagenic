/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines exceptions and their messages.
 */

using _3D_Engine.Enums;
using System;

namespace _3D_Engine.Constants
{
    internal static class Exceptions
    {
        // Format:
        // <struct/class name that uses the exception><brief description of message content>

        #region Exceptions

        internal static readonly ArgumentException Angle = new("Cannot calculate angle with one or more zero vectors.");
        internal static readonly ArgumentException Normalise = new("Cannot normalise a zero vector.");
        internal static readonly ArgumentException QuaternionNormalise = new("Cannot normalise a zero quaternion.");
        internal static readonly NotSupportedException RenderingObjectTypeNotSupported = new("This type is not supported.");
        internal static readonly InvalidOperationException Matrix4x4NoInverse = new("Matrix4x4 does not have an inverse.");

        #endregion

        #region Messages

        internal const string Vector2DParameterLength = "Parameter \"elements\" must at least be of length 2.";
        internal const string Vector3DParameterLength = "Parameter \"elements\" must at least be of length 3.";
        internal const string FourParameterLength = "Parameter \"elements\" must at least be of length 4.";
        internal const string Matrix4x4ParameterSize = "Parameter \"elements\" must at least be of size 4x4.";

        internal const string RenderWidthLessThanZero = "Parameter \"renderWidth\" must be non-negative.";
        internal const string RenderHeightLessThanZero = "Parameter \"renderHeight\" must be non-negative.";
        internal const string SceneToRenderCannotBeNull = "Parameter \"sceneToRender\" cannot be null.";
        internal const string InvalidPixelFormatForRendering = "Parameter \"renderPixelFormat\" is invalid for rendering.";

        #endregion

    }

    internal static class EngineExceptions
    {
        #region Messages

        private const string VectorCannotBeZeroMessage = "Vector {0} cannot be zero.";
        private const string Matrix4x4DoesNotHaveAnInverseMessage = "Matrix4x4 does not have an inverse.";
        private const string ArrayLengthTooLowMessage = "Array length is too low (must be greater than {0}).";
        private const string ParameterCannotBeNullMessage = "{0} cannot be null.";
        private const string FileDoesNotExistMessage = "File {0} does not exist.";

        #endregion
    }

    public static class GenerateException
    {
        public static T WithParameters<T>(params string[] parameters) where T : Exception, IVerbose, new()
        {
            T engineException = new();
            string message = Properties.Settings.Default.Verbosity switch
            {
                Verbosity.None => string.Empty,
                Verbosity.Brief => engineException.BriefVerbosityText,
                Verbosity.Detailed => engineException.DetailedVerbosityText,
                Verbosity.All => engineException.AllVerbosityText,
                _ => throw new Exception("Cannot handle setting.")
            };

            return Activator.CreateInstance(typeof(T), string.Format(message, parameters)) as T;
        }
    }

    #region Exception Types

    [Serializable]
    public class VectorCannotBeZeroException : Exception, IVerbose
    {
        public string BriefVerbosityText => "";
        public string DetailedVerbosityText => "";
        public string AllVerbosityText => "";

        public VectorCannotBeZeroException() { }
        public VectorCannotBeZeroException(string message) : base(message) { }
        public VectorCannotBeZeroException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class Matrix4x4DoesNotHaveAnInverseException : InvalidOperationException, IVerbose
    {
        public string BriefVerbosityText => "";
        public string DetailedVerbosityText => "";
        public string AllVerbosityText => "";

        public Matrix4x4DoesNotHaveAnInverseException() { }
        public Matrix4x4DoesNotHaveAnInverseException(string message) : base(message) { }
        public Matrix4x4DoesNotHaveAnInverseException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class InvalidPixelFormatException : Exception, IVerbose
    {
        public string BriefVerbosityText => "";
        public string DetailedVerbosityText => "";
        public string AllVerbosityText => "";

        public InvalidPixelFormatException() { }
        public InvalidPixelFormatException(string message) : base(message) { }
        public InvalidPixelFormatException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ArrayLengthTooLowException : ArgumentException, IVerbose
    {
        public string BriefVerbosityText => "";
        public string DetailedVerbosityText => "";
        public string AllVerbosityText => "";

        public ArrayLengthTooLowException() { }
        public ArrayLengthTooLowException(string message) : base(message) { }
        public ArrayLengthTooLowException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ParameterCannotBeNullException : ArgumentNullException, IVerbose
    {
        public string BriefVerbosityText => "";
        public string DetailedVerbosityText => "";
        public string AllVerbosityText => "";

        public ParameterCannotBeNullException() { }
        public ParameterCannotBeNullException(string message) : base(message) { }
        public ParameterCannotBeNullException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class FileDoesNotExistException : Exception, IVerbose
    {
        public string BriefVerbosityText => "";
        public string DetailedVerbosityText => "";
        public string AllVerbosityText => "";

        public FileDoesNotExistException() { }
        public FileDoesNotExistException(string message) : base(message) { }
        public FileDoesNotExistException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class VectorsAreNotOrthogonalException : Exception, IVerbose
    {
        public string BriefVerbosityText => "Vectors are not orthogonal.";
        public string DetailedVerbosityText => "Vectors {0} and {1} are not orthogonal.";
        public string AllVerbosityText => "Vectors {0} and {1} are not orthogonal - the angle between them should be 90 degrees or pi/2 radians.";

        public VectorsAreNotOrthogonalException() { }
        public VectorsAreNotOrthogonalException(string message) : base(message) { }
        public VectorsAreNotOrthogonalException(string message, Exception inner) : base(message, inner) { }
    }

    #endregion
}