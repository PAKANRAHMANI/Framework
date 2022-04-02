using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Framework.Kafka
{
    public class OffsetManager : IOffsetManager
    {
        public async Task<TopicPartitionOffset> CurrentOffset(string path)
        {
            var content = await File.ReadAllTextAsync(path);

            return string.IsNullOrEmpty(content) ? null : ParseContent(content);
        }
        private static TopicPartitionOffset ParseContent(string content)
        {
            var parts = content.Split(',').ToList();

            return new TopicPartitionOffset(parts.First(), new Partition(int.Parse(parts[1])), new Offset(long.Parse(parts[2])));
        }
        public async Task Persist(string path, TopicPartitionOffset topicPartitionOffset)
        {
            var body =
                $"{topicPartitionOffset.Topic},{topicPartitionOffset.Partition.Value},{topicPartitionOffset.Offset.Value}";

            await File.AppendAllTextAsync(path, $"{body},{DateTime.Now}{Environment.NewLine}");
        }
    }
}
