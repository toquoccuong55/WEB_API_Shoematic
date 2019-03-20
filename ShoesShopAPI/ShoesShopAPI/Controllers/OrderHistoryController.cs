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
    [System.Web.Mvc.RoutePrefix("api/[controller]")]
    public class OrderHistoryController : ApiController
    {
        OrderRepository orderRepository = new OrderRepository();
        CustomerRepository customerRepository = new CustomerRepository();
        OrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        ProductSkuRepository productSkuRepository = new ProductSkuRepository();
        ProductRepository productRepository = new ProductRepository();
        ProductImageRepository imageRepository = new ProductImageRepository();


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("loginByPhone")]
        public JSONFormat<List<OrderViewModel>> SetOrderHistory([FromBody]LoginViewModel loginViewModel)
        {
            JSONFormat<List<OrderViewModel>> result = null;
            List<OrderViewModel> orderViewModelList = new List<OrderViewModel>();

            string customerId = Utilities.getCustomerIDFromToken(loginViewModel.accessToken);
            Customer existedCustomer = customerRepository.Find(customerId);

            if (existedCustomer != null)
            {
                var orderHistoryList = orderRepository.All().Where(p => p.CustomerID == customerId);
                if (orderHistoryList != null)
                {
                    foreach (var order in orderHistoryList)
                    {
                        OrderViewModel orderViewModel = new OrderViewModel();
                        orderViewModel.OrderId = order.ID;
                        orderViewModel.OrderTime = order.CreatedDate.ToString();
                        orderViewModel.Status = order.Status;

                        List<OrderDetailViewModel> orderDetailViewModelList = new List<OrderDetailViewModel>();

                        var orderDetaiList = orderDetailRepository.All().Where(p => p.OrderID == order.ID);
                        if (orderDetaiList != null)
                        {
                            foreach (var item in orderDetaiList)
                            {
                                int productSkuID = item.ID;
                                ProductSku productSku = productSkuRepository.Find(p => p.ID == productSkuID);
                                double size = productSku.Size.Value;

                                int parentProId = productSku.ProductID;
                                Product parentProduct = productRepository.Find(p => p.ID == parentProId);
                                string name = parentProduct.Name;

                                string imageURL = imageRepository.All().Where(p => p.ProductID == parentProId).First().ToString();

                                OrderDetailViewModel orderDetail = new OrderDetailViewModel();
                                orderDetail.imageURL = imageURL;
                                orderDetail.Name = name;
                                orderDetail.Size = size;
                                orderDetail.UnitPrice = item.UnitPrice;
                                orderDetail.Quantity = item.Quantity;

                                orderDetailViewModelList.Add(orderDetail);
                            }
                        }
                        orderViewModel.OrderDetailList.AddRange(orderDetailViewModelList);

                        orderViewModelList.Add(orderViewModel);
                    }
                    result = new JSONFormat<List<OrderViewModel>>
                    {
                        status = new Status()
                        {
                            success = true,
                            message = ConstantManager.MES_GET_ORDER_SUCCESS,
                            status = ConstantManager.STATUS_SUCCESS
                        },
                        data = orderViewModelList
                    };
                }
            }
            else
            {
                result = new JSONFormat<List<OrderViewModel>>
                {
                    status = new Status()
                    {
                        success = false,
                        message = ConstantManager.MES_GET_ORDER_FAIL,
                        status = ConstantManager.STT_FAIL
                    },
                    data = null
                };
            }

            return result;
        }

    }
}
