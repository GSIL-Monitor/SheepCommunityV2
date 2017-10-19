namespace Sheep
{
    /// <summary>
    ///     数据库配置的名称。
    /// </summary>
    public static class AppSettingsDbNames
    {
        // 数据库提供程序。
        public const string DbProvider = "db.Provider";

        // 数据库连接字符串。
        public const string DbConnectionString = "db.ConnectionString";

        // RethinkDB 主机名。
        public const string RethinkDbHostName = "rethinkdb.HostName";

        // RethinkDB 端口。
        public const string RethinkDbPort = "rethinkdb.Port";

        // RethinkDB 超时。
        public const string RethinkDbTimeout = "rethinkdb.Timeout";

        // RethinkDB 连接数据库。
        public const string RethinkDbDatabase = "rethinkdb.Database";

        // RethinkDB 数据库分片数量。
        public const string RethinkDbShards = "rethinkdb.Shards";

        // RethinkDB 数据库表复制份数。
        public const string RethinkDbReplicas = "rethinkdb.Replicas";

        // Redis连接字符串。
        public const string RedisConnectionString = "redis.ConnectionString";
    }
}