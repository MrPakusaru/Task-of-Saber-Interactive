using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ListOfNodes
{
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;
        public void Serialize(FileStream s)
        {
            //создание списка с узлами и их нумерацией
            Dictionary<ListNode, int> indexForNode = new Dictionary<ListNode, int>();
            ListNode ln = Head;
            int index = 0;
            while (ln != null && !indexForNode.ContainsKey(ln))
            {
                indexForNode.Add(ln, index++);
                ln = ln.Next;
            }
            //создание форматированной записи данных о двусвязном списке
            StringBuilder strbild = new StringBuilder();
            strbild.Append("[");
            ln = Head;
            while (ln != null)
            {
                strbild.Append("{");
                strbild.Append($"Data='{ln.Data}',");
                strbild.Append($"Prev={(ln.Prev == null ? "null" : indexForNode[ln.Prev].ToString())},");
                strbild.Append($"Next={(ln.Next == null ? "null" : indexForNode[ln.Next].ToString())},");
                strbild.Append($"Rand={(ln.Rand == null ? "null" : indexForNode[ln.Rand].ToString())}");
                strbild.Append("}");
                ln = ln.Next;
                if (ln == null) break;
                strbild.Append(",");
            }
            strbild.Append("]");
            //записать форматированных данных в файл в кодировке UTF8
            byte[] nodeInBytes = new UTF8Encoding(true).GetBytes(strbild.ToString());
            s.Write(nodeInBytes, 0, nodeInBytes.Length);
        }
        public void Deserialize(FileStream s, ListRand listRand)
        {
            if (DeListNode.IfEmptyStream(s)) throw new EmptyStreamException();//проверка на наличие символов и целостность данных
            else
            {
                List<DeListNode> deList = new List<DeListNode>();
                //заполнение временного списка узлов данными из файла
                bool ifReadNode = false;
                while (true)
                {
                    int rByte = s.ReadByte();
                    if (rByte == '{' && !ifReadNode)
                    {
                        ifReadNode = true;
                        Dictionary<string, string> nodeFields = new Dictionary<string, string>();
                        for (int i = 0; i < 3; ++i) DeListNode.GetFields(s, ',', nodeFields);
                        DeListNode.GetFields(s, '}', nodeFields);
                        deList.Add(DeListNode.getDeNode(nodeFields));
                    }
                    else if (rByte == ',' && ifReadNode) ifReadNode = false;
                    else if (rByte == -1) throw new UnexpectedEndException();
                    else if (rByte == ']') break;
                    else throw new UnexpectedCharException((char)rByte);
                }
                if (deList.Count <= 0) return;    
                //конвертация данных об узлах списка в список готовых узлов
                DeListNode.SetNodes(deList);
                //заполнение пустого двусвязного списка содержанием временных узлов
                listRand.Head = deList.First().DeNode;
                listRand.Tail = deList.Last().DeNode;
                listRand.Count = deList.Count;




            }
        }
    }
}
