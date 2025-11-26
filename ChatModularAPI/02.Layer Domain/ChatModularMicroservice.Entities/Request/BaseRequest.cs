namespace ChatModularMicroservice.Entities
{

    public class Pagination
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public int TotalRows { get; set; } = 0;
    }
    public abstract class BaseRequest
    {
        public string Ticket { get; set; } = Guid.NewGuid().ToString();
        public string ClientName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string ServerName { get; set; } = string.Empty;
        public int Resultado { get; set; } = int.MaxValue;
    }


}

