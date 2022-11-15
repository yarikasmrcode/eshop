using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.DTOs;
using Shop.Api.Entities;
using Shop.Api.Errors;
using Shop.Api.Helpers;
using Shop.Core.Entities;
using Shop.Core.Interfaces;
using Shop.Core.Specifications;

namespace Shop.Api.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductType> _typesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
                                  IGenericRepository<ProductBrand> brandsRepo,
                                  IGenericRepository<ProductType> typesRepo,
                                  IMapper mapper)
        {
            _productsRepo = productsRepo;
            _brandsRepo = brandsRepo;
            _typesRepo = typesRepo;   
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts
            ([FromQuery] ProductSpecificationParams productParams)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(specification);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, 
                                                         productParams.PageSize,
                                                         totalItems, data));    
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(id);
           
            var product = await _productsRepo.GetEntityWithSpecification(specification);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _brandsRepo.GetAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductTypes()
        {
            return Ok(await _typesRepo.GetAllAsync());
        }
    }
}
