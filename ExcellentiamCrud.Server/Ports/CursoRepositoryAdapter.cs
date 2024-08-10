using ExcellentiamCrud.Server.Models;
using Microsoft.EntityFrameworkCore;
using ExcellentiamCrud.Shared;
using ExcellentiamCrud.Server.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExcellentiamCrud.Server.Repositories;
using ClosedXML.Excel;

namespace ExcellentiamCrud.Server.Ports
{
    public class CursoRepositoryAdapter : ICursoRepositoryPort
    {
        private readonly ExcellentiamTestDbContext _context;

        public CursoRepositoryAdapter(ExcellentiamTestDbContext dbContext)
        {
            _context = dbContext;
        }

   
        // Obtener cursos por patrón en el nombre
        public async Task<List<CursoDTO>> ObtenerCursos(string nombrePatron = "")
        {
            var cursos = await _context.Cursos
                .Where(cur => string.IsNullOrEmpty(nombrePatron) || cur.NombreCurso.Contains(nombrePatron))
                .Select(cur => new CursoDTO
                {
                    CursoId = cur.CursoId,
                    NombreCurso = cur.NombreCurso,
                    Descripcion = cur.Descripcion,
                    FechaInicio = cur.FechaInicio,
                    FechaFin = cur.FechaFin
                })
                .ToListAsync();

            return cursos;
        }

        //Obtener curso por id
        public async Task<CursoDTO> ObtenerCursoPorId(int id)
        {
            var curso = await _context.Cursos
                .Where(cur => cur.CursoId == id)
                .Select(cur => new CursoDTO
                {
                    CursoId = cur.CursoId,
                    NombreCurso = cur.NombreCurso,
                    Descripcion = cur.Descripcion,
                    FechaInicio = cur.FechaInicio,
                    FechaFin = cur.FechaFin
                })
                .FirstOrDefaultAsync();

            return curso;
        }

        //Guardar un curso
        public async Task<string> GuardarCurso(CursoDTO cursoDto)
        {
            var curso = new Curso
            {
                NombreCurso = cursoDto.NombreCurso,
                Descripcion = cursoDto.Descripcion,
                FechaInicio = cursoDto.FechaInicio,
                FechaFin = cursoDto.FechaFin
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return $"Curso '{curso.NombreCurso}' guardado con éxito con ID {curso.CursoId}.";
        }

        //Actualizar curso
        public async Task<string> ActualizarCurso(CursoDTO cursoDto)
        {
            var cursoExistente = await _context.Cursos.FindAsync(cursoDto.CursoId);

            if (cursoExistente == null)
            {
                return "Curso no encontrado.";
            }

            cursoExistente.NombreCurso = cursoDto.NombreCurso;
            cursoExistente.Descripcion = cursoDto.Descripcion;
            cursoExistente.FechaInicio = cursoDto.FechaInicio;
            cursoExistente.FechaFin = cursoDto.FechaFin;

            await _context.SaveChangesAsync();

            return $"Curso '{cursoExistente.NombreCurso}' actualizado con éxito.";
        }

        //Eliminar curso
        public async Task<string> EliminarCurso(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Estudiantes)
                .FirstOrDefaultAsync(c => c.CursoId == id);

            if (curso == null)
            {
                return "Curso no encontrado.";
            }

            if (curso.Estudiantes.Any())
            {
                return "El curso no se puede eliminar porque tiene estudiantes asociados.";
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return $"Curso '{curso.NombreCurso}' eliminado con éxito.";
        }

        //Descargar Cursos
        public async Task<byte[]> DescargarCursosExcel()
        {
            var cursos = await ObtenerCursos("");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Cursos");

            worksheet.Cell(1, 1).Value = "CursoId";
            worksheet.Cell(1, 2).Value = "NombreCurso";
            worksheet.Cell(1, 3).Value = "Descripcion";
            worksheet.Cell(1, 4).Value = "FechaInicio";
            worksheet.Cell(1, 5).Value = "FechaFin";

            for (int i = 0; i < cursos.Count; i++)
            {
                var cur = cursos[i];
                worksheet.Cell(i + 2, 1).Value = cur.CursoId;
                worksheet.Cell(i + 2, 2).Value = cur.NombreCurso;
                worksheet.Cell(i + 2, 3).Value = cur.Descripcion;
                worksheet.Cell(i + 2, 4).Value = cur.FechaInicio.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2, 5).Value = cur.FechaFin.ToString("yyyy-MM-dd");
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
