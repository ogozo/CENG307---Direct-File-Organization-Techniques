using System;

namespace CENG307_HW1
{
    // OGUZHAN TONGADUR
    internal class Program
    {
        static void FillTables(BEISCHHashTable beisch, ComputedChaining cc, BTTable bttable, int elementCount)
        {
            Random random = new Random();
            for (int i = 0; i < elementCount; i++)
            {
                int randomNumber = random.Next(1, Int16.MaxValue);
                beisch.BEISCHInsert(randomNumber);
                cc.ComputedChainingInsert(randomNumber);
                bttable.BinaryTreeInsert(randomNumber);
            }
        }
        static bool isPrime(int data)
        {
            if (data == 1) return false;
            if (data == 2) return true;

            var limit = Math.Ceiling(Math.Sqrt(data));

            for (int i = 2; i <= limit; ++i)
                if (data % i == 0)
                    return false;
            return true;
        }
        static void Main(string[] args)
        {
            BEISCHHashTable beischTable;
            ComputedChaining computedChainingTable;
            BTTable binaryTreeTable;
            int tableSize;
            while (true)
            {
                Console.Write("Asal bir sayi olmak sartiyla tablo boyutunu giriniz: ");
                tableSize = Convert.ToInt32(Console.ReadLine());
                if (tableSize <= 0 || tableSize > 1051 || !isPrime(tableSize))
                {
                    Console.Clear();
                    Console.WriteLine("Tablo boyutu 0'dan büyük 1051'den kucuk asal bir sayi olmalidir.");
                }
                else
                {
                    Console.Clear();
                    beischTable = new BEISCHHashTable(tableSize);
                    computedChainingTable = new ComputedChaining(tableSize);
                    binaryTreeTable = new BTTable(tableSize);
                    Console.WriteLine("Tablolar {0} boyutunda olusturuldu.", tableSize);
                    break;
                }
            }

            int tablePlus = tableSize + 1;
            int minTableElementCount = (tablePlus * 65) / 100;
            int maxTableElementCount = (tableSize * 95) / 100;
            if ((tableSize * 95) > 90000 && (tableSize * 95) < 100000)
            {
                maxTableElementCount = 1000;
                minTableElementCount = 684;
            }

            while (true)
            {
                Console.Write("Tablolara kac adet eleman eklensin?: ");
                int elementCount = Convert.ToInt32(Console.ReadLine());
                if (elementCount < (int)minTableElementCount || elementCount > (int)maxTableElementCount)
                {
                    Console.Clear();
                    Console.WriteLine("Packing factor %65 ile %95 arasinda olmasi icin tabloya {0} - {1} araliginda eleman eklenebilir.\n", (int)minTableElementCount, (int)maxTableElementCount);
                }
                else
                {
                    FillTables(beischTable, computedChainingTable, binaryTreeTable, elementCount);
                    Console.WriteLine("Tablolara {0} adet eleman eklendi.", elementCount);
                    Console.WriteLine("Packing factor: %{0}", Math.Floor(((float)elementCount / tableSize * 100)));
                    Console.WriteLine();
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("1- Tablolari goster");
                Console.WriteLine("2- Tablolarda arama yap");
                Console.WriteLine("3- Tablolara yeni eleman ekle");
                Console.WriteLine("4- Performans karsilastirmasi yap");
                Console.WriteLine("5- Uygulamadan cikis yap");
                Console.Write("Hangi islemi yapmak istersiniz?: ");
                int enter = Convert.ToInt32(Console.ReadLine());

                switch (enter)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("--- BEISCH TABLE ---");
                        beischTable.PrintTable();
                        Console.WriteLine();
                        Console.WriteLine("--- COMPUTED CHAINING TABLE ---");
                        computedChainingTable.DisplayHashTableContents();
                        Console.WriteLine();
                        Console.WriteLine("--- BINARY TREE TABLE ---");
                        binaryTreeTable.PrintTable();
                        Console.WriteLine();
                        break;
                    case 2:
                        Console.Write("Aramak istediginiz deger: ");
                        int searchingNumber = Convert.ToInt32(Console.ReadLine());
                        int beischProbeCount = beischTable.ProbeNumber(searchingNumber);
                        int ccProbeCount = computedChainingTable.FindProbeCount(searchingNumber);
                        int btProbeCount = binaryTreeTable.ProbeCount(searchingNumber);

                        if (beischProbeCount == -1)
                        {
                            Console.Clear();
                            Console.WriteLine("Aradiginiz {0} degeri tablolarda yer almamaktadir.\n", searchingNumber);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Aranan deger: " + searchingNumber);
                            Console.WriteLine("BEISCH Table icin {0} probeda bulundu.", beischProbeCount);
                            Console.WriteLine("ComputedChaining Table icin {0} probeda bulundu.", ccProbeCount);
                            Console.WriteLine("BinaryTree Table icin {0} probeda bulundu.\n", btProbeCount);
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.Write("Eklemek istediginiz eleman: ");
                        int newElement = Convert.ToInt32(Console.ReadLine());
                        beischTable.BEISCHInsert(newElement);
                        computedChainingTable.ComputedChainingInsert(newElement);
                        binaryTreeTable.BinaryTreeInsert(newElement);
                        Console.WriteLine("Yeni eleman basariyla eklendi\n");
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("BEISCH Table icin ortalama probe sayisi: " + beischTable.AverageProbeCount());
                        Console.WriteLine("ComputedChaining Table icin ortalama probe sayisi: " + computedChainingTable.AverageProbeCount());
                        Console.WriteLine("BinaryTree Table icin ortalama probe sayisi: " + binaryTreeTable.AverageProbeCount());
                        Console.WriteLine();
                        break;
                    case 5: 
                        Environment.Exit(1);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Hatali giris. Tekrardan deneyiniz.");
                        break;
                }
            }
        }
    }
}