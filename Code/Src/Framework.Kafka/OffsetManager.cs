﻿using Confluent.Kafka;
using System;
using System.IO;
using System.Linq;

namespace Framework.Kafka
{
	public class OffsetManager : IOffsetManager
	{
		public TopicPartitionOffset CurrentOffset(string path)
		{
			var content = File.ReadLines(path).Last();

			return string.IsNullOrEmpty(content) ? null : ParseContent(content);
		}

		private static TopicPartitionOffset ParseContent(string content)
		{
			var parts = content.Split(',').ToList();

			return new TopicPartitionOffset(parts.First(), new Partition(int.Parse(parts[1])), new Offset(long.Parse(parts[2])));
		}

		public void Persist(string path, TopicPartitionOffset topicPartitionOffset)
		{
			var body = $"{topicPartitionOffset.Topic},{topicPartitionOffset.Partition.Value},{topicPartitionOffset.Offset.Value}";

			if (!Directory.Exists(Path.GetDirectoryName(path)))
				Directory.CreateDirectory(Path.GetDirectoryName(path));

			File.AppendAllText(path, $"{body},{DateTime.UtcNow}{Environment.NewLine}");
		}
	}
}
