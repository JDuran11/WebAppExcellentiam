using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExcellentiamCrud.Server.Repositories;
using ExcellentiamCrud.Shared;
using ExcellentiamCrud.Server.Ports;

namespace ExcellentiamCrud.Server.Controllers
{
    [Route("api/curso")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepositoryPort _repository;

        public CursoController(ICursoRepositoryPort repository)
        {
            _repository = repository;
        }

        [HttpGet("getCursos")]
        public async Task<IActionResult> ObtenerCursos(string nombrePatron = "")
        {
            var responseApi = new ResponseAPI<List<CursoDTO>>();

            try
            {
                var res = await _repository.ObtenerCursos(nombrePatron);
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

        [HttpGet("getCurso/{id}")]
        public async Task<IActionResult> ObtenerCursoPorId(int id)
        {
            var responseApi = new ResponseAPI<CursoDTO>();

            try
            {
                var curso = await _repository.ObtenerCursoPorId(id);

                if (curso == null)
                {
                    responseApi.Success = false;
                    responseApi.Message = "Curso no encontrado.";
                    return NotFound(responseApi);
                }

                responseApi.Success = true;
                responseApi.Value = curso;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }


        [HttpPost("agregarCurso")]
        public async Task<IActionResult> GuardarCurso(CursoDTO cursoDto)
        {
            var responseApi = new ResponseAPI<string>();

            try
            {
                var mensaje = await _repository.GuardarCurso(cursoDto);
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

        [HttpPut("actualizarCurso")]
        public async Task<IActionResult> ActualizarCurso(CursoDTO cursoDto)
        {
            var responseApi = new ResponseAPI<string>();

            try
            {
                var mensaje = await _repository.ActualizarCurso(cursoDto);
                responseApi.Success = mensaje.Contains("actualizado con éxito");
                responseApi.Value = mensaje;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpDelete("eliminarCurso/{id}")]
        public async Task<IActionResult> EliminarCurso(int id)
        {
            var responseApi = new ResponseAPI<string>();

            try
            {
                var mensaje = await _repository.EliminarCurso(id);

                if (mensaje.Contains("no se puede eliminar"))
                {
                    responseApi.Success = false;
                    responseApi.Message = mensaje;
                    return BadRequest(responseApi);
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

        [HttpGet("descargar-cursos")]
        public async Task<IActionResult> DescargarCursosExcel()
        {
            var archivoBytes = await _repository.DescargarCursosExcel();
            return File(archivoBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Cursos.xlsx");
        }



    }
}
