using System.Collections.Generic;
using System.Threading.Tasks;

namespace TZDriver.Models.Services.IServices
{
    public interface ILocalStoreService
    {
        /// <summary>
        /// Save the location data to a Local Folder storage.
        /// T obj is of type IEnumerable<T> data
        /// </summary>
        /// <param name="data">The data to save.</param>
        /// <param name="key">The file name.</param>
        Task<bool> SaveLocalData<T>(string key, IEnumerable<T> data) where T : class;

        /// <summary>
        /// Load the saved location data from Local Folder storage.
        /// T obj is of type List<T>
        /// </summary>
        /// <param name="key">The file name.</param>
        Task<T> LoadLocalData<T>(string key) where T : class;

        Task<T> ToObjectAsync<T>(string value) where T : class;

        Task<string> StringifyAsync(object value);
    }
}
