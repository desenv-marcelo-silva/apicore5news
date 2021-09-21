using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Catalog.Entities;
using Catalog.Repositories;
using System.Linq;
using Catalog.Dtos;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private readonly IItemsRepository repository;

    public ItemsController(IItemsRepository repository)
    {
        this.repository = repository;
    }

    // GET /items
    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
      var items = (await repository.GetItemsAsync())
                  .Select(item => item.AsDto());

      return items;
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
      var item = await repository.GetItemAsync(id);

      if (item is null) {
        return NotFound();
      }

      return item.AsDto();
    }

    // POST /items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {
      Item item = new() {
        Id = Guid.NewGuid(),
        Name = itemDto.Name,
        Price = itemDto.Price,
        CreateDate = DateTimeOffset.UtcNow
      };

      await repository.CreateItemAsync(item);

      return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
    }

    // PUT /items/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto item) 
    {
      var existinItem = await repository.GetItemAsync(id);

      if (existinItem is null) {
        return NotFound();
      }

      Item updatedItem = existinItem with {
        Name = item.Name,
        Price = item.Price
      };

      await repository.UpdateItemAsync(updatedItem);

      return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemAsync(Guid id)
    {
      var existinItem = await repository.GetItemAsync(id);

      if (existinItem is null) {
        return NotFound();
      }

      await repository.DeleteItemAsync(id);

      return NoContent();
    }
  }
}