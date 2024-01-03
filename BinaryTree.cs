using System;
using System.Collections.Generic;
using System.Linq;

namespace CENG307_HW1
{
    class BTNode
    {
        public int data { get; set; }
        public int index { get; set; }
        public bool isRight { get; set; }
        public BTNode parent { get; set; }
        public BTNode()
        {
            data = -1;
            index = -1;
        }
        public BTNode(int data, int index)
        {
            this.data = data;
            this.index = index;
        }
    }
    class BTTable
    {
        public int tableSize { get; set; }
        public List<int> blankIndexes { get; set; }
        public BTNode[] table { get; set; }
        public BTTable(int tableSize)
        {
            this.tableSize = tableSize;
            blankIndexes = Enumerable.Range(0, tableSize).ToList<int>();
            table = new BTNode[tableSize];
        }
        private int HashFunction(int data)
        {
            return data % tableSize;
        }
        private int QuotientFunction(int data)
        {
            if (data < tableSize)
                return 1;
            else
                return (data / tableSize) % tableSize;
        }
        public void BinaryTreeInsert(int data)
        {
            List<BTNode> tempTable = new List<BTNode>();
            int homeIndex = HashFunction(data);
            int dataIncrement = QuotientFunction(data);

            bool isRight;
            bool isFound = true;

            BTNode newNode = new BTNode(data, -1);
            int foundedIndex = -1;

            if (table[homeIndex] == null)
            {
                table[homeIndex] = new BTNode(data, homeIndex); ;
                blankIndexes.Remove(homeIndex);
            }
            else
            {
                if (blankIndexes.Count > 0)
                {
                    BTNode trackNode = null;

                    tempTable.Add(newNode);

                    BTNode parentNode = table[homeIndex];
                    parentNode.parent = newNode;
                    parentNode.isRight = true;
                    tempTable.Add(parentNode);
                    int leftIndex = (homeIndex + dataIncrement) % tableSize;
                    if (table[leftIndex] != null)
                    {
                        BTNode childNode = new BTNode(table[leftIndex].data, table[leftIndex].index);
                        childNode.parent = parentNode;
                        childNode.isRight = false;
                        tempTable.Add(childNode);
                    }
                    else
                    {
                        isFound = false;
                        foundedIndex = leftIndex;
                        BTNode childNode = new BTNode(-1, leftIndex); // data onemsiz, agacta index bulmaya calisiyoruz
                        childNode.parent = parentNode;
                        childNode.isRight = false;
                        tempTable.Add(childNode);
                        trackNode = childNode;
                        blankIndexes.Remove(foundedIndex);
                    }

                    while (isFound)
                    {
                        if (tempTable.Count % 2 != 0)
                        {
                            isRight = true;
                        }
                        else
                        {
                            isRight = false;
                        }

                        int holderIndex = tempTable.Count / 2;
                        BTNode tempNode = tempTable[holderIndex];
                        int tempIndex;
                        if (isRight)
                        {
                            tempIndex = QuotientFunction(tempNode.data);
                            tempIndex = (tempNode.index + tempIndex) % tableSize;
                        }
                        else
                        {
                            BTNode tempNode2 = tempTable[holderIndex];
                            while (!tempNode2.isRight)
                            {
                                tempNode2 = tempNode2.parent;
                            }
                            tempIndex = QuotientFunction(tempNode2.parent.data);
                            tempIndex = (tempNode.index + tempIndex) % tableSize;

                        }
                        if (table[tempIndex] != null)
                        {
                            BTNode childNode = new BTNode(table[tempIndex].data, table[tempIndex].index);
                            childNode.parent = tempNode;
                            childNode.isRight = isRight;
                            tempTable.Add(childNode);
                        }
                        else
                        {
                            isFound = false;
                            foundedIndex = tempIndex;
                            BTNode childNode = new BTNode(-1, tempIndex);
                            childNode.parent = tempNode;
                            childNode.isRight = isRight;
                            tempTable.Add(childNode);
                            trackNode = childNode;
                            blankIndexes.Remove(foundedIndex);
                        }

                    }

                    while (trackNode.index != -1)
                    {
                        if (trackNode.isRight)
                        {
                            BTNode tempTrack = new BTNode(trackNode.parent.data, foundedIndex);
                            table[foundedIndex] = tempTrack;
                            foundedIndex = trackNode.parent.index;
                            trackNode = trackNode.parent;
                        }
                        else
                        {
                            trackNode = trackNode.parent;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Binary table is full.");
                }
            }
        }
        public int ProbeCount(int searchKey)
        {
            int homeIndex = HashFunction(searchKey);
            int increment = QuotientFunction(searchKey);

            if (table[homeIndex] == null)
            {
                return -1;
            }

            if (table[homeIndex].data == searchKey)
            {
                return 1;
            }

            else
            {
                int probe = 1;
                int temp = homeIndex;
                while (table[temp].data != searchKey)
                {
                    probe++;
                    temp = (temp + increment) % tableSize;
                    if (table[temp] == null)
                    {
                        return -1;
                    }
                    else if (table[temp].data == searchKey)
                    {
                        return probe;
                    }
                }
            }
            return -1;
        }
        public float AverageProbeCount()
        {
            int totalProbes = 0;
            int elementCount = 0;
            for (int i = 0; i < tableSize; i++)
            {
                if (table[i] == null)
                {
                    continue;
                }
                else
                {
                    elementCount++;
                    totalProbes += ProbeCount(table[i].data);
                }
            }
            return (float)totalProbes / elementCount;
        }
        public void PrintTable()
        {
            for (int i = 0; i < tableSize; i++)
            {
                if (table[i] != null)
                    Console.WriteLine("Index[{0}]: {1} -> Probe sayisi: {2}", i, table[i].data, ProbeCount(table[i].data));
                else
                    Console.WriteLine("Index[{0}]: ---", i);
            }
        }
    }
}