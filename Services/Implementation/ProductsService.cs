using Data.Repositorys;
using Data.Repositorys.Products;
using DomainModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services.Implementation
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productrepo;
        private IUnitOfWork _unitofwork { get; set; }

        public ProductsService(IProductsRepository productrepo, IUnitOfWork unitofwork)
        {
            _productrepo = productrepo;
            _unitofwork = unitofwork;
        }

        private bool IsNameTaken(string productname)
        {
            IEnumerable<Product> lstproducts = _productrepo.GetAll();

            if (lstproducts.Any(c => c.Name == productname))
                return true;
            else return false;
        }

        public string CreateProduct(Product product)
        {
            if (IsNameTaken(product.Name))
                return "Product already exists";
            else
            {
                if (product.Name.Contains(" "))
                {
                    product.Name.Replace(" ", "-").ToLower();
                }

                product.Slug = product.Name.Replace(" ", "-").ToLower();

                _productrepo.Add(product);
                _unitofwork.Save();
            }
            return string.Empty;
        }

        public void DeleteProduct(int id)
        {
            Product product = _productrepo.GetById(id);
            _productrepo.Delete(product);
            _unitofwork.Save();
        }

        public void EditProduct(Product product)
        {
            _productrepo.Edit(product);
            _unitofwork.Save();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productrepo.GetAll();
        }

        public Product GetProduct(int id)
        {
            return _productrepo.GetById(id);
        }
        
    }
}

