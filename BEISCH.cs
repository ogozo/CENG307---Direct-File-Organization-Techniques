using System;

namespace CENG307_HW1
{
    class BEISCHHashNode
    {
        public int Key { get; set; }
        public BEISCHHashNode Link { get; set; }
    }
    class BEISCHHashTable
    {
        private int size;
        private bool bottomOrTop = true;
        private BEISCHHashNode[] table;
        public BEISCHHashTable(int size)
        {
            this.size = size;
            table = new BEISCHHashNode[size];
        }
        private int HashFunction(int key)
        {
            return key % size;
        }
        public void BEISCHInsert(int key)
        {
            int homeIndex = HashFunction(key);
            BEISCHHashNode newNode = new BEISCHHashNode { Key = key };

            if (table[homeIndex] == null)
            {
                table[homeIndex] = newNode;
            }

            else
            {
                if (bottomOrTop)
                {
                    InsertFromBottom(newNode, homeIndex);
                }
                else
                {
                    InsertFromTop(newNode, homeIndex);
                }
            }
        }
        private void InsertFromBottom(BEISCHHashNode newNode, int homeIndex)
        {
            for (int i = table.Length - 1; i >= 0; i--)
            {
                if (table[i] == null)
                {
                    table[i] = newNode;
                    if (table[homeIndex].Link != null && (table[homeIndex].Key % size == homeIndex))
                    {
                        BEISCHHashNode temp = table[homeIndex].Link;
                        table[homeIndex].Link = newNode;
                        newNode.Link = temp;
                    }
                    else if (table[homeIndex].Link != null && table[homeIndex].Key % size != homeIndex)
                    {
                        BEISCHHashNode current = table[homeIndex];
                        while (current.Link != null)
                        {
                            current = current.Link;
                        }
                        current.Link = newNode;
                    }
                    else
                    {
                        table[homeIndex].Link = newNode;
                    }
                    bottomOrTop = !bottomOrTop;
                    break;
                }
            }
        }
        private void InsertFromTop(BEISCHHashNode newNode, int homeIndex)
        {
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] == null)
                {
                    table[i] = newNode;
                    if (table[homeIndex].Link != null && (table[homeIndex].Key % size == homeIndex))
                    {
                        BEISCHHashNode temp = table[homeIndex].Link;
                        table[homeIndex].Link = newNode;
                        newNode.Link = temp;
                    }
                    else if (table[homeIndex].Link != null && table[homeIndex].Key % size != homeIndex)
                    {
                        BEISCHHashNode current = table[homeIndex];
                        while (current.Link != null)
                        {
                            current = current.Link;
                        }
                        current.Link = newNode;
                    }
                    else
                    {
                        table[homeIndex].Link = newNode;
                    }
                    bottomOrTop = !bottomOrTop;
                    break;
                }
            }
        }
        public void PrintTable()
        {
            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    if (table[i].Link != null)
                    {
                        Console.WriteLine("Index[{0}]: {1} -> Link to Index[{2}] ve Probe sayisi: {3}", i, table[i].Key, GetNodeIndex(table[i].Link), ProbeNumber(table[i].Key));
                    }
                    else
                    {
                        Console.WriteLine("Index[{0}]: {1} -> Probe sayisi: {2}", i, table[i].Key, ProbeNumber(table[i].Key));
                    }
                }
                else
                {
                    Console.WriteLine("Index[{0}]: ---", i);
                }
            }
        }
        private int GetNodeIndex(BEISCHHashNode node)
        {
            for (int i = 0; i < size; i++)
            {
                if (table[i] == node)
                {
                    return i;
                }
            }
            return -1;
        }
        public int ProbeNumber(int key)
        {
            int homeIndex = HashFunction(key);

            if (table[homeIndex] == null)
            {
                return -1;
            }
            if (table[homeIndex].Key == key)
            {
                return 1;
            }
            else
            {
                int probe = 0;
                BEISCHHashNode current = table[homeIndex];
                while (current != null)
                {
                    probe++;
                    if (current.Key == key)
                    {
                        return probe;
                    }
                    current = current.Link;
                }
            }
            return -1;
        }
        public float AverageProbeCount()
        {
            int totalProbes = 0;
            int elementCount = 0;
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] == null)
                {
                    continue;
                }
                else
                {
                    elementCount++;
                    totalProbes += ProbeNumber(table[i].Key);
                }
            }

            return ((float)totalProbes / elementCount);
        }
    }
}