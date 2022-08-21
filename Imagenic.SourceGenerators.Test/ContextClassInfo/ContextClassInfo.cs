namespace Imagenic.SourceGenerators.CountableContexts.ContextClassInfo
{
    internal class ContextClassInfo
    {
        #region Fields and Properties

        internal string GeneralName { get; }
        internal int Iteration { get; }
        internal ContextRange Range { get; }

        #endregion

        #region Constructors

        public ContextClassInfo(ContextRange range, string generalName, int iteration)
        {
            Range = range;
            GeneralName = generalName;
            Iteration = iteration;
        }

        #endregion
    }
}