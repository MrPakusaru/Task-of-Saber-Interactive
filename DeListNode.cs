using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace ListOfNodes
{
    public class DeListNode
    {
        //временный узел с информацией о нём
        public ListNode DeNode;
        public string Prev;
        public string Next;
        public string Rand;
        //конструктор создания временного узла
        DeListNode(ListNode deNode, string data, string prev, string next, string rand)
        {
            DeNode = deNode;
            DeNode.Data = data;
            Prev = prev;
            Next = next;
            Rand = rand;
        }
        //условия для проверки на наличие данных и исправность файла с данными
        public static bool IfEmptyStream(FileStream s) { int bt = s.ReadByte(); return bt != '[' || bt == -1; }
        //получение текста до нужного символа
        private static string GetCharsToEnd(FileStream fs, char endChar)
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int rByte = fs.ReadByte();
                if (rByte == -1) throw new UnexpectedEndException();
                if ((char)rByte == endChar) break;
                if (rByte == '\'') sb.Append(GetCharsToEnd(fs, '\''));
                else sb.Append((char)rByte);
            }

            return sb.ToString();
        }
        //получение сериализованных данных 
        public static void GetFields(FileStream s, char c, Dictionary<string, string> NF)
        {
            string str = GetCharsToEnd(s, c);
            string[] field = str.Split(new[] { '=' }, 2);
            NF.Add(field[0], field[1]);
        }
        //возвращение собранного временного узла с данными о нём
        public static DeListNode getDeNode(Dictionary<string, string> nodeFields)
        {
            return new DeListNode(new ListNode(), nodeFields["Data"], nodeFields["Prev"], nodeFields["Next"], nodeFields["Rand"]);
        }
        //конвертация данных об узлах списка в список гготовых узлов
        public static void SetNodes(List<DeListNode> deList)
        {
            foreach (DeListNode dln in deList)
            {
                if (dln.Next != "null") dln.DeNode.Next = deList[int.Parse(dln.Next)].DeNode;
                if (dln.Prev != "null") dln.DeNode.Prev = deList[int.Parse(dln.Prev)].DeNode;
                if (dln.Rand != "null") dln.DeNode.Rand = deList[int.Parse(dln.Rand)].DeNode;
                if (deList.Count > 1 && dln.DeNode.Next == null && dln.DeNode.Prev == null)
                {
                    throw new UnconnectedNodeException();
                }
            }
        }
    }
    public class UnexpectedEndException : SerializationException
    {
        public UnexpectedEndException() : base("Непредвиденный конец потока")
        {
        }
    }
    public class UnconnectedNodeException : SerializationException
    {
        public UnconnectedNodeException() : base("Отсутствует узел списка")
        {
        }
    }
    public class UnexpectedCharException : SerializationException
    {
        public UnexpectedCharException(char c) : base($"Непредвиденный символ: \'{c}\'''")
        {
        }
    }
    public class EmptyStreamException : SerializationException
    {
        public EmptyStreamException() : base("Поток не имеет данных")
        {
        }
    }

}