using System.Security.Cryptography;
using System.Text;

namespace Framework.Security
{
    public class HashAlgorithm
    {
        private readonly int _keySize;
        private readonly int _iteration;
        private readonly HashAlgorithmName _hashAlgorithmName;

        public HashAlgorithm(int keySize, int iteration, HashAlgorithmName hashAlgorithmName)
        {
            _keySize = keySize;
            _iteration = iteration;
            _hashAlgorithmName = hashAlgorithmName;
        }

        public HashAlgorithm()
        {
            this._keySize = 64;
            this._iteration = 350000;
            this._hashAlgorithmName = HashAlgorithmName.SHA512;
        }
        public string GetHash(string input, out byte[] salt)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            salt = RandomNumberGenerator.GetBytes(_keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(input), salt, _iteration, _hashAlgorithmName, _keySize);

            return Convert.ToHexString(hash);
        }

        public bool VerifyHash(string input, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(input), salt, _iteration, _hashAlgorithmName, _keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        public string GetSha256HashWithoutSalt(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            using var hashAlgorithm = SHA512.Create();

            var value = Encoding.UTF8.GetBytes(input);

            var hash = hashAlgorithm.ComputeHash(value);

            return Convert.ToBase64String(hash);
        }
        public bool VerifySha256HashWithoutSalt(string input, string hash)
        {
            var computeHash = GetSha256HashWithoutSalt(input);

            return computeHash.Equals(hash);
        }
    }
}
