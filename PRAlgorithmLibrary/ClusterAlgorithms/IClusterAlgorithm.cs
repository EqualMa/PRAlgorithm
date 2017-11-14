using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    public interface IClusterAlgorithm
    {
        List<List<Vector>> Cluster(Vector[] vectors);

        void SetParameter(string paraName, decimal paraValue);

        decimal GetParameterValue(string paraName);

        List<string> Parameters { get; }
    }


    public class ParameterNotExistException : Exception
    {
        public ParameterNotExistException(string message) : base(message) { }

    }
}
