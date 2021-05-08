using AutoMapper;
using ValueObjects;
using ShopBridgeDataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
	public class ProductsService : IProductsService
	{
		private readonly IMapper _mapper;
		private readonly IUnitofWork _unitOfWork;
		private readonly IGenericRepository<Products> _genericRepository;

		public ProductsService(IUnitofWork unitofWork, IGenericRepository<Products> genericRepository, IMapper mapper)
		{
			_unitOfWork = unitofWork;
			_genericRepository = genericRepository;
			_mapper = mapper;
		}

		public async Task<int> CreateProductAsync(VOProduct product)
		{
			var itemData = _mapper.Map<Products>(product);

			using (var transaction = _unitOfWork.BeginTransaction())
			{
				await _genericRepository.InsertAsync(itemData).ConfigureAwait(false);
				await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
				transaction.Commit();
			}
			return itemData.Id;
		}

		public async Task<bool> DeleteProductAsync(int productId)
		{
			var fetchItem = await _genericRepository.GetByIDAsync(productId);

			if (fetchItem == null)
				return false;

			_genericRepository.Delete(fetchItem);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			return true;
		}

		public async Task<IEnumerable<VOProduct>> GetAllProductsAsync()
		{
			var product = await _genericRepository.GetAllAsync();
			if (product != null)
			{
				var dest = _mapper.Map<IEnumerable<VOProduct>>(product);
				return dest;
			}
			return null;
		}

		public async Task<VOProduct> GetProductByIdAsync(int productId)
		{
			var product = await _genericRepository.GetByIDAsync(productId);
			if (product != null)
			{
				var dest = _mapper.Map<VOProduct>(product);
				return dest;
			}
			return null;
		}

		public async Task<bool> UpdateProductAsync(int productId, VOProduct product)
		{
			using var transaction = _unitOfWork.BeginTransaction();
			var fetchItem = await _genericRepository.GetByIDAsync(productId);

			if (fetchItem == null)
				return false;

			fetchItem.Name = product.Name;
			fetchItem.Description = product.Description;
			fetchItem.Price = product.Price;
			fetchItem.Availiblity = product.StockCount > 0;

			_genericRepository.Update(fetchItem);
			await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
			transaction.Commit();
			return true;
		}
	}
}
