using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExcellentiamCrud.Server.Repositories;
using ExcellentiamCrud.Shared;
using ExcellentiamCrud.Server.Ports;

namespace ExcellentiamCrud.Server.Controllers
{
    [Route("api/estudiante")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteRepositoryPort _repository;

        public EstudianteController(IEstudianteRepositoryPort repository)
        {
            _repository = repository;
        }

        [HttpGet("getEstudiantes")]
        public async Task<IActionResult> ObtenerEstudiantes(string nombrePatron = "")
        {
            var responseApi = new ResponseAPI<List<EstudianteDTO>>();

            try
            {
                var res = await _repository.ObtenerEstudiantes(nombrePatron);
                responseApi.Success = true;
                responseApi.Value = res;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpGet("getEstudiante/{id}")]
        public async Task<IActionResult> ObtenerEstudiantePorId(int id)
        {
            var responseApi = new ResponseAPI<EstudianteDTO>();

            try
            {
                var estudiante = await _repository.ObtenerEstudiantePorId(id);

                if (estudiante == null)
                {
                    responseApi.Success = false;
                    responseApi.Message = "Estudiante no encontrado.";
                    return NotFound(responseApi);
                }

                responseApi.Success = true;
                responseApi.Value = estudiante;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }


        [HttpPost("agregarEstudiante")]
        public async Task<IActionResult> AgregarEstudiante(EstudianteDTO estudianteDto)
        {
            var responseApi = new ResponseAPI<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    responseApi.Success = false;
                    responseApi.Message = "Datos del estudiante no válidos.";
                    return BadRequest(responseApi);
                }

                var mensaje = await _repository.AgregarEstudiante(estudianteDto);
                responseApi.Success = true;
                responseApi.Value = mensaje;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpPut("actualizarEstudiante")]
        public async Task<IActionResult> ActualizarEstudiante(EstudianteDTO estudianteDto)
        {
            var responseApi = new ResponseAPI<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    responseApi.Success = false;
                    responseApi.Message = "Datos del estudiante no válidos.";
                    return BadRequest(responseApi);
                }
                var mensaje = await _repository.ActualizarEstudiante(estudianteDto);

                if (mensaje.Contains("no encontrado"))
                {
                    responseApi.Success = false;
                    responseApi.Message = mensaje;
                    return NotFound(responseApi);
                }
                responseApi.Success = true;
                responseApi.Value = mensaje;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpDelete("eliminarEstudiante/{id}")]
        public async Task<IActionResult> EliminarEstudiante(int id)
        {
            var responseApi = new ResponseAPI<string>();

            try
            {
                var mensaje = await _repository.EliminarEstudiante(id);

                if (mensaje.Contains("no encontrado"))
                {
                    responseApi.Success = false;
                    responseApi.Message = mensaje;
                    return NotFound(responseApi);
                }
                responseApi.Success = true;
                responseApi.Value = mensaje;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpGet("descargar")]
        public async Task<IActionResult> DescargarEstudiantesExcel(int cursoId)
        {
            var archivoBytes = await _repository.DescargarEstudiantesExcel(cursoId);
            return File(archivoBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Estudiantes.xlsx");
        }


    }
}
