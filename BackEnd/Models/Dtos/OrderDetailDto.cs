namespace itsc_dotnet_practice.Models.Dtos;

public class OrderDetailDto
{
    public class OrderDetailRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
