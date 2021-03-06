﻿using System;
using YAMP;
using YAMP.Numerics;

namespace YAMP.Physics
{
    [Description("In mathematics, the Hermite polynomials are a classical orthogonal polynomial sequence that arise in probability, such as the Edgeworth series; in combinatorics, as an example of an Appell sequence, obeying the umbral calculus; in numerical analysis as Gaussian quadrature; in finite element methods as Shape Functions for beams; and in physics, where they give rise to the eigenstates of the quantum harmonic oscillator. They are also used in systems theory in connection with nonlinear operations on Gaussian noise.")]
    [Kind(PopularKinds.Function)]
    class HermiteFunction : ArgumentFunction
    {
        [Description("Evaluates the Hermite polynomial of some order n at the given point z in C.")]
        [Example("hermite(2, 1.5)", "Evaluates the Hermite polynomial of order 2 at the point z = 1.5.")]
        public ScalarValue Function(ScalarValue n, ScalarValue z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Hermite polynomial of order n < 0 does not make sense.");

            return HermitePolynomial(nn, z);
        }

        [Description("Evaluates the Hermite polynomial of some order n at the given points of the matrix Z in C.")]
        [Example("hermite(3, [0, 0.5, 1.0 1.5])", "Evaluates the Hermite polynomial of order 3 at the points z = 0, 0.5, 1.0 and 1.5.")]
        public MatrixValue Function(ScalarValue n, MatrixValue Z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Hermite polynomial of order n < 0 does not make sense.");

            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.Length; i++)
                M[i] = HermitePolynomial(nn, Z[i]);

            return M;
        }

        #region ALgorithm

        /// <summary>
        /// Returns the Hermite polynomials of order m.
        /// </summary>
        /// <param name="m">Order</param>
        /// <param name="z">z</param>
        /// <returns>Value</returns>
        static ScalarValue HermitePolynomial(int m, ScalarValue z)
        {
            var mh = m / 2;
            var p = ScalarValue.Zero;
            var two_z = z * 2;
            var two_z_squared = two_z * two_z;
            var two_z_to_the_n_minus_2_times_m = m % 2 == 0 ? ScalarValue.One : two_z;
            var s = Math.Pow(-1, mh);

            for (int k = mh; k >= 0; k--)
            {
                p += s / (Helpers.Factorial(k) * Helpers.Factorial(m - 2 * k)) * two_z_to_the_n_minus_2_times_m;
                s *= (-1);
                two_z_to_the_n_minus_2_times_m *= two_z_squared;
            }

            return p * Helpers.Factorial(m);
        }

        #endregion
    }
}
