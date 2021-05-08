using System.ComponentModel.DataAnnotations;

namespace ValueObjects
{
	public class VOProduct
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }


		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		public int StockCount { get; set; }

		public string Supplier { get; set; } 

	}
}
