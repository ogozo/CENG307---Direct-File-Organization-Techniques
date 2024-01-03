using System;
using System.Collections.Generic;


namespace CENG307_HW1
{
    class Record
    {
        public int RecordValue { get; set; }
        public int Link { get; set; } 
        public Record(int record, short link)
        {
            RecordValue = record;
            Link = link;
        }
    }
    class ComputedChaining
    {
        public int tableSize;
        public Record[] Table;
        public ComputedChaining(int size)
        {
            tableSize = size;
            Table = new Record[tableSize];
        }
        private int HashFunction(int key)
        {
            return key % tableSize;
        }
        private int QuotientFunction(int key)
        {
            int x = ((key / tableSize) % tableSize);
            if (x == 0) return 1;
            else return x;
        }
        public void ComputedChainingInsert(int record)
        {
            Record newRecord = new Record(record, -1);
            int homeIndex = HashFunction(record);

            if (Table[homeIndex] == null)
            {
                Table[homeIndex] = newRecord;
                return;
            }
            else if (Table[homeIndex].RecordValue == record) // duplicate record
            {
                return;
            }

            else if (Table[homeIndex] != null && (HashFunction(Table[homeIndex].RecordValue) == homeIndex)) // home address chaining
            {
                if (Table[homeIndex].Link == -1)
                {
                    int increment = QuotientFunction(Table[homeIndex].RecordValue);
                    int howManySteps = 0;
                    int i = homeIndex;

                    while (Table[i] != null)
                    {
                        howManySteps++;
                        i = (i + increment) % tableSize;
                    }
                    Table[i] = newRecord;
                    Table[homeIndex].Link = howManySteps;
                }

                else
                {
                    int current = homeIndex;
                    while (Table[current].Link != -1)
                    {
                        current = (current + (QuotientFunction(Table[current].RecordValue) * Table[current].Link));
                        current = current % tableSize;
                        if (Table[current].RecordValue == record)
                        {
                            return;
                        }
                    }

                    int increment = QuotientFunction(Table[current].RecordValue);
                    int howManySteps = 0;
                    int i = current;

                    while (Table[i] != null)
                    {
                        howManySteps++;
                        i = (i + increment) % tableSize;
                    }
                    Table[i] = newRecord;
                    Table[current].Link = howManySteps;
                }
            }

            else if (Table[homeIndex] != null && (HashFunction(Table[homeIndex].RecordValue) != homeIndex)) // not home address chaining
            {
                int current = homeIndex;
                Queue<Record> queue = new Queue<Record>();
                queue.Enqueue(Table[current]);
                while (Table[current].Link != -1)
                {
                    current = (current + (QuotientFunction(Table[current].RecordValue) * Table[current].Link)) % tableSize;
                    queue.Enqueue(Table[current]);
                }
                RemoveChain(homeIndex);

                ComputedChainingInsert(record);
                while (queue.Count > 0)
                {
                    int tempVal = queue.Peek().RecordValue;
                    queue.Dequeue();
                    ComputedChainingInsert(tempVal);
                }
            }
        }
        private void RemoveChain(int indexToRemove)
        {
            int current = indexToRemove;
            int last = -1;
            int prev = -1;
            int initialIndex = HashFunction(Table[indexToRemove].RecordValue);

            while (Table[initialIndex].RecordValue != Table[indexToRemove].RecordValue)
            {
                prev = initialIndex;
                initialIndex = initialIndex + (QuotientFunction(Table[initialIndex].RecordValue) * Table[initialIndex].Link);
                initialIndex = initialIndex % tableSize;
            }
            if (prev == -1) { prev = initialIndex; }
            Table[prev].Link = -1;

            while (Table[current].Link != -1)
            {
                last = current;
                int next = (current + (QuotientFunction(Table[current].RecordValue) * Table[current].Link));
                next = next % tableSize;
                Table[current] = null;
                current = next;
            }

            Table[current] = null;

            if (last != -1 && Table[last] != null)
            {
                Table[last].Link = -1;
            }
        }
        public void DisplayHashTableContents()
        {
            for (int i = 0; i < tableSize; i++)
            {
                if (Table[i] != null)
                {
                    Console.WriteLine($"Index[{i}] = {Table[i].RecordValue}, nof = {Table[i].Link} -> Probe sayisi = {FindProbeCount(Table[i].RecordValue)}");
                }
                else
                {
                    Console.WriteLine($"Index[{i}] = NULL");
                }
            }
        }
        public int FindProbeCount(int recordToSearch)
        {
            int homeIndex = HashFunction(recordToSearch);

            if (Table[homeIndex] == null)
            {
                return -1;
            }
            if (Table[homeIndex].RecordValue == recordToSearch)
            {
                return 1;
            }
            else
            {
                int probeCount = 1;
                int temp = homeIndex;
                while (Table[temp].RecordValue != recordToSearch)
                {
                    probeCount++;
                    temp = (temp + (QuotientFunction(Table[temp].RecordValue) * Table[temp].Link)) % tableSize;
                    if (Table[temp] == null || Table[temp].Link == -1 && Table[temp].RecordValue != recordToSearch)
                    {
                        return -1;
                    }
                }
                if (Table[temp].RecordValue == recordToSearch)
                {
                    return probeCount;
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
                if (Table[i] == null)
                {
                    continue;
                }
                else
                {
                    elementCount++;
                    totalProbes += FindProbeCount(Table[i].RecordValue);
                }
            }
            return (float)totalProbes / elementCount;
        }
    }
}