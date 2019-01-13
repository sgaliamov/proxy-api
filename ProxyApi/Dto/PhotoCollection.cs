namespace ProxyApi.Dto
{
    public sealed class PhotoCollection
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int PerPage { get; set; }
        public Photo[] Photo { get; set; }
        public long Total { get; set; }
    }
}
