using System;
using System.IO;

namespace ListOfNodes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            LRand listrand = new LRand();
            // добавление элементов
            listrand.Add("ccc");
            listrand.Add("qwe");
            listrand.Add("abba");
            listrand.Add("qbd");
            listrand.Add("mir");
            listrand.Add("tori");
            //вывод добавленных в двусвязный список элементов
            for (int i = 0; i < listrand.Count; i++)
            {
                Console.WriteLine(listrand.GetNode(i).Data);
            }

            Console.WriteLine(" = = = ");
            FileStream fs = new FileStream("D:/str.JSON", FileMode.OpenOrCreate);

            //listrand.Serialize(fs);


            ListRand newlistrand = new LRand();
            newlistrand.Deserialize(fs, newlistrand);

            LRand lr = (LRand)newlistrand;
            for (int i = 0; i < lr.Count; i++)
            {
                Console.WriteLine(lr.GetNode(i).Data);
            }
        }
    }
}
