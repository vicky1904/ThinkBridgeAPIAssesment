﻿namespace ShopBridgeDataModel
{
	public class Products
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		public bool Availiblity { get; set; }
	}
}