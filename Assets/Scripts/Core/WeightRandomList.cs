using System;
using System.Collections.Generic;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobileRpg.Core
{
    [Serializable]
    public class WeightRandomList<T>
    {
        [Serializable]
        public struct Pair
        {
            public T Item => _item;
            public float Weight => _weight;
            
            [SerializeField] private T _item;
            [SerializeField] private float _weight;
            

            public Pair(T item, float weight)
            {
                _item = item;
                _weight = weight;
            }
        }

        [SerializeField] private List<Pair> _list = new();
        

        public int Count => _list.Count;

        public void Add(T item, float weight)
        {
            _list.Add(new Pair(item, weight));
        }

        public T GetRandom()
        {
            float totalWeight = 0f;

            foreach (var p in _list)
            {
                totalWeight += p.Weight;
            }

            float value = Random.value * totalWeight;

            float sumWeight = 0f;

            foreach (var p in _list)
            {
                sumWeight += p.Weight;

                if (sumWeight >= value)
                    return p.Item;
            }

            return default;
        }

        public List<Pair> GetPairs() => _list;

    }
}