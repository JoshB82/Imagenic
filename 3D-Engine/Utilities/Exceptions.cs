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

        #region Methods

        internal static T GenerateException<T>(params string[] parameters) where T : Exception
        {
            string message = typeof(EngineExceptions).GetField($"{typeof(T).Name[0..^"Exception".Length]}Message").GetValue(null).ToString();

            message = string.Format(message, parameters);

            return Activator.CreateInstance(typeof(T), message) as T;
        }

        #endregion
    }

    internal interface IEngineException<T> where T : Exception
    {
        internal string NoneVerbosityText { get; }
        internal string BriefVerbosityText { get; }
        internal string DetailedVerbosityText { get; }
        internal string AllVerbosityText { get; }

        internal T WithParameters(params string[] parameters)
        {
            string message = Properties.Settings.Default.Verbosity switch
            {
                Verbosity.None => NoneVerbosityText,
                Verbosity.Brief => BriefVerbosityText,
                Verbosity.Detailed => DetailedVerbosityText,
                Verbosity.All => AllVerbosityText,
                _ => throw new Exception("Cannot handle setting.")
            };

            return Activator.CreateInstance(typeof(T), string.Format(message, parameters)) as T;
        }
    }

    #region Exceptions

    [Serializable]
    public class VectorCannotBeZeroException : Exception, IEngineException<VectorCannotBeZeroException>
    {
        internal string NoneVerbosityText => "";
        internal string BriefVerbosityText => "";
        internal string DetailedVerbosityText => "";
        internal string AllVerbosityText => "";

        public VectorCannotBeZeroException() { }
        public VectorCannotBeZeroException(string message) : base(message) { }
        public VectorCannotBeZeroException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class Matrix4x4DoesNotHaveAnInverseException : InvalidOperationException
    {
        public Matrix4x4DoesNotHaveAnInverseException() { }
        public Matrix4x4DoesNotHaveAnInverseException(string message) : base(message) { }
        public Matrix4x4DoesNotHaveAnInverseException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class InvalidPixelFormatException : Exception
    {
        public InvalidPixelFormatException() { }
        public InvalidPixelFormatException(string message) : base(message) { }
        public InvalidPixelFormatException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ArrayLengthTooLowException : ArgumentException
    {
        public ArrayLengthTooLowException() { }
        public ArrayLengthTooLowException(string message) : base(message) { }
        public ArrayLengthTooLowException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ParameterCannotBeNullException : ArgumentNullException
    {
        public ParameterCannotBeNullException() { }
        public ParameterCannotBeNullException(string message) : base(message) { }
        public ParameterCannotBeNullException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class FileDoesNotExistException : Exception
    {
        public FileDoesNotExistException() { }
        public FileDoesNotExistException(string message) : base(message) { }
        public FileDoesNotExistException(string message, Exception inner) : base(message, inner) { }
    }

    #endregion
}