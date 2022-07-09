using System;
using System.Linq;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Collections.Generic;

namespace TicTacToe
{
    public abstract class CellsAssociations<T> : ScriptableObject
    {
        [SerializeField] private List<CellResource<T>> _resources = new List<CellResource<T>>();
        
        private void OnEnable()
        {
            bool isErrorHappened = false;
            for (int resourceId = 0; resourceId < _resources.Count; resourceId++)
            {
                var originalResource = _resources[resourceId];
                var sameCellTypes = _resources.Where((r) => r.Type == _resources[resourceId].Type);

                if (sameCellTypes.Count() > 1)
                {
                    _resources.RemoveAll((r) => r.Type == _resources[resourceId].Type);
                    _resources.Add(originalResource);
                    isErrorHappened = true;

                    resourceId = 0;
                }
            }

            if (isErrorHappened)
                Debug.LogError($"Each {nameof(Cell)} type value can have only 1 {nameof(CellResource<string>)} instance!" +
                    $"Extra resources have been removed!");
        }

        public CellResource<T> GetResource(Cell cellTypeResource)
        {
            var resource = _resources.First((c) => c.Type == cellTypeResource);

            if (resource == null)
                throw new ArgumentException($"Resource with type {cellTypeResource} is not present in the database!");

            return resource;
        }
    }
}
