using MongoDB.Bson;

namespace Acme.Todoist.Infrastructure.Utils
{
    /// <summary>
    /// Generator of keys randomly generated
    /// </summary>
    public interface IKeyGenerator
    {
        /// <summary>
        /// Generates random number.
        /// </summary>
        int GenerateNumber(int min = 1, int max = 999999);

        /// <summary>
        /// Generates a unique value to be used as identifier.
        /// </summary>
        string Generate();
    }

    /// <summary>
    /// Generator of keys randomly generated.
    /// </summary>
    public class KeyGenerator : IKeyGenerator
    {
        /// <summary>
        /// Generates random number.
        /// </summary>
        /// <returns></returns>
        public int GenerateNumber(int min, int max) => new Random().Next(min, max);

        /// <inheritdoc />
        // public string Generate() => SequentialGuid.NewGuid().ToString("N");
        public string Generate() => ObjectId.GenerateNewId().ToString().ToLower();
    }
}
