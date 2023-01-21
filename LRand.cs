using System;

namespace ListOfNodes
{
    //вспомогательный дочерний класс для узлов списка
    public class LNode : ListNode
    {
        public LNode(string data) { Data = data; }
    }
    //вспомогательный дочерний класс для списка
    public class LRand : ListRand
    {
        //пустой конструктор для создания пустого списка
        public LRand() { }
        //создание списка с одним заданным узлом
        public LRand(string data)
        {
            LNode ln = new LNode(data);
            Head = ln;
            Tail = ln;
            Count = 1;
        }
        //функция добавления узла
        public void Add(string data)
        {
            LNode ln = new LNode(data);
            if (Head == null)
            {
                ln.Rand = ln;
                Head = ln;
            }
            else
            {
                Random rnd = new Random();
                ln.Prev = Tail;
                ln.Next = null;
                ln.Rand = GetNode(rnd.Next(0, Count));
                Tail.Next = ln;
            }
            Tail = ln;
            Count++;
        }
        //функция получения данных узла по его номеру в списке
        public ListNode GetNode(int num)
        {
            if (num >= 0 && num < Count + 1)
            {
                ListNode current = Head;
                for (int i = 0; i < num; i++)
                {
                    current = current.Next;
                }
                return current;
            }
            else return null;
        }
    }
}