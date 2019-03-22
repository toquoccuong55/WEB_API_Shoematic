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
    public class OrderController : ApiController
    {
        OrderRepository orderRepository = new OrderRepository();
        CustomerRepository customerRepository = new CustomerRepository();
        OrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        ProductSkuRepository productSkuRepository = new ProductSkuRepository();
        ProductRepository productRepository = new ProductRepository();
        ProductImageRepository imageRepository = new ProductImageRepository();


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("getOrderHistory")]
        public JSONFormat<List<OrderViewModel>> OrderHistory([FromBody]LoginViewModel loginViewModel)
        {
            JSONFormat<List<OrderViewModel>> result = null;
            List<OrderViewModel> orderViewModelList = new List<OrderViewModel>();

            string customerId = Utilities.getCustomerIDFromToken(loginViewModel.accessToken);
            Customer existedCustomer = customerRepository.Find(customerId);
            try
            {
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
                            orderViewModel.AccessToken = loginViewModel.accessToken;
                            orderViewModel.ShippingAddress = order.ShippingAddress;
                            orderViewModel.PaymentType = order.Payment;
                            orderViewModel.Note = order.Note;

                            double totalAmount = 0;
                            List<OrderDetailViewModel> orderDetailViewModelList = new List<OrderDetailViewModel>();

                            var orderDetaiList = orderDetailRepository.All().Where(p => p.OrderID == order.ID);
                            if (orderDetaiList != null)
                            {
                                foreach (var item in orderDetaiList)
                                {
                                    int productSkuID = item.ProductSkuID;
                                    ProductSku productSku = productSkuRepository.Find(p => p.ID == productSkuID);
                                    double size = productSku.Size.Value;

                                    int parentProId = productSku.ProductID;
                                    Product parentProduct = productRepository.Find(p => p.ID == parentProId);
                                    string name = parentProduct.Name;

                                    string imageURL = imageRepository.All().Where(p => p.ProductID == parentProId).First().PicURL.ToString();

                                    OrderDetailViewModel orderDetail = new OrderDetailViewModel();
                                    orderDetail.ProductSkuID = productSkuID;
                                    orderDetail.imageURL = imageURL;
                                    orderDetail.Name = name;
                                    orderDetail.Size = size;
                                    orderDetail.UnitPrice = item.UnitPrice;
                                    orderDetail.Quantity = item.Quantity;

                                    orderDetailViewModelList.Add(orderDetail);

                                    totalAmount += item.Quantity * item.UnitPrice;
                                }
                            }
                            orderViewModel.TotalAmount = totalAmount;
                            orderViewModel.OrderDetailList = orderDetailViewModelList;

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
                else {
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
            }
            catch (Exception ex)
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

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("setOrder")]
        public JSONFormat<OrderSuccessViewModel> SetOrder([FromBody] OrderViewModel orderViewModel)
        {
            JSONFormat<OrderSuccessViewModel> result = null;
            try
            {
                string customerID = Utilities.getCustomerIDFromToken(orderViewModel.AccessToken);
                Customer existedCustomer = customerRepository.Find(customerID);
                if (existedCustomer != null)
                {
                    //insert order to DB
                    Order order = new Order();
                    order.CustomerID = customerID;
                    order.CreatedDate = DateTime.Now;
                    order.ShippingAddress = orderViewModel.ShippingAddress;
                    order.ShippingFee = 0;
                    if (orderViewModel.PaymentType == "CASH")
                    {
                        order.Payment = "COD";
                    }
                    else
                    {
                        order.Payment = "Other payment";
                    }
                    order.Note = orderViewModel.Note;
                    order.Status = 1;

                    Order insertedOrder = orderRepository.Create(order);

                    //insert orderDetail
                    List<OrderDetailViewModel> orderDetailList = orderViewModel.OrderDetailList;
                    foreach (OrderDetailViewModel item in orderDetailList)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderID = insertedOrder.ID;
                        orderDetail.ProductSkuID = item.ProductSkuID;
                        orderDetail.Quantity = item.Quantity;
                        orderDetail.UnitPrice = item.UnitPrice;

                        orderDetailRepository.Create(orderDetail);
                    }
                    //return result for client
                    result = new JSONFormat<OrderSuccessViewModel>
                    {
                        status = new Status()
                        {
                            success = true,
                            message = ConstantManager.MES_SET_ORDER_SUCCESS,
                            status = ConstantManager.STATUS_SUCCESS
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                result = new JSONFormat<OrderSuccessViewModel>
                {
                    status = new Status()
                    {
                        success = false,
                        message = ConstantManager.MES_SET_ORDER_FAIL,
                        status = ConstantManager.STT_FAIL
                    }
                };
            }

            return result;
        }

    }
}
