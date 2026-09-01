using Gamana_Muttopalvelu_Backend.DTO.Admin;
using Gamana_Muttopalvelu_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamana_Muttopalvelu_Backend.Controllers
{
    [ApiController]
    [Route("api/admin/key-services")]
    public class KeyServicesAdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public KeyServicesAdminController(IAdminService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllKeyServicesAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetKeyServiceByIdAsync(id));
        [HttpPost] public async Task<IActionResult> Create([FromBody] AdminKeyServiceUpsertDto dto) => Ok(await _service.SaveKeyServiceAsync(dto));
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, [FromBody] AdminKeyServiceUpsertDto dto) => Ok(await _service.SaveKeyServiceAsync(dto, id));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteKeyServiceAsync(id));
    }

    [ApiController]
    [Route("api/admin/process-steps")]
    public class ProcessStepsAdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public ProcessStepsAdminController(IAdminService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllProcessStepsAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetProcessStepByIdAsync(id));
        [HttpPost] public async Task<IActionResult> Create([FromBody] AdminProcessStepUpsertDto dto) => Ok(await _service.SaveProcessStepAsync(dto));
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, [FromBody] AdminProcessStepUpsertDto dto) => Ok(await _service.SaveProcessStepAsync(dto, id));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteProcessStepAsync(id));
    }

    [ApiController]
    [Route("api/admin/detailed-services")]
    public class DetailedServicesAdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public DetailedServicesAdminController(IAdminService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllDetailedServicesAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetDetailedServiceByIdAsync(id));
        [HttpPost] public async Task<IActionResult> Create([FromBody] AdminDetailedServiceUpsertDto dto) => Ok(await _service.SaveDetailedServiceAsync(dto));
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, [FromBody] AdminDetailedServiceUpsertDto dto) => Ok(await _service.SaveDetailedServiceAsync(dto, id));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteDetailedServiceAsync(id));
    }

    [ApiController]
    [Route("api/admin/packages")]
    public class PackagesAdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public PackagesAdminController(IAdminService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllPackagesAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetPackageByIdAsync(id));
        [HttpPost] public async Task<IActionResult> Create([FromBody] AdminPackageUpsertDto dto) => Ok(await _service.SavePackageAsync(dto));
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, [FromBody] AdminPackageUpsertDto dto) => Ok(await _service.SavePackageAsync(dto, id));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeletePackageAsync(id));
    }

    [ApiController]
    [Route("api/admin/reviews")]
    public class ReviewsAdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public ReviewsAdminController(IAdminService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllReviewsAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetReviewByIdAsync(id));
        [HttpPost] public async Task<IActionResult> Create([FromBody] AdminReviewUpsertDto dto) => Ok(await _service.SaveReviewAsync(dto));
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, [FromBody] AdminReviewUpsertDto dto) => Ok(await _service.SaveReviewAsync(dto, id));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteReviewAsync(id));
    }

    [ApiController]
    [Route("api/admin/form-options")]
    public class FormOptionsAdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public FormOptionsAdminController(IAdminService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllFormOptionsAsync());
        [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetFormOptionByIdAsync(id));
        [HttpPost] public async Task<IActionResult> Create([FromBody] AdminFormOptionUpsertDto dto) => Ok(await _service.SaveFormOptionAsync(dto));
        [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, [FromBody] AdminFormOptionUpsertDto dto) => Ok(await _service.SaveFormOptionAsync(dto, id));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteFormOptionAsync(id));
    }
}
