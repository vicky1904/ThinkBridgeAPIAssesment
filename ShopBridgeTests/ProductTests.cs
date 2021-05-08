using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ShopBridgeAPI.Controllers;
using ValueObjects;
using Services;
using Services.AutoMapper;
using ShopBridgeDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeTests
{
    public class Tests : BaseSpecs
    {
        private ProductController itemController;

        [SetUp]
        public void Setup()
        {
            SetupDbAndLogger();
            var unitOfWork = new UnitofWork(ShopBridgeDbContext);
            var itemRepository = new GenericRepository<Products>(ShopBridgeDbContext);
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            var itemService = new ProductsService(unitOfWork, itemRepository, mapper);
            itemController = new ProductController(itemService);
        }

        [TearDown]
        public void TearDown()
        {
            ShopBridgeDbContext.Dispose();
        }

        [Test]
        public async Task Can_create_item()
        {
            var item = Item();
            var createdItem = await CreateItem(item);
            Assert.NotNull(createdItem);
            Assert.AreEqual(200, ((ObjectResult)createdItem).StatusCode);
            Assert.AreNotEqual(0, ((ObjectResult)createdItem).Value);
        }

        [Test]
        public async Task Cannot_create_item_if_item_is_null()
        {
            var createdItem = await CreateItem(null);
            Assert.NotNull(createdItem);
            Assert.AreEqual("Please specify Name, Description and Price", ((ObjectResult)createdItem).Value);
            Assert.AreEqual(400, ((ObjectResult)createdItem).StatusCode);
        }

        [Test]
        public async Task Cannot_create_item_if_validation_fail()
        {
            var createdItem = await CreateItem(Item(x => x.Name = null));
            Assert.NotNull(createdItem);
            Assert.AreEqual("Please specify Name, Description and Price", ((ObjectResult)createdItem).Value);
            Assert.AreEqual(400, ((ObjectResult)createdItem).StatusCode);
        }

        [Test]
        public async Task Can_get_item()
        {
            var createdItem = await CreateItem(Item());
            var id = ((ObjectResult)createdItem).Value;
            var fetchItem = await GetItem((int)id);
            var result = ((ObjectResult)fetchItem).Value;
            var returned = (VOProduct)result;
            Assert.NotNull(fetchItem);
            Assert.AreEqual(200, ((ObjectResult)fetchItem).StatusCode);
            Assert.AreEqual(id, returned.Id);
        }

        [Test]
        public async Task Shall_get_error_while_fetching_item_if_id_not_present()
        {
            var fetchItem = await GetItem(5);
            Assert.NotNull(fetchItem);
            Assert.Null(((ObjectResult)fetchItem).Value);
            Assert.AreEqual(200, ((ObjectResult)fetchItem).StatusCode);
        }

        [Test]
        public async Task Can_get_all_items()
        {
            var createdItem1 = await CreateItem(Item());
            var createdItem2 = await CreateItem(Item());
            var fetchItems = await GetAllItems();
            Assert.NotNull(fetchItems);
            Assert.NotNull(((ObjectResult)fetchItems).Value);
            Assert.AreEqual(200, ((ObjectResult)fetchItems).StatusCode);
            var expected1 = ((ObjectResult)createdItem1).Value;
            var expectedData1 = (int)expected1;
            var expected2 = ((ObjectResult)createdItem2).Value;
            var expectedData2 = (int)expected2;
            var fetchItemsData = ((ObjectResult)fetchItems).Value;
            var fetchItemsOutputData = (List<VOProduct>)fetchItemsData;
            Assert.AreEqual(expectedData1, fetchItemsOutputData.First(x => x.Id == expectedData1).Id);
            Assert.AreEqual(expectedData2, fetchItemsOutputData.First(x => x.Id == expectedData2).Id);
        }

        [Test]
        public async Task Can_update_item()
        {
            var createdItem = await CreateItem(Item());
            var id = ((ObjectResult)createdItem).Value;
            var updateItem = await UpdateItem((int)id, Item());
            Assert.NotNull(updateItem);
            Assert.NotNull(((ObjectResult)updateItem).Value);
            Assert.AreEqual(200, ((ObjectResult)updateItem).StatusCode);
        }

        [Test]
        public async Task Cannot_update_item_if_id_is_empty()
        {
            var updateItem = await UpdateItem(0, Item());
            Assert.NotNull(updateItem);
            Assert.NotNull(((ObjectResult)updateItem).Value);
            Assert.AreEqual(400, ((ObjectResult)updateItem).StatusCode);
        }

        [Test]
        public async Task Cannot_update_item_if_item_is_null()
        {
            var updateItem = await UpdateItem(1, null);
            Assert.NotNull(updateItem);
            Assert.AreEqual("Please specify correct details", ((ObjectResult)updateItem).Value);
            Assert.AreEqual(400, ((ObjectResult)updateItem).StatusCode);
        }

        [Test]
        public async Task Cannot_update_item_if_id_and_item_id_mismatch()
        {
            var updateItem = await UpdateItem(100, Item());
            Assert.NotNull(updateItem);
            Assert.IsFalse(Convert.ToBoolean(((ObjectResult)updateItem).Value));
            Assert.AreEqual(200, ((ObjectResult)updateItem).StatusCode);
        }

        [Test]
        public async Task Cannot_update_item_if_validation_fail()
        {
            var id = 101;
            var updateItem = await UpdateItem(id, Item(x =>
            {
                x.Id = id;
                x.Name = string.Empty;
            }));
            Assert.NotNull(updateItem);
            Assert.NotNull(((ObjectResult)updateItem).Value);
            Assert.AreEqual(400, ((ObjectResult)updateItem).StatusCode);
        }

        [Test]
        public async Task Cannot_update_item_if_item_not_found()
        {
            var id = 124;
            var updateItem = await UpdateItem(id, Item(x => x.Id = id));
            Assert.NotNull(updateItem);
            Assert.IsFalse(Convert.ToBoolean(((ObjectResult)updateItem).Value));
            Assert.AreEqual(200, ((ObjectResult)updateItem).StatusCode);
        }

        [Test]
        public async Task Can_delete_item()
        {
            var createdItem = await CreateItem(Item());
            var data = ((ObjectResult)createdItem).Value;
            var deleteResponse = await DeleteItem((int)data);
            var delResponse = ((ObjectResult)deleteResponse).Value;
            Assert.IsTrue((bool)delResponse);
            Assert.AreEqual(200, ((ObjectResult)deleteResponse).StatusCode);
            var fetchItem = await GetItem((int)data);
            Assert.AreEqual(200, ((ObjectResult)fetchItem).StatusCode);
        }

        [Test]
        public async Task Cannot_delete_item_if_item_not_found()
        {
            var deleteResponse = await DeleteItem(5);
            Assert.NotNull(deleteResponse);
            Assert.AreEqual(200, ((ObjectResult)deleteResponse).StatusCode);
            Assert.IsTrue(Convert.ToBoolean(((ObjectResult)deleteResponse).Value));
        }

        [Test]
        public async Task Cannot_delete_item_if_item_id_is_empty()
        {
            var deleteResponse = await DeleteItem(0);
            Assert.NotNull(deleteResponse);
            Assert.AreEqual(400, ((ObjectResult)deleteResponse).StatusCode);
            Assert.AreEqual("Please specify correct Product Id", ((ObjectResult)deleteResponse).Value);
        }

        #region Private methods

        private async Task<IActionResult> CreateItem(VOProduct item)
        {
            return await itemController.Post(item);
        }

        private async Task<IActionResult> GetItem(int id)
        {
            return await itemController.Get(id);
        }

        private async Task<IActionResult> GetAllItems()
        {
            return await itemController.Get();
        }

        private async Task<IActionResult> UpdateItem(int id, VOProduct item)
        {
            return await itemController.Put(id, item);
        }

        private async Task<IActionResult> DeleteItem(int id)
        {
            return await itemController.Delete(id);
        }

        private VOProduct Item(Action<VOProduct> action = null)
        {
            var item = new VOProduct
            {
                Name = Helper.RandomLettersString(100),
                Description = Helper.RandomString(500),
                Price = Helper.RandomDecimal(100, 10)
            };
            action?.Invoke(item);
            return item;
        }

        private static void AssertItem(VOProduct expectedItem, VOProduct actualItem)
        {
            Assert.AreEqual(expectedItem.Name, actualItem.Name);
            Assert.AreEqual(expectedItem.Description, actualItem.Description);
            Assert.AreEqual(expectedItem.Price, actualItem.Price);
        }

        #endregion
    }
}