# learning-kafka

## Useful things

Set number of partitions of topic `weather-updates` by using the following command (executed in the `broker` container):

    kafka-topics --alter --topic weather-updates --partitions 64 --zookeeper zookeeper:2181

A partition is like a hash bucket - messages in a topic are divided into partitions based on their keys. Therefore, keys don't need to be unique, they don't identify the message but the subject of the message. If processing messages related to the same entity in order is important, they must be in the same bucket/partition, and this can be ensured by them having the same key (usually the key of the subject entity itself).