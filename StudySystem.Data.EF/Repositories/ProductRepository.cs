using EFCore.BulkExtensions;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {

            _context = context;

        }

        public async Task<bool> AddWishList(string userId, string productId)
        {
            var qeury = await _context.WishLists.SingleOrDefaultAsync(x => x.UserId.Equals(userId) && x.ProductId.Equals(productId)).ConfigureAwait(false);
            if (qeury != null)
            {
                _context.WishLists.Remove(qeury);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return false;
            }
            else
            {
                WishList addNew = new WishList();
                addNew.UserId = userId;
                addNew.ProductId = productId;
                await _context.AddAsync(addNew).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }

        }

        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task CreateProduct(List<Product> products)
        {
            await _context.BulkInsertOrUpdateAsync(products);
        }
        /// <summary>
        /// DeleteProduct
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteProduct(string productId)
        {
            var rs = await _context.Set<Product>().SingleOrDefaultAsync(x => x.ProductId.Equals(productId)).ConfigureAwait(false);
            if (rs != null)
            {
                _context.Remove(rs);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
        /// <summary>
        /// GetAllProduct
        /// </summary>
        /// <returns></returns>
        public async Task<ListProductDetailResponseModel> GetAllProduct(string userId, string hostUrl)
        {
            ListProductDetailResponseModel model = new ListProductDetailResponseModel();
            var resul1t = (from p in _context.Products
                           join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                           join c in _context.Categories on pc.CategoryId equals c.CategoryId
                           join i in _context.Images on p.ProductId equals i.ProductId
                           join pc2 in _context.ProductConfigurations on p.ProductId equals pc2.ProductId
                           group new { p, c, i, pc2 } by new { p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice, p.BrandName, p.ProductQuantity, p.ProductionDate, p.ProductStatus, p.PriceSell, c.CategoryName, c.CategoryId, pc2.Ram, pc2.Rom, pc2.Chip, pc2.Screen } into pGroup
                           select new ProductDetailResponseModel
                           {
                               ProductId = pGroup.Key.ProductId,
                               ProductName = pGroup.Key.ProductName,
                               ProductDescription = pGroup.Key.ProductDescription,
                               ProductPrice = pGroup.Key.ProductPrice,
                               ProductBrand = pGroup.Key.BrandName,
                               ProductQuantity = pGroup.Key.ProductQuantity,
                               ProductSell = pGroup.Key.PriceSell,
                               ProductionDate = pGroup.Key.ProductionDate,
                               ProductStatus = pGroup.Key.ProductStatus,
                               CategoryId = pGroup.Key.CategoryId,
                               ProductCategory = pGroup.Key.CategoryName,
                               Images = pGroup.Select(x => new ImageProductData { ImagePath = hostUrl + pGroup.Key.ProductId + "/" + x.i.ImageDes }).ToList(),
                               ProductConfig = new ProductConfigData { Chip = pGroup.Key.Chip, Ram = pGroup.Key.Ram, Rom = pGroup.Key.Rom, Screen = pGroup.Key.Screen },
                               IsLike = _context.WishLists.Any(x => x.UserId.Equals(userId) && x.ProductId.Equals(pGroup.Key.ProductId))
                           }).ToListAsync();


            model.listProductDeatails = await resul1t;
            return model;
        }

        public IQueryable<ProductDetailResponseModel> GetProductDetail(string productId, string userId)
        {
            var resul1t = (from p in _context.Products
                           join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                           join c in _context.Categories on pc.CategoryId equals c.CategoryId
                           join i in _context.Images on p.ProductId equals i.ProductId
                           join pc2 in _context.ProductConfigurations on p.ProductId equals pc2.ProductId
                           where p.ProductId == productId
                           group new { p, c, i, pc2 } by new { p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice, p.BrandName, p.ProductQuantity, p.ProductionDate, p.ProductStatus, p.PriceSell, c.CategoryName, c.CategoryId, pc2.Ram, pc2.Rom, pc2.Chip, pc2.Screen } into pGroup
                           select new ProductDetailResponseModel
                           {
                               ProductId = pGroup.Key.ProductId,
                               ProductName = pGroup.Key.ProductName,
                               ProductDescription = pGroup.Key.ProductDescription,
                               ProductPrice = pGroup.Key.ProductPrice,
                               ProductBrand = pGroup.Key.BrandName,
                               ProductQuantity = pGroup.Key.ProductQuantity,
                               ProductSell = pGroup.Key.PriceSell,
                               ProductionDate = pGroup.Key.ProductionDate,
                               ProductStatus = pGroup.Key.ProductStatus,
                               CategoryId = pGroup.Key.CategoryId,
                               ProductCategory = pGroup.Key.CategoryName,
                               Images = pGroup.Select(x => new ImageProductData { ImagePath = x.i.ImageDes }).ToList(),
                               ProductConfig = new ProductConfigData { Chip = pGroup.Key.Chip, Ram = pGroup.Key.Ram, Rom = pGroup.Key.Rom, Screen = pGroup.Key.Screen },
                               IsLike = _context.WishLists.Any(x => x.UserId.Equals(userId) && x.ProductId.Equals(pGroup.Key.ProductId))
                           });
            return resul1t;
        }
        /// <summary>
        /// GetWishList
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ListProductDetailResponseModel> GetWishList(string userId)
        {
            var result = (from wl in _context.WishLists
                          join p in _context.Products on wl.ProductId equals p.ProductId
                          join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                          join i in _context.Images on p.ProductId equals i.ProductId
                          where wl.UserId.Equals(userId)
                          group new { p, i } by new { p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice, p.BrandName, p.ProductQuantity, p.ProductionDate, p.ProductStatus, p.PriceSell } into pGroup
                          select new ProductDetailResponseModel
                          {
                              ProductId = pGroup.Key.ProductId,
                              ProductName = pGroup.Key.ProductName,
                              ProductPrice = pGroup.Key.ProductPrice,
                              ProductSell = pGroup.Key.PriceSell,
                              ProductStatus = pGroup.Key.ProductStatus,
                              Images = new List<ImageProductData> { pGroup.Select(x => new ImageProductData { ImagePath = x.i.ImageDes }).FirstOrDefault() },
                              ProductBrand = pGroup.Key.BrandName,
                              IsLike = _context.WishLists.Any(x => x.UserId.Equals(userId) && x.ProductId.Equals(pGroup.Key.ProductId))
                          }).ToListAsync().ConfigureAwait(false);
            ListProductDetailResponseModel listProductDetailResponseModel = new ListProductDetailResponseModel();
            listProductDetailResponseModel.listProductDeatails = await result;
            return listProductDetailResponseModel;

        }

        public async Task<ListProductDetailResponseModel> ProductByCategoryId(string categoryId, string userId)
        {
            var result = (from p in _context.Products
                          join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                          join i in _context.Images on p.ProductId equals i.ProductId
                          where pc.CategoryId.Equals(categoryId)
                          group new { p, i } by new { p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice, p.BrandName, p.ProductQuantity, p.ProductionDate, p.ProductStatus, p.PriceSell } into pGroup
                          select new ProductDetailResponseModel
                          {
                              ProductId = pGroup.Key.ProductId,
                              ProductName = pGroup.Key.ProductName,
                              ProductPrice = pGroup.Key.ProductPrice,
                              ProductSell = pGroup.Key.PriceSell,
                              ProductStatus = pGroup.Key.ProductStatus,
                              Images = new List<ImageProductData> { pGroup.Select(x => new ImageProductData { ImagePath = x.i.ImageDes }).FirstOrDefault() },
                              ProductBrand = pGroup.Key.BrandName,
                              IsLike = _context.WishLists.Any(x => x.UserId.Equals(userId) && x.ProductId.Equals(pGroup.Key.ProductId))
                          }).ToListAsync().ConfigureAwait(false);
            ListProductDetailResponseModel listProductDetailResponseModel = new ListProductDetailResponseModel();
            listProductDetailResponseModel.listProductDeatails = await result;
            return listProductDetailResponseModel;
        }

        /// <summary>
        /// UpdateProduct
        /// </summary>
        /// <param name="updateProduct"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProduct(UpdateProductRequestModel updateProduct)
        {
            var rs = await _context.Set<Product>().FirstOrDefaultAsync(x => x.ProductId.Equals(updateProduct.ProductId)).ConfigureAwait(false);
            if (rs != null)
            {
                rs.ProductName = updateProduct.ProductName;
                rs.ProductPrice = updateProduct.Price;
                rs.PriceSell = updateProduct.PriceSell;
                rs.ProductDescription = updateProduct.Description;
                rs.ProductQuantity = updateProduct.ProductQuantity;
                rs.ProductionDate = updateProduct.ProductionDate;
                rs.ProductStatus = updateProduct.ProductStatus;
                rs.BrandName = updateProduct.ProductBrand;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateProductQuantity(UpdateProductQuantityDataModel data)
        {
            int count = 0;
            foreach (var item in data.ProductChangedData)
            {
                var productChange = await _context.Set<Product>().FirstOrDefaultAsync(x => x.ProductId.Equals(item.ProductId)).ConfigureAwait(false);
                if (productChange != null)
                {
                    productChange.ProductQuantity = productChange.ProductQuantity - item.Quantity;
                    count++;
                }
            }
            if (count == data.ProductChangedData.Count())
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;

        }

        public async Task<ListProductDetailResponseModel> ViewedProduct(ViewedProductRequestModel request, string userId)
        {
            var result = (from p in _context.Products
                          join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                          join i in _context.Images on p.ProductId equals i.ProductId
                          where request.ProductIdData.Select(k => k.ProductId).Contains(p.ProductId)
                          group new { p, i } by new { p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice, p.BrandName, p.ProductQuantity, p.ProductionDate, p.ProductStatus, p.PriceSell } into pGroup
                          select new ProductDetailResponseModel
                          {
                              ProductId = pGroup.Key.ProductId,
                              ProductName = pGroup.Key.ProductName,
                              ProductPrice = pGroup.Key.ProductPrice,
                              ProductSell = pGroup.Key.PriceSell,
                              ProductStatus = pGroup.Key.ProductStatus,
                              Images = pGroup.Select(x => new ImageProductData { ImagePath = x.i.ImageDes }).ToList(),
                              IsLike = _context.WishLists.Any(x => x.UserId.Equals(userId) && x.ProductId.Equals(pGroup.Key.ProductId))
                          }).ToListAsync().ConfigureAwait(false);
            ListProductDetailResponseModel listProductDetailResponseModel = new ListProductDetailResponseModel();
            listProductDetailResponseModel.listProductDeatails = await result;
            return listProductDetailResponseModel;
        }
    }
}
