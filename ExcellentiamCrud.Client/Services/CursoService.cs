using ExcellentiamCrud.Shared;
using System.Net.Http.Json;

namespace ExcellentiamCrud.Client.Services
{
    public class CursoService : ICursoService
    {
        private readonly HttpClient _http;

        public CursoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CursoDTO>> getCursos(string nombrePatron = "")
        {
            var result = await _http.GetFromJsonAsync<ResponseAPI<List<CursoDTO>>>("api/curso/getCursos");

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<CursoDTO> ObtenerCursoPorId(int id)
        {
            var result = await _http.GetFromJsonAsync<ResponseAPI<CursoDTO>>($"api/curso/getCurso/{id}");

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<string> GuardarCurso(CursoDTO cursoDto)
        {
            var response = await _http.PostAsJsonAsync("api/curso/agregarCurso", cursoDto);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<string> ActualizarCurso(CursoDTO cursoDto)
        {
            var response = await _http.PutAsJsonAsync("api/curso/actualizarCurso", cursoDto);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<string> EliminarCurso(int id)
        {
            var response = await _http.DeleteAsync($"api/curso/eliminarCurso/{id}");
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<byte[]> DescargarCursosExcel()
        {
            var response = await _http.GetAsync("api/curso/descargar-cursos");

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                return fileBytes;
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();
                throw new Exception(result!.Message);
            }
        }

        public async Task<byte[]> DescargarEstudiantesExcel(int cursoId)
        {
            var response = await _http.GetAsync($"api/estudiante/descargar?cursoId={cursoId}");

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                return fileBytes;
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();
                throw new Exception(result!.Message);
            }
        }

    }
}
