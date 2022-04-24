using Microsoft.EntityFrameworkCore;
using Notes.Interface;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace Notes.Repository
{
    public class NotesRepo : INotesRepo
    {
        private readonly NotesContext _context;

        public static object Image { get; private set; }

        #region Constructor
        public NotesRepo(NotesContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<IEnumerable<Note>> GetAll()
        {
            var customer = await _context.Notes.Where(_ => _.Active == 1).ToListAsync();

            if (customer == null)
                return null;
            return customer;
        }

        public async Task<dynamic> AddNote(NotesParameter note)
        {
            Note nt = new Note();
            nt.Description = note.Description;
            nt.Title = note.Title;
            nt.Contents = note.Contents;
            
            _context.Notes.Add(nt);
            await _context.SaveChangesAsync();

            var res = await _context.Notes.FindAsync(nt.Id);
            res.Image = "/Attachments/Note-" + nt.Id + "/Image.jpg";

            await _context.SaveChangesAsync();

            return nt.Id;

        }

        public async Task<dynamic> UpdateNote(NotesParameter note)
        {

            var res = await _context.Notes.FindAsync(note.Id);

            if (res == null)
                return ("Note not found.");

            res.Title = note.Title;
            res.Description = note.Description;
            res.Contents = note.Contents;       
            res.DateModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return "Updated Successfully";

        }

        public async Task<dynamic> DeleteNote(int id)
        {

            var res = await _context.Notes.FindAsync(id);

            if (res == null)
                return "Note not found.";

            res.Active = 0;
            res.DateDeleted = DateTime.Now;

            await _context.SaveChangesAsync();

            return "Deleted Successfully";

        }

    }
}

