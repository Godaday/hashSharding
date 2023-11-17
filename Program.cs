// See https://aka.ms/new-console-template for more information
// 模拟数据库表数据
using hashSharding;

List<int> data = new List<int> { 10, 25, 35, 50, 60, 75, 90 };

        // 定义分片节点数量
        int initialShardNodeCount = 3;

        // 创建一致性哈希环
        ConsistentHashRing hashRing = new ConsistentHashRing(initialShardNodeCount);

        // 初始化数据分片
        Dictionary<int, List<int>> shards = new Dictionary<int, List<int>>();
        for (int i = 0; i < initialShardNodeCount; i++)
        {
            shards[i] = new List<int>();
        }

        // 初始化每个分片节点的数据列表
        for (int i = 0; i < initialShardNodeCount; i++)
        {
            hashRing.AddNode(i);
        }

        // 使用一致性哈希进行数据分片
        foreach (int value in data)
        {
            int shardNode = hashRing.GetNode(value);
            shards[shardNode].Add(value);
        }

        // 打印初始分片结果
        Console.WriteLine("Initial Shard Assignment:");
        PrintShards(shards);

        // 模拟增加节点
        int addedNode = 3;
        hashRing.AddNode(addedNode);


        // 打印增加节点后的分片结果
        Console.WriteLine($"\nAfter Adding Node {addedNode}:");
        PrintShards(shards);

        // 模拟删除节点
        int removedNode = 1;
        hashRing.RemoveNode(removedNode);



        // 打印删除节点后的分片结果
        Console.WriteLine($"\nAfter Removing Node {removedNode}:");
        PrintShards(shards);


// 打印分片结果
static void PrintShards(Dictionary<int, List<int>> shards)
{
    foreach (var shard in shards)
    {
        Console.WriteLine($"Shard {shard.Key + 1}: " + string.Join(", ", shard.Value));
    }
}


