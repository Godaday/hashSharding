using System.Security.Cryptography;

namespace hashSharding;
// 一致性哈希环类
public class ConsistentHashRing
{
  private SortedDictionary<int, int> ring = new SortedDictionary<int, int>();

    public List<int> Nodes => new List<int>(ring.Values);

    public ConsistentHashRing(int initialNodeCount)
    {
        for (int i = 0; i < initialNodeCount; i++)
        {
            AddNode(i);
        }
    }

    // 添加节点
    public void AddNode(int newNode)
    {
        int hash = GetHash(newNode.ToString());

        // 找到紧邻的两个节点
        var successors = ring.Where(pair => pair.Key > hash).Take(2).ToList();

        // 插入新节点到环上
        ring[hash] = newNode;

        if (successors.Count == 2)
        {
            // 将数据从后一个节点移动到新节点
            MoveData(successors[0].Value, newNode);
        }
    }

    // 移除节点
    public void RemoveNode(int nodeToRemove)
    {
        int hashToRemove = GetHash(nodeToRemove.ToString());

        // 找到要删除节点的后继节点
        var successor = ring.FirstOrDefault(pair => pair.Key > hashToRemove);

        if (successor.Key == 0 && successor.Value == 0)
        {
            // 要删除的节点是最后一个节点，找到环的第一个节点作为后继
            successor = ring.First();
        }

        // 将后继节点的数据移到要删除的节点上
        MoveData(successor.Value, nodeToRemove);

        // 从环上移除节点
        ring.Remove(hashToRemove);
    }

    // 获取数据在哈希环上的节点
    public int GetNode(int data)
    {
        if (ring.Count == 0)
        {
            throw new InvalidOperationException("哈希环为空。");
        }

        int hash = GetHash(data.ToString());
        var node = ring.FirstOrDefault(n => n.Key >= hash).Value;

        return node.Equals(default(int)) ? ring.First().Value : node;
    }

    // 简单的哈希函数
    private int GetHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return BitConverter.ToInt32(hashBytes, 0);
        }
    }

    // 移动数据
    private void MoveData(int sourceNode, int destinationNode)
    {
        Console.WriteLine($"Moving data from Node {sourceNode} to Node {destinationNode}");

        // 模拟数据移动：将源节点的数据移动到目标节点
        foreach (var pair in ring.Where(entry => entry.Value == sourceNode).ToList())
        {
            ring[pair.Key] = destinationNode;
        }
    }
}