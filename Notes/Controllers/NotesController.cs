using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Interface;
using Notes.Models;

namespace Notes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepo _context;
        public NotesController(INotesRepo context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Note>> GetAll()
        {
            try
            {
                var result = await _context.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddNote(NotesParameter note)
        {
            try
            {
                var result = await _context.AddNote(note);

                if (note.Image!=null) {
                    var filePath = "Attachments/Note-" + result;
                    save_image(filePath, note.Image, "Image");
                }                  

                return Ok(result);              
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpPut]
        public async Task<ActionResult> UpdateNote(NotesParameter note)
        {
            try
            {
                var result = await _context.UpdateNote(note);

                if (!note.Image.Contains("Attachments"))
                 {
                    var filePath = "Attachments/Note-" + note.Id;
                    save_image(filePath, note.Image, "Image");
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteNote(int id)
        {
            try
            {
                var result = await _context.DeleteNote(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        public static void save_image(string filepath, string base64, string filename)
        {
            var base64Data1 = Regex.Match(base64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            byte[] imageBytes1 = Convert.FromBase64String(base64Data1);

            using (Image image = Image.FromStream(new MemoryStream(imageBytes1)))
            {

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                    Console.WriteLine("Directory Created!");
                }
                else
                {
                    Console.WriteLine("Directory Exists!");
                }
                image.Save(filepath + "/" + filename + ".jpg");
            }

        }


    }
}