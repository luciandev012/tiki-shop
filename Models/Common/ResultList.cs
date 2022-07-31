namespace tiki_shop.Models.Common
{
    public class ResultList<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<T>? Data { get; set; }
    }
}
