﻿using API.DTOs.API.Entities.Models;
using API.Entities.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]/[action]")]
[ApiController]
public class ShoppingListController : ControllerBase
{
    private readonly IShoppingListService _shoppingListService;

    public ShoppingListController(IShoppingListService shoppingListService)
    {
        _shoppingListService = shoppingListService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingListDto>> GetShoppingList(int id)
    {
        var shoppingList = await _shoppingListService.Get(id);
        if (shoppingList == null)
        {
            return NotFound();
        }
        return Ok(shoppingList);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShoppingListDto>>> GetAllShoppingLists()
    {
        var shoppingLists = await _shoppingListService.GetAll();
        return Ok(shoppingLists);
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingListDto>> Save([FromBody] ShoppingListDto shoppingListDto)
    {
        var createdShoppingList = await _shoppingListService.Save(shoppingListDto);

        return CreatedAtAction(nameof(GetShoppingList), new { id = createdShoppingList.Id }, createdShoppingList);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShoppingListDto>> UpdateShoppingList(int id, [FromBody] ShoppingListDto shoppingListDto)
    {
        if (id != shoppingListDto.Id)
        {
            return BadRequest("Mismatched ID in request body");
        }

        var updatedShoppingList = await _shoppingListService.Save(shoppingListDto);
        if (updatedShoppingList == null)
        {
            return NotFound();
        }

        return Ok(updatedShoppingList);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ShoppingListDto>> DeleteShoppingList(int id)
    {
        var deletedShoppingList = await _shoppingListService.Delete(id);
        if (deletedShoppingList == null)
        {
            return NotFound();
        }

        return Ok(deletedShoppingList);
    }

    [HttpDelete]
    public async Task<ActionResult<int>> DeleteShoppingLists([FromBody] IEnumerable<int> ids)
    {
        var affectedRows = await _shoppingListService.Delete(ids);
        return Ok(affectedRows);
    }
}