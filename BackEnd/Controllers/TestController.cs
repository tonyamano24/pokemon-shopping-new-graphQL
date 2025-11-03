using AutoMapper;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using itsc_dotnet_practice.Services;

namespace itsc_dotnet_practice.Controllers;

[Route("api/test")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public TestController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetTest()
    {
        await _productService.TestAddProduct();
        return Ok();
    }
}
