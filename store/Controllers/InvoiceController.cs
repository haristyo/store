using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using store.core.Entities;
using store.core.Services;
using store.core.Spesifications;
using store.Profiles.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        public InvoiceController(IInvoiceService invoiceService, IMapper mapper, IItemService itemService)
        {
            _itemService = itemService;
            _invoiceService = invoiceService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<InvoiceDTO>>> GetAll(CancellationToken cancellationToken = default)
        {
            InvoiceSpesification spesification = new InvoiceSpesification();
            List<Invoice> listInvoice = await _invoiceService.GetList(spesification.Build(), cancellationToken);
            List<InvoiceDTO> listInvoiceDTO = _mapper.Map<List<Invoice>, List<InvoiceDTO>>(listInvoice);
            return Ok(listInvoiceDTO);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemDTO>> GetSingle([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            Invoice invoice = await _invoiceService.getSingle(id, cancellationToken);
            InvoiceDTO result = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            if (await _invoiceService.Delete(id, cancellationToken) == false)
            {
                Dictionary<string, List<string>> errors = _invoiceService.GetError();
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

        //public async Task<ActionResult<InvoiceDTO>> Insert(CancellationToken cancellationToken = default)
        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> Insert([FromBody] InvoiceDTO model, CancellationToken cancellationToken = default)
        {
            Invoice invoiceInserted = _mapper.Map<InvoiceDTO, Invoice>(model);  
            //foreach(InvoiceDetailDTO invoiceDetailDTO in model.InvoiceDetails)
            //{
            //    invoiceInserted.addInvoiceDetailFull(_mapper.Map<InvoiceDetailDTO, InvoiceDetail>(invoiceDetailDTO));
            //}

            if (await _invoiceService.Insert(invoiceInserted, cancellationToken) == null)
            {
                Dictionary<string, List<string>> errors = _invoiceService.GetError();
                foreach (KeyValuePair<string, List<string>> error in errors)
                {
                    foreach (string errorValue in error.Value)
                    {
                        ModelState.AddModelError(error.Key, errorValue);
                    }
                }
                return BadRequest(ModelState);
            }
            InvoiceDTO result = _mapper.Map<Invoice, InvoiceDTO>(await _invoiceService.getSingle(invoiceInserted.Id));
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromBody] InvoiceDTO model, [FromRoute] int id, CancellationToken cancellationToken = default)
        {
            Invoice invoiceInserted = _mapper.Map<InvoiceDTO, Invoice>(model);
            //foreach (InvoiceDetailDTO invoiceDetailDTO in model.InvoiceDetails)
            //{
            //    InvoiceDetail invoiceDetail = _mapper.Map<InvoiceDetailDTO, InvoiceDetail>(invoiceDetailDTO);
            //    //invoiceDetail.InvoiceID = invoiceInserted.Id;
            //    invoiceInserted.addInvoiceDetailFull(invoiceDetail);
            //}

            if (await _invoiceService.Update(invoiceInserted, id, cancellationToken) == null)
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

            InvoiceDTO result = _mapper.Map<Invoice, InvoiceDTO>(await _invoiceService.getSingle(id));
            return Ok(result);
        }

    }
}
