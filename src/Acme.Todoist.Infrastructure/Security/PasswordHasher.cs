using Acme.Todoist.Application.Core.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Acme.Todoist.Infrastructure.Security
{
    /// <summary>
    /// Implements password hashing.
    /// </summary>
    public sealed class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Gets or sets the number of iterations used when hashing passwords using PBKDF2. Default is 10,000.
        /// </summary>
        /// <value>
        /// The number of iterations used when hashing passwords using PBKDF2.
        /// </value>
        /// <remarks>
        /// This value is only used when the compatibility mode is set to 'V3'.
        /// The value must be a positive integer.
        /// </remarks>
        private const int IterationCount = 10_000;

        private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }

            return areSame;
        }

        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password"/>.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password"/>.</returns>
        public string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var hashPassword = HashPassword(
                password: password,
                rng: _rng,
                prf: KeyDerivationPrf.HMACSHA256,
                iterCount: IterationCount,
                saltSize: 16,
                numBytesRequested: 32);

            return Convert.ToBase64String(hashPassword);
        }

        /// <summary>
        /// Returns a <see cref="bool"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="password">The password supplied for comparison.</param>
        /// <returns>A <see cref="bool"/> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            try
            {
                // Read header information
                var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
                var iterCount = ReadNetworkByteOrder(decodedHashedPassword, 5);
                var saltLength = ReadNetworkByteOrder(decodedHashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 16)
                {
                    return false;
                }

                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subKeyLength = decodedHashedPassword.Length - 13 - salt.Length;
                if (subKeyLength < 16)
                {
                    return false;
                }
                byte[] expectedSubKey = new byte[subKeyLength];
                Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

                // Hash the incoming password and verify it
                byte[] subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subKeyLength);

                return ByteArraysEqual(subKey, expectedSubKey);
            }
            catch (Exception)
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        private static byte[] HashPassword(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested)
        {
            // Produce a version 3 (see comment above) text hash.
            byte[] salt = new byte[saltSize];
            rng.GetBytes(salt);
            byte[] subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subKey.Length];
            outputBytes[0] = 1;
            WriteNetworkByteOrder(outputBytes, 1, (int)prf);
            WriteNetworkByteOrder(outputBytes, 5, (int)iterCount);
            WriteNetworkByteOrder(outputBytes, 9, (int)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subKey, 0, outputBytes, 13 + saltSize, subKey.Length);
            return outputBytes;
        }

        private static int ReadNetworkByteOrder(IReadOnlyList<byte> buffer, int offset)
        {
            return buffer[offset] << 24 | buffer[offset + 1] << 16 | buffer[offset + 2] << 8 | buffer[offset + 3];
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, int value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }
    }
}
