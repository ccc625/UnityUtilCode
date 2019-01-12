using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomEx
{
    public class RandomEx
    {
        public static Roulette<T> CreateRoulette<T>(int index, Dictionary<T, int> table)
        {
            return new Roulette<T>(table);
        }
    }

    public class Roulette<T>
    {
        private RandomTable<T> _randomTable;

        public Roulette(Dictionary<T, int> table)
        {
            _randomTable = new RandomTable<T>(table);
        }

        public T Peek()
        {
            return _randomTable.GetValue();
        }

        public void PrintSeedTable()
        {
            _randomTable.PrintSeedTable();
        }
    }

    internal class RandomTable<T>
    {
        public class Node
        {
            public int rate;
            public T value;
        }

        private List<Node> _seedTable;

        public RandomTable(Dictionary<T, int> table)
        {
            CreateTable(table);
        }

        public T GetValue()
        {
            int rate = UnityEngine.Random.Range(0, _seedTable[_seedTable.Count - 1].rate);

            Debug.LogWarning("selected Rate = " + rate);

            return SearchNode(rate).value;
        }

        public void CreateTable(Dictionary<T, int> table)
        {
            if (_seedTable == null)
                _seedTable = new List<Node>();

            int rate = 0;
            foreach (var seed in table)
            {
                Node node = new Node();
                node.value = seed.Key;
                node.rate = rate;

                _seedTable.Add(node);

                rate = node.rate + seed.Value;
            }
        }

        private Node SearchNode(int rate)
        {
            Node result = null;

            int min = 0;
            int max = _seedTable.Count - 1;

            int middle;

            while (true)
            {
                middle = min + (int)(((float)max - (float)min) / 2);

                result = _seedTable[middle];

                if (middle <= min || middle >= max)
                {
                    if (result.rate < rate)
                        result = _seedTable[max];
                    break;
                }
                
                if (result.rate > rate)
                {
                    max = middle;
                }
                else
                {
                    min = middle;
                }
            }

            return result;
        }

        public void PrintSeedTable()
        {
            foreach (var seed in _seedTable)
            {
                Debug.LogWarning(string.Format("[RandomEx] rate = {0}, value = {1}", seed.rate, seed.value));
            }
        }

        /* NOTE @jimin 트리 사용안함
        public void CreateTable(Dictionary<int, int> table)
        {
            foreach (var key in table.Keys)
            {
                if (root == null)
                {
                    root = new Node<int>(key, table[key]);
                }
                else
                {
                    Node<int> current = root;
                    while (true)
                    {
                        if (key < current.key)
                        {
                            if (current.left == null)
                            {
                                current.left = new Node<int>(key, table[key]);
                                break;
                            }

                            current = current.left;
                        }
                        else if (key > current.key)
                        {
                            if (current.right == null)
                            {
                                current.right = new Node<int>(key, table[key]);
                                break;
                            }

                            current = current.right;
                        }
                        else
                        {
                            Debug.LogError("key is duplicated!!");
                        }
                    }
                }
            }
        }
        //*/
    }
}
