using DatabaseService.Entities;
using DatabaseService.Repositories;
using Facebook;
using Newtonsoft.Json.Linq;
using ShoesShopAPI.Models;
using ShoesShopAPI.Response;
using ShoesShopAPI.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace ShoesShopAPI.Controllers
{
    [System.Web.Mvc.RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        CustomerRepository customerRepository = new CustomerRepository();
        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("loginByPhone")]
        public JSONFormat<CustomerViewModel> LoginByPhone([FromBody]LoginViewModel loginModel)
        {
            JSONFormat<CustomerViewModel> result;
            var customerInfoJson = GetPhoneFromFacebook(loginModel.accessToken);
            try
            {
                var parsedObject = JObject.Parse(customerInfoJson);

                var phone = parsedObject["phone"]["number"].ToString();
                var id = parsedObject["id"].ToString();

                Customer existedCustomer = customerRepository.Find(c => c.ID == id);
                CustomerViewModel customerModel = null;
                if (existedCustomer == null)
                {
                    Customer customerDB = new Customer
                    {
                        ID = id,
                        Phone = phone
                    };

                    Customer createdCustomer = customerRepository.Create(customerDB);
                    if (createdCustomer != null)
                    {
                        string clientToken = Utilities.GenerateToken(id);
                        customerModel = new CustomerViewModel
                        {
                            access_token = clientToken,
                            phone = phone,
                            first_login = true
                        };
                    }
                }
                else
                {
                    string clientToken = Utilities.GenerateToken(id);
                    customerModel = new CustomerViewModel
                    {
                        access_token = clientToken,
                        name = existedCustomer.Name,
                        phone = phone,
                        first_login = false,
                        email = existedCustomer.Email,
                        address = existedCustomer.Address
                    };
                }
                if (customerModel != null)
                {
                    result = new JSONFormat<CustomerViewModel>
                    {
                        status = new Status
                        {
                            success = true,
                            message = ConstantManager.MES_LOGIN_SUCCESS,
                            status = ConstantManager.STATUS_SUCCESS
                        },
                        data = customerModel
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            result = new JSONFormat<CustomerViewModel>
            {
                status = new Status
                {
                    success = false,
                    message = ConstantManager.MES_LOGIN_FAIL,
                    status = ConstantManager.STT_FAIL
                },
                data = null
            };
            return result;
        }

        private string GetPhoneFromFacebook(string accessToken)
        {
            string fbclid = "IwAR3hJcRIbxSyBU6b_ZA3vLAycQ_FwpISrc6AFCmckJJE8v0hCpZ_cLYKo_I";
            string url = "https://graph.accountkit.com/v1.3/me/?access_token=" +
                accessToken + "&fbclid=" + fbclid;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                string jsonText;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    jsonText = sr.ReadToEnd();
                }
                return jsonText;
            }
            catch
            {
                return "";
            }
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("updateCustomer")]
        public JSONFormat<CustomerViewModel> UpdateCustomer([FromBody]CustomerViewModel customer)
        {
            JSONFormat<CustomerViewModel> result;

            try
            {
                if (customer != null)
                {
                    string accessToken = customer.access_token;

                    string customerID = Utilities.getCustomerIDFromToken(accessToken);
                    if (!customerID.Equals(""))
                    {
                        string address = "";
                        Customer cus;
                        if (customer.address == null || customer.address.Length == 0)
                        {
                            Customer existedCus = customerRepository.Find(c => c.ID == customerID);
                            address = existedCus.Address;
                            cus = new Customer
                            {
                                ID = customerID,
                                Name = customer.name,
                                Email = customer.email,
                                Address = address
                            };
                        }
                        else
                        {
                            cus = new Customer
                            {
                                ID = customerID,
                                Name = customer.name,
                                Email = customer.email,
                                Address = customer.address
                            };
                        }
                        
                        bool update = customerRepository.updateCustomer(cus);
                        if (update)
                        {
                            customer.first_login = false;
                            result = new JSONFormat<CustomerViewModel>
                            {
                                status = new Status
                                {
                                    success = true,
                                    message = ConstantManager.MES_UPDATE_SUCCESS,
                                    status = ConstantManager.STATUS_SUCCESS
                                },
                                data = customer
                            };
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            result = new JSONFormat<CustomerViewModel>
            {
                status = new Status
                {
                    success = false,
                    message = ConstantManager.MES_UPDATE_FAIL,
                    status = ConstantManager.STT_FAIL
                },
                data = null
            };
            return result;
        }


    }
}
