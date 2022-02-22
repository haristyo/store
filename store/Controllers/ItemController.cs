using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using store.core.Entities;
using store.core.Services;
using store.core.Spesifications;
using store.data;
using store.Profiles.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace store.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class ItemController : ControllerBase
        {
            private readonly IItemService _itemService;
            //private readonly AppsContext _context;
            private readonly IMapper _mapper;
            public ItemController(IItemService itemService, IMapper mapper)
            {
                _itemService = itemService;
                _mapper = mapper;
            }
            [HttpGet]
            public async Task<ActionResult<List<ItemDTO>>> GetAll( CancellationToken cancellationToken = default)
            {
                ItemSpesification spesification = new ItemSpesification();
                //spesification.NameContains = "ana";
                List<Item> listItem = await _itemService.GetList(spesification.Build(), cancellationToken);
                List<ItemDTO> listItemDTO = _mapper.Map<List<Item>, List<ItemDTO>>(listItem);
                return Ok(listItemDTO);
            }
            [HttpGet("{id:int}")]
            public async Task<ActionResult<List<ItemDTO>>> GetSingle([FromRoute] int id, CancellationToken cancellationToken = default)
            {
            //ItemSpesification spesification = new ItemSpesification();
            //List<Item> listItem = await _itemService.GetList(spesification, cancellationToken);
            //List<ItemDTO> listItemDTO = _mapper.Map<List<Item>, List<ItemDTO>>(listItem);
            //return Ok(listItemDTO);
                Item item = await _itemService.getSingle(id, cancellationToken);
                //if(item==null)
                ItemDTO result = _mapper.Map<Item, ItemDTO>(item);
                return Ok(result);
            }

            [HttpPost]
            public async Task<ActionResult<ItemDTO>> Post([FromBody] ItemDTO model, CancellationToken cancellationToken = default)
            {
                Item item = _mapper.Map<ItemDTO, Item>(model);
                if(await _itemService.Insert(item,cancellationToken)== null)
                {
                    Dictionary<string, List<string>> errors = _itemService.GetError();
                    foreach(KeyValuePair<string, List<string>> error in errors)
                    {
                        foreach (string errorValue in error.Value)
                        {
                            ModelState.AddModelError(error.Key, errorValue);
                        }
                    }
                    return BadRequest(ModelState);
                }
                ItemDTO result = _mapper.Map<Item, ItemDTO>(item);
                return Ok(result);
            }

            [HttpDelete("{id:int}")]
            public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
            {
                if (await _itemService.Delete(id, cancellationToken) == false)
                {
                    Dictionary<string, List<string>> errors = _itemService.GetError();
                    foreach (KeyValuePair<string, List<string>> error in errors)
                    {
                        foreach (string errorValue in error.Value)
                        {
                            ModelState.AddModelError(error.Key, errorValue);
                        }
                    }
                    return BadRequest(ModelState);
                }
                return Ok("berhasil dihapus");
            }

            [HttpPut("{id:int}")]
            public async Task<ActionResult> Update([FromBody] ItemDTO model, [FromRoute] int id, CancellationToken cancellationToken = default)
            {
                Item item = _mapper.Map<ItemDTO, Item>(model);
                if (await _itemService.Update(item,id, cancellationToken) == null)
                {
                    Dictionary<string, List<string>> errors = _itemService.GetError();
                    foreach (KeyValuePair<string, List<string>> error in errors)
                    {
                        foreach (string errorValue in error.Value)
                        {
                            ModelState.AddModelError(error.Key, errorValue);
                        }
                    }
                    return BadRequest(ModelState);
                }
                ItemDTO result = _mapper.Map<Item, ItemDTO>(item);
                return Ok(result);
            }

    }
}
