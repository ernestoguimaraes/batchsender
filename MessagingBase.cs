namespace AzureBatchSender
{
    public abstract class MessagingBase
    {
        public string ConnectionString { get; set; }
        public int NumThreads { get; set; }
        public int NumMessages { get; set; }
    }
}
