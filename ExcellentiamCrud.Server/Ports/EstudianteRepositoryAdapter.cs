using ExcellentiamCrud.Server.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExcellentiamCrud.Server.Repositories;
using ExcellentiamCrud.Server.Models;
using ExcellentiamCrud.Shared;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace ExcellentiamCrud.Server.Ports
{
    public class EstudianteRepositoryAdapter : IEstudianteRepositoryPort
    {
        private readonly ExcellentiamTestDbContext _context;

        public EstudianteRepositoryAdapter(ExcellentiamTestDbContext context)
        {
            _context = context;
        }

        //Obtener todos los estudiantes

        public async Task<List<EstudianteDTO>> ObtenerEstudiantes(string nombrePatron = "")
        {
            var estudiantes = await _context.Estudiantes
                .Include(c => c.Curso)
                .Where(estu => string.IsNullOrEmpty(nombrePatron) ||
                               estu.Nombre.Contains(nombrePatron) ||
                               estu.Apellido.Contains(nombrePatron))
                .Select(estu => new EstudianteDTO
                {
                    EstudianteId = estu.EstudianteId,
                    Nombre = estu.Nombre,
                    Apellido = estu.Apellido,
                    FechaNacimiento = estu.FechaNacimiento,
                    CorreoElectronico = estu.CorreoElectronico,
                    Telefono = estu.Telefono,
                    Direccion = estu.Direccion,
                    FechaRegistro = estu.FechaRegistro,
                    Activo = estu.Activo,
                    CursoId = estu.CursoId,
                    Curso = estu.Curso != null ? new CursoDTO
                    {
                        CursoId = estu.Curso.CursoId,
                        NombreCurso = estu.Curso.NombreCurso,
                        Descripcion = estu.Curso.Descripcion,
                        FechaInicio = estu.Curso.FechaInicio,
                        FechaFin = estu.Curso.FechaFin
                    } : null
                })
                .ToListAsync();

            return estudiantes;
        }

        //Obtener estudiante por id
        public async Task<EstudianteDTO> ObtenerEstudiantePorId(int id)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.Curso)
                .Where(e => e.EstudianteId == id)
                .Select(e => new EstudianteDTO
                {
                    EstudianteId = e.EstudianteId,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    FechaNacimiento = e.FechaNacimiento,
                    CorreoElectronico = e.CorreoElectronico,
                    Telefono = e.Telefono,
                    Direccion = e.Direccion,
                    FechaRegistro = e.FechaRegistro,
                    Activo = e.Activo,
                    CursoId = e.CursoId,
                    Curso = e.Curso != null ? new CursoDTO
                    {
                        CursoId = e.Curso.CursoId,
                        NombreCurso = e.Curso.NombreCurso,
                        Descripcion = e.Curso.Descripcion,
                        FechaInicio = e.Curso.FechaInicio,
                        FechaFin = e.Curso.FechaFin
                    } : null
                })
                .FirstOrDefaultAsync();
            return estudiante;
        }

        // Guardar estudiante
        public async Task<string> AgregarEstudiante(EstudianteDTO estudianteDto)
        {
            var estudiante = new Estudiante
            {
                Nombre = estudianteDto.Nombre,
                Apellido = estudianteDto.Apellido,
                FechaNacimiento = estudianteDto.FechaNacimiento,
                CorreoElectronico = estudianteDto.CorreoElectronico,
                Telefono = estudianteDto.Telefono,
                Direccion = estudianteDto.Direccion,
                FechaRegistro = DateTime.Now,
                Activo = true,
                CursoId = estudianteDto.CursoId
            };

            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return $"Estudiante '{estudiante.Nombre} {estudiante.Apellido}' agregado con éxito.";
        }

        //Actualizar estudiante
        public async Task<string> ActualizarEstudiante(EstudianteDTO estudianteDto)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.Curso)
                .FirstOrDefaultAsync(e => e.EstudianteId == estudianteDto.EstudianteId);

            if (estudiante == null)
            {
                return "Estudiante no encontrado.";
            }

            estudiante.Nombre = estudianteDto.Nombre ?? estudiante.Nombre;
            estudiante.Apellido = estudianteDto.Apellido ?? estudiante.Apellido;
            estudiante.FechaNacimiento = estudianteDto.FechaNacimiento != default ? estudianteDto.FechaNacimiento : estudiante.FechaNacimiento;
            estudiante.CorreoElectronico = estudianteDto.CorreoElectronico ?? estudiante.CorreoElectronico;
            estudiante.Telefono = estudianteDto.Telefono ?? estudiante.Telefono;
            estudiante.Direccion = estudianteDto.Direccion ?? estudiante.Direccion;
            estudiante.Activo = estudianteDto.Activo ?? estudiante.Activo;
            estudiante.CursoId = estudianteDto.CursoId != 0 ? estudianteDto.CursoId : estudiante.CursoId;

            await _context.SaveChangesAsync();
            return $"Estudiante '{estudiante.Nombre} {estudiante.Apellido}' actualizado con éxito.";
        }

        //Eliminar estudiante
        public async Task<string> EliminarEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.Curso)
                .FirstOrDefaultAsync(e => e.EstudianteId == id);

            if (estudiante == null)
            {
                return "Estudiante no encontrado.";
            }

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return $"Estudiante '{estudiante.Nombre} {estudiante.Apellido}' eliminado con éxito.";
        }

        private async Task<List<EstudianteDTO>> ObtenerEstudiantesPorCursoId(int cursoId)
        {

            return await _context.Estudiantes
                .Where(e => e.CursoId == cursoId)
                .Select(e => new EstudianteDTO
                {
                    EstudianteId = e.EstudianteId,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    FechaNacimiento = e.FechaNacimiento,
                    CorreoElectronico = e.CorreoElectronico,
                    Telefono = e.Telefono,
                    Direccion = e.Direccion,
                    FechaRegistro = e.FechaRegistro,
                    Activo = e.Activo,
                    CursoId = e.CursoId,
                    Curso = new CursoDTO
                    {
                        CursoId = e.Curso.CursoId,
                        NombreCurso = e.Curso.NombreCurso,
                        Descripcion = e.Curso.Descripcion,
                        FechaInicio = e.Curso.FechaInicio,
                        FechaFin = e.Curso.FechaFin
                    }
                })
                .ToListAsync();
        }


        public async Task<byte[]> DescargarEstudiantesExcel(int cursoId)
        {
            var estudiantes = await ObtenerEstudiantesPorCursoId(cursoId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Estudiantes");

            worksheet.Cell(1, 1).Value = "EstudianteId";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Apellido";
            worksheet.Cell(1, 4).Value = "FechaNacimiento";
            worksheet.Cell(1, 5).Value = "CorreoElectronico";
            worksheet.Cell(1, 6).Value = "Telefono";
            worksheet.Cell(1, 7).Value = "Direccion";
            worksheet.Cell(1, 8).Value = "FechaRegistro";
            worksheet.Cell(1, 9).Value = "Activo";
            worksheet.Cell(1, 10).Value = "CursoId";
            worksheet.Cell(1, 11).Value = "NombreCurso";
            worksheet.Cell(1, 12).Value = "Descripcion";
            worksheet.Cell(1, 13).Value = "FechaInicio";
            worksheet.Cell(1, 14).Value = "FechaFin";

            for (int i = 0; i < estudiantes.Count; i++)
            {
                var estu = estudiantes[i];
                worksheet.Cell(i + 2, 1).Value = estu.EstudianteId;
                worksheet.Cell(i + 2, 2).Value = estu.Nombre;
                worksheet.Cell(i + 2, 3).Value = estu.Apellido;
                worksheet.Cell(i + 2, 4).Value = estu.FechaNacimiento.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2, 5).Value = estu.CorreoElectronico;
                worksheet.Cell(i + 2, 6).Value = estu.Telefono;
                worksheet.Cell(i + 2, 7).Value = estu.Direccion;
                worksheet.Cell(i + 2, 8).Value = estu.FechaRegistro.HasValue
                    ? estu.FechaRegistro.Value.ToString("yyyy-MM-dd")
                    : "N/A";
                worksheet.Cell(i + 2, 9).Value = estu.Activo.HasValue
                    ? (estu.Activo.Value ? "Sí" : "No")
                    : "Desconocido";
                worksheet.Cell(i + 2, 10).Value = estu.CursoId;
                if (estu.Curso != null)
                {
                    worksheet.Cell(i + 2, 11).Value = estu.Curso.NombreCurso;
                    worksheet.Cell(i + 2, 12).Value = estu.Curso.Descripcion;
                    worksheet.Cell(i + 2, 13).Value = estu.Curso.FechaInicio.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 2, 14).Value = estu.Curso.FechaFin.ToString("yyyy-MM-dd");
                }
                else
                {
                    worksheet.Cell(i + 2, 11).Value = "N/A";
                    worksheet.Cell(i + 2, 12).Value = "N/A";
                    worksheet.Cell(i + 2, 13).Value = "N/A";
                    worksheet.Cell(i + 2, 14).Value = "N/A";
                }
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }



    }
}
