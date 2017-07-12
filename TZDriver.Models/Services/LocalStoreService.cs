using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TZDriver.Models.Services.IServices;
using TZDriver.Models.Tools.Utilities;
using Windows.Storage;

namespace TZDriver.Models.Services
{
    public class LocalStoreService : ILocalStoreService
    {
        /// <summary>
        /// Save the location data to a Local Folder storage.
        /// T obj is of type IEnumerable<T> data
        /// </summary>
        /// <param name="data">The data to save.</param>
        /// <param name="key">The file name.</param>
        public async Task<bool> SaveLocalData<T>(string key, IEnumerable<T> data) where T : class
        {
            var fencejson = JsonConvert.SerializeObject(data);
            StorageFolder _local = ApplicationData.Current.LocalFolder;

            try
            {
                var dataFolder = await _local.CreateFolderAsync(Constants.LocalFolderIdentifier, CreationCollisionOption.OpenIfExists);
                var file = await dataFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, fencejson);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Load the saved location data from Local Folder storage.
        /// T obj is of type List<T>
        /// </summary>
        /// <param name="key">The file name.</param>
        public async Task<T> LoadLocalData<T>(string key) where T : class
        {
            T data = null;
            StorageFolder _local = ApplicationData.Current.LocalFolder;
            try
            {
                var dataFolder = await _local.GetFolderAsync(Constants.LocalFolderIdentifier);
                var file = await dataFolder.GetFileAsync(key);
                string text = await FileIO.ReadTextAsync(file);
                data = JsonConvert.DeserializeObject<T>(text);
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }

            return data ?? default(T);
        }

        public async Task<T> ToObjectAsync<T>(string value) where T : class
        {
            return await Task.Run<T>(() =>
            {
                return JsonConvert.DeserializeObject<T>(value);
            });
        }

        public async Task<string> StringifyAsync(object value)
        {
            return await Task.Run<string>(() =>
            {
                return JsonConvert.SerializeObject(value);
            });
        }
    }
}
