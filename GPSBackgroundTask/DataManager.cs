﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using System.IO;
using Windows.UI.Notifications;
using Windows.Foundation;

namespace GPSBackgroundTask
{
    public static class DataManager
    {
        // Name of our storage file
        private const string storageFile = "GPSBrutData.json";
        private const string clusterFile = "GPSClusterData.json";


        #region EXPOSED METHODS
        // PUBLIC exposed operations of the DataManager ==> avoid error for exposed Task
        public static IAsyncOperation<bool> SaveDataAsync(IList<GPSElement> positions)
        {
            return DataManager.WriteInFile(positions).AsAsyncOperation();
        }



        public static IAsyncOperation<IList<GPSElement>> GetGPSBrutDataAsync()
        {
            return DataManager.ReadGPSBrutDataAsync().AsAsyncOperation();
        }

        

        #endregion



        #region HIDDEN METHODS
        // Real tasks to perform
        // Read and deserialize the element, return the list of GPSElement as cluster
        private static async Task<IList<GPSElement>> ReadGPSBrutDataAsync() 
        {
            List<GPSElement> cluster;
            var JSONSerializer = new DataContractJsonSerializer(typeof(List<GPSElement>));
            var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(storageFile);

            cluster = (List<GPSElement>)JSONSerializer.ReadObject(stream);

            return cluster;
        }

        // Write the List as 
        private static async Task<bool> WriteInFile (IList<GPSElement> positions)
        {
            if (positions == null)
            {
                return false;
            }

            var serializer = new DataContractJsonSerializer(typeof(List<GPSElement>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(storageFile, CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, positions);
            }

            return true;
        }
        #endregion
    }
}
