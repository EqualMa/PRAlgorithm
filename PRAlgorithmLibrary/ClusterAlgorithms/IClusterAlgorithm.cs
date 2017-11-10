using System;
using System.Collections.Generic;
using System.Text;

namespace PRAlgorithmLibrary.ClusterAlgorithms
{
    interface IClusterAlgorithm
    {
        List<List<Vector>> Cluster(Vector[] vectors);

        void SetParameter(string paraName, decimal paraValue);
    }


    public class ParameterNotExistException : Exception
    {
        public ParameterNotExistException(string message) : base(message) { }

    }
}
