using DatabaseService.Entities;
using DatabaseService.Repositories;
using DatabaseService.Repositories.DBSet;
using ShoesShopAPI.Models;
using ShoesShopAPI.Response;
using ShoesShopAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShoesShopAPI.Controllers
{
    [RoutePrefix("api/[controller]")]
    public class ProductController : ApiController
    {
        ProductRepository productRepository = new ProductRepository();
        CategoryRepository categoryRepository = new CategoryRepository();
        ProductSkuRepository productSkuRepository = new ProductSkuRepository();
        ProductImageRepository imageRepository = new ProductImageRepository();

        [HttpGet]
        [ActionName("getProducts")]
        public JSONFormat<List<ProductViewModel>> GetAllProducts()
        {
            JSONFormat<List<ProductViewModel>> result = null;
            //create productViewModel List to return for client
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                //get all cateogories
                var categories = categoryRepository.All().Where(c => c.Active == true).ToList();
                for (int i = 0; i < categories.Count; i++)
                {
                    int cateID = categories[i].ID;
                    //get all products of category
                    var products = productRepository.GetAllProducts()
                        .Where(q => q.CategoryID == cateID)
                        .Where(q => q.Active == true).ToList();

                    if (products != null)
                    {
                        for (int j = 0; j < products.Count; j++)
                        {
                            //get product skus by productID
                            int proID = products[j].ID;
                            var productSkus = productSkuRepository.All()
                                .Where(p => p.ProductID == proID)
                                .Where(p => p.Active == true).ToList();

                            var proImageList = imageRepository.All().
                                        Where(p => p.ProductID == proID).ToList();
                            List<string> imageList = new List<string>();

                            foreach (ProductImage item in proImageList)
                            {
                                imageList.Add(item.PicURL.ToString());
                            }

                            if (productSkus != null)
                            {
                                //add products sku to product sku model
                                List<ProductSkuViewModel> proSkus = new List<ProductSkuViewModel>();
                                foreach (ProductSku sku in productSkus)
                                {
                                    
                                    //add productSkuViewModel to a product
                                    proSkus.Add(new ProductSkuViewModel()
                                    {
                                        ID = sku.ID,
                                        ProductID = sku.ProductID,
                                        Size = sku.Size,
                                        UnitPrice = sku.UnitPrice
                                    });
                                }//end for productSku

                                productList.Add(new ProductViewModel()
                                {
                                    ID = products[j].ID,
                                    Name = products[j].Name,
                                    Description = products[j].Description,
                                    imageList = imageList,
                                    ProductSkus = proSkus
                                });
                            }//end if productSkus
                        }//end for product
                        
                    }
                }
                result = new JSONFormat<List<ProductViewModel>>
                {
                    status = new Status
                    {
                        success = true,
                        message = ConstantManager.MES_GET_PRODUCT_SUCCESS,
                        status = ConstantManager.STATUS_SUCCESS
                    },
                    data = productList
                };
                return result;
            }
            catch (Exception ex)
            {
                result = new JSONFormat<List<ProductViewModel>>
                {
                    status = new Status
                    {
                        success = false,
                        message = ex.Message,
                        status = ConstantManager.STT_FAIL
                    },
                    data = null
                };
            }
            return result;
        }

    }
}
